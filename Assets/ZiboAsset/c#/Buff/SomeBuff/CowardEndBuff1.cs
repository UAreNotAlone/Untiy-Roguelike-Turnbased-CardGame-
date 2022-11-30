using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowardEndBuff1 :MonoBehaviour,IEndBuff
{
    public void EndBuff()
    {
        gameObject.GetComponent<IBuff>().character.setAttackForce(gameObject.GetComponent<IBuff>().character.attackForce + 9);
    }
}
