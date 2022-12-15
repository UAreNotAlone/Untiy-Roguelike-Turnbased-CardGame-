using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  State for loss
public class Fight_Loss : FightUnit
{
    public override void Init()
    {
        Debug.Log("Loss");
        FightManager.Instance.StopAllCoroutines();
        
        //  Show The Loss UI.
    }

    public override void OnFightUpdate()
    {
        
    }
}
