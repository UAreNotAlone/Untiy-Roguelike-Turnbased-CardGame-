using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefendCardItem : CardItem
{

    //  Use the card
    public override void OnEndDrag(PointerEventData eventData)
    {
        //  Detect whether the mouse is one the player's position.
        
        if (TryUseCard_Mana() == true)
        {
            //  The Card Used Effect takes place here.
            int effect_val = int.Parse(cardData["Arg0"]);
            //  Audio effect.
            AudioManager.Instance.PlayEffectByName("Effect/healspell");
            //  Enhance the Shield.
            FightManager.Instance.player_defenseValue += effect_val;
            UIManager.Instance.GetUIScript<FightUI>("FightUI").UpdateDefenseTxt();
            //  Effect
            Vector3 pos = Camera.main.transform.position;
            pos.y = 0;
            PlayCardEffect(pos);

        }
        else
        {
            base.OnEndDrag(eventData);
        }
        
        
    }

}
