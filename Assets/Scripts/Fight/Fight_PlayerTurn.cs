using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  Player's Turn
public class Fight_PlayerTurn : FightUnit
{
    public override void Init()
    {
        UIManager.Instance.ShowTurnInformation("Player's Turn", Color.yellow, delegate
        {
            //  Mana back up
            FightManager.Instance.player_curMana = FightManager.Instance.player_MAXMana;
            UIManager.Instance.GetUIScript<FightUI>("FightUI").UpdateManaTxt();
            
            //  No Card! - > Re
            if (FightCardManager.Instance.isPlayerHasCard() == false)
            {
                FightCardManager.Instance.Init();
                //
                UIManager.Instance.GetUIScript<FightUI>("FightUI").UpdateUsedCardCountTxt();
            }
            //  Fetch the card
            Debug.Log("Fetch the card here ?");
            //  Get 4 cards every round.
            //  TODO: Set some limitation on the number of card choose every round.
            UIManager.Instance.GetUIScript<FightUI>("FightUI").CreateCardItemUI(5); 
            UIManager.Instance.GetUIScript<FightUI>("FightUI").UpdateCardItemPos();
            
            //  Update the Card Count
            UIManager.Instance.GetUIScript<FightUI>("FightUI").UpdateCardCountTxt();
            
        });
        Debug.Log("Player's turn activated");
        
    }

    public override void OnFightUpdate()
    {
        
    }

}
