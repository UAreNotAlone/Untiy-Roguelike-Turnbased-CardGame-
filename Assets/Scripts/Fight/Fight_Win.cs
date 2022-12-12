using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  State for win
public class Fight_Win : FightUnit
{
    public override void Init()
    {
        //FightManager.Instance.StopAllCoroutines();
        Debug.Log("Suc");
        //  Close Fight UI
        UIManager.Instance.CloseUIByName("FightUI");
        //  Show Reward UI
        UIManager.Instance.LoadUIByName<WinUI>("SelectCardUI");
    }
}
