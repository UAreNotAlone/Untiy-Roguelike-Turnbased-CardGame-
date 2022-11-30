using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsMouseAroundSomeEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public static BattleGameMaster gameMaster;
    public void Start()
    {
        gameMaster = GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>();


    }
    public static Enemy IsMouseAroundEnemy()
    {
        List<Enemy> enemies = gameMaster.enemies;
        for (int i = 0; i < enemies.Count; i++)
        {

            if (IsMouseOnThisEnemy(enemies[i]))
            {

                enemies[i].enemyStage = Enemy.EnemyStage.MouseAround;

                //Debug.Log(Enemy.isMouseAroundSomeEnemy);
                return enemies[i];
            }
            else
            {
                enemies[i].enemyStage = Enemy.EnemyStage.notMouseAround;
            }

        }
        
        
        return null;
    }
    public static bool IsMouseOnThisEnemy(Enemy enemy)
    {
        Vector3 mousePos = Input.mousePosition;//注意此步骤获取的值是屏幕坐标
        Vector3 RealMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        //Debug.Log(RealMousePos);//这里的坐标z统统为-10！()
        RealMousePos = new Vector3(RealMousePos.x, RealMousePos.y, enemy.transform.position.z);

        //Debug.Log(enemy.transform.position);
        //Debug.Log(Vector3.Distance(enemy.transform.position, RealMousePos));
        if (-enemy.transform.position.y + RealMousePos.y < enemy.enemySize.y &&
            -enemy.transform.position.y + RealMousePos.y > 0  &&
            Mathf.Abs(enemy.transform.position.x - RealMousePos.x) <enemy.enemySize.x*0.5)
        {
            Debug.Log("MouseOnThisEnemy");
            return true;
        }
        else
        {
            return false;
        }
    }


}
