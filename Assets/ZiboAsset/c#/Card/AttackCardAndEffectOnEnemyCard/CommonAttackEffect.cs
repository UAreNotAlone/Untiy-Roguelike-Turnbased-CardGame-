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
        foreach (Transform target in targetList)//解决了有多个攻击目标的问题
        {
            Enemy ene = target.GetComponent<Enemy>();
            if(ene == null)
            {
                Cyberborg cy = target.GetComponent<Cyberborg>();
                player.Attack(cy.holdCharacter.GetComponent<Enemy>(), damage, cy,null);
            }
            if (ene != null)
            {
                player.Attack(ene, damage,null,null);
            }
            else
            {
                Debug.Log(target.name + "do not have enemy component");
            }
        }
    }
}
