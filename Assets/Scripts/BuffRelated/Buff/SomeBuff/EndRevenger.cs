using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRevenger : MonoBehaviour,IEndBuff
{
   public void EndBuff()
    {
        FightManager.Instance.player_AttackValue -= gameObject.GetComponent<Revenger>().increaseAttack;
    }
}
