using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
///     Enemy Manager
/// </summary>
public class EnemyManager
{
    //  This is not the mono behaviour, so you can't use the Awake()
    public static EnemyManager Instance = new EnemyManager();
    private List<Enemy> _enemyList;

    /// <summary>
    /// Load Enemy Resources
    /// </summary>
    /// <param name="levelID">The ID of the level</param>
    public void LoadEnemyResources(string levelID)
    {
        _enemyList = new List<Enemy>();
        //  Content stored in the txt
        //  e.g. -> -------------------------------------------------------------------
        //          Id       levelName   EnemyIds                Pos
        //          10003    3           10001 = 10002 = 10003   3,0,1 = 0,0,1 = -3,0,1
        //          --------------------------------------------------------------------
        
        
        //  Load the content in the level table.
        Dictionary<string, string> levelData = GameConfigManager.Instance.GetLevelInfoById(levelID);

        //  Get the information about enemy's id
        string[] enemyIDList = levelData["EnemyIds"].Split('=');
        //  Get the information about enemy's Pos
        //  TODO:   The Position data should be formatted into grid location. 
        string[] enemyPosList = levelData["Pos"].Split('=');
        
        //  Init Enemy By ID
        for (int i = 0; i < enemyIDList.Length; i++)
        {
            string enemyID = enemyIDList[i];
            string[] enemyPos = enemyPosList[i].Split(',');
            
            //  Fetch Transformation of enemy.
            float enemy_x = float.Parse(enemyPos[0]);
            float enemy_y = float.Parse(enemyPos[1]);
            float enemy_z = float.Parse(enemyPos[2]);

            int enemy_x_bu = int.Parse(enemyPos[0]);
            int enemy_y_bu = int.Parse(enemyPos[1]);
            
            BoardUnit boardUnit = MapManager.Instance.fightBoard_dict[new Vector2Int(enemy_x_bu, enemy_y_bu)];
            if (boardUnit == null)
            {
                Debug.Log("[EnemyManager]: Wrong Spawn Point!");
            }
            
            //  Get enemy information by ID
            Dictionary<string, string> enemyData = GameConfigManager.Instance.GetEnemyInfoById(enemyID);
            
            //  Instantiate
            //  ["Model"]:  Path to enemy prefab.
            GameObject enemyObj = Object.Instantiate(Resources.Load(enemyData["Model"])) as GameObject;
            if (enemyObj == null)
            {
                Debug.Log(enemyData["Model"]);
                Debug.Log("[EnemyManager]: This enemy is null");
            }
            
            //  Load the Enemy Script for this enemy gameobject.
            Enemy enemyScript = enemyObj.AddComponent<Enemy>();
            enemyScript.InitEnemy(enemyData);
            _enemyList.Add(enemyScript);
            
            PositionEnemyOnBoardUnit(boardUnit, enemyScript);
            //enemyObj.transform.position = new Vector3(enemy_x, enemy_y, enemy_z);
        }
    }



    public void PositionEnemyOnBoardUnit(BoardUnit bu, Enemy enemyScript)
    {
        enemyScript.transform.position = bu.transform.position;
        if (enemyScript.actionIcon_obj != null)
        {
            
            enemyScript.actionIcon_obj.transform.position = Camera.main.WorldToScreenPoint(enemyScript.transform.Find("ActionIconHolder").position + Vector3.down*0.2f);
        }

        if (enemyScript.hpItem_obj != null)
        {
            enemyScript.hpItem_obj.transform.position = Camera.main.WorldToScreenPoint(enemyScript.transform.Find("HpItemHolder").position + Vector3.down*0.2f);
        }
        //  Align the sorting order
        enemyScript.GetComponent<SpriteRenderer>().sortingOrder = bu.GetComponent<SpriteRenderer>().sortingOrder + 1;
        enemyScript.EnemyActiveBoardUnit = bu;

    }


    public void DeleteOneEnemyFromList(Enemy targetEnemy)
    {
        _enemyList.Remove(targetEnemy);
        if (_enemyList.Count == 0)
        {
            FightManager.Instance.ChangeFightType(FightType.Win);
        }
        
    }

    public IEnumerator DoAllEnemyActionInTurn()
    {
        for (int i = 0; i < _enemyList.Count; i++)
        {
            yield return FightManager.Instance.StartCoroutine(_enemyList[i].EnemyAI_DoAction());
        }
        
        //  Update Action for next round
        for (int i = 0; i < _enemyList.Count; i++)
        {
            _enemyList[i].SetRandomAction();
        }
        
        //  Change to Player's turn
        FightManager.Instance.ChangeFightType(FightType.Player);
    }

}
