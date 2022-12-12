using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_EnemyTurn : FightUnit
{
    public override void Init()
    {
        //  Del all the Card UI on show.
        UIManager.Instance.GetUIScript<FightUI>("FightUI").RemoveAllCards();
        
        //  Show Tips for Enemy's turn.
        UIManager.Instance.ShowTurnInformation("Enemy's Turn", Color.red, delegate
        {
            //  Fetch the card
            Debug.Log("Enemy AI On the RUN...");
            FightManager.Instance.StartCoroutine(EnemyManager.Instance.DoAllEnemyActionInTurn());


        });
        Debug.Log("Player's turn activated");
    }

    public override void OnFightUpdate()
    {
        
    }

}
