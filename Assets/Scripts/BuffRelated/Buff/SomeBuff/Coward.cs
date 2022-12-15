using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coward : MonoBehaviour,IBuff
{


    public int decreaedAttackValue = 3;
    public bool executed = false;

    public void BuffEffect()
    {
        //
        if (!executed)
        {
            FightManager.Instance.player_AttackValue -= 3;
            executed = true;
        }
        Debug.Log("decreased attack" + 3);
    }
}
