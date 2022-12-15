using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revenger: MonoBehaviour, IBuff
{
    //使用buff时，先实例化buff，再传入setCharacter,最后设置buff参数
    [Header("手动设置参数")]
    public int increaseAttack = 2;
    private bool executed = false;                      // Start is called before the first frame update

    public void BuffEffect()
    {
        if (!executed) 
        { 
            FightManager.Instance.player_AttackValue += increaseAttack;
            executed = true;
        }
        Debug.Log("player attack increased to" + FightManager.Instance.player_AttackValue);
    }
}
