using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseDefenseEndBuff :MonoBehaviour, IEndBuff
{
    public void EndBuff()
    {
        Character cha = gameObject.GetComponent<IncreaseDefenseBuff>().character;

        cha.setDefenceAbility(cha.defenceAbility - GetComponent<IncreaseDefenseBuff>().increaseDefense);
    }
}
