using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI1 : MonoBehaviour, IEnemyAI
{
    public Enemy enemy;
    public BattleGameMaster battleGameMaster;
    public List<OverlayTile> _path;
    public int maxLength = 1;
    public bool ToMove = false;
    public void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        battleGameMaster = GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>();
    }
    // Start is called before the first frame update

    //只有一格攻击范围和一格移动范围的敌人
    public void AttackAction()
    {
        if (IsAbleAttack(battleGameMaster.player.onMapPoint, gameObject.GetComponent<Enemy>()))
        {
            enemy.Attack(battleGameMaster.player, enemy.attackForce * 0.1f,null,null);
            Debug.Log("enemyAttack");
        }
        else
        {
            Debug.Log("Enemy attempt to move");
            MoveOneStep(enemy, battleGameMaster.player);
        }

    }
    public bool IsAbleAttack(MapPoint mapPoint, Enemy enemy)
    {
        AttackRangeFour _attackRangeFour = new AttackRangeFour();
        if (_attackRangeFour.isInAttackRange(battleGameMaster.player.onMapPoint, enemy))
        {
            return true;
        }
        return false;
    }
    public void MoveOneStep(Enemy enemy, Player player)
    {
        PathFinder _pathFinder = new PathFinder();
        _path = _pathFinder.FindPath(enemy.onMapPoint.GetComponent<OverlayTile>(),
            player.onMapPoint.GetComponent<OverlayTile>());
        while (_path.Count > maxLength){_path.Remove(_path[_path.Count - 1]);}//包含自身
        Debug.Log(_path.Count + "Enemy want to move steps");
        enemy.BeginToMove();//开启移动动画
        enemy.onMapPoint = _path[_path.Count - 1].GetComponent<MapPoint>();
        ToMove = true;
        Debug.Log(_path[0].transform.position);
        Debug.Log(enemy.transform.position);
    }
    public void Update()
    {
        if(ToMove)
        {
            float step = 10 * Time.deltaTime;
            //Debug.Log(player.transform.position);
            //Debug.Log(Vector2.MoveTowards(player.transform.position, _path[0].transform.position, step));
            Vector2 tar =
                Vector2.MoveTowards(enemy.transform.position, _path[0].transform.position, step);
            enemy.transform.position = new Vector3(tar.x, tar.y, _path[0].transform.position.z);
            //用二维向量赋值位置失败
            //Debug.Log(step);
            //Debug.Log(tar.x);
            //Debug.Log(Vector2.MoveTowards(player.transform.position, _path[0].transform.position, step));
            //Debug.Log(player.transform.position);
            //Debug.Log(_path[0].transform.position);
            //Debug.Log(player.name);

            if (Vector2.Distance(enemy.transform.position, _path[0].transform.position) < 0.01f)
            {
                Debug.Log("reach pone point");
                enemy.GetComponent<SpriteRenderer>().sortingOrder = _path[0].GetComponent<SpriteRenderer>().sortingOrder + 1;
                _path.RemoveAt(0);
            }
            if (ReachDestination())
            {
                Debug.Log("enemy reach destination");
                Debug.Log("reach destination");
                enemy.EndMove();
                
                ToMove = false;
            }
       
        }
    }
    private bool ReachDestination()
    {
        if (_path.Count == 0) return true;
        if (_path.Count == 1 && Vector2.Distance(enemy.transform.position, _path[0].transform.position) < 0.001f)
        {
            return true;
        }
        return false;
    }

}

    
   