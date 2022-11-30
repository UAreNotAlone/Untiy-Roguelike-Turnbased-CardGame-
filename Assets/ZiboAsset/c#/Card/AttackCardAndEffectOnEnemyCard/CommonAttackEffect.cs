using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonAttackEffect : MonoBehaviour,IEffect
{
    public Player player;
    [Header("手设变量")]
    public float damage = 10;
    public void Start()
    {
        player = GetComponent<CardClass>().holdPlayer;
    }
    // Start is called before the first frame update
    public void CauseEffect(List<Transform> targetList)
    {
        Debug.Log("EnemyisHurt");
        foreach (Transform enemy in targetList)//解决了有多个攻击目标的问题
        {
            Enemy ene = enemy.GetComponent<Enemy>();
            if (ene != null)
            {
                player.Attack(ene, damage);
            }
            else
            {
                Debug.Log(enemy.name + "do not have enemy component");
            }
        }
    }
}
