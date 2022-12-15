using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muslim : MonoBehaviour,IBuff
{

    public int damage = 4;
    // Start is called before the first frame update

    public void BuffEffect()
    {
        List<Enemy> enemies = new List<Enemy>(EnemyManager.Instance._enemyList);
        foreach(Enemy enemy in enemies)
        {
            //造成真实伤害？还是让player去攻击？（反伤）真实伤害好像不会被反掉，
            enemy.OnDamaged(damage);

        }
    }
}
