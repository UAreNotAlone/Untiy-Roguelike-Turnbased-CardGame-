using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawCardItem : CardItem
{
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (TryUseCard_Mana() == true)
        {
            //  TODO: Use player's body Hp to consume use this card.
            int effect_val = int.Parse(cardData["Arg0"]);
            //  Check whether there still cards to draw from.
            if (FightCardManager.Instance.isPlayerHasCard() == true)
            {
                //  Fetch one card.
                UIManager.Instance.GetUIScript<FightUI>("FightUI").CreateCardItemUI(effect_val);
                UIManager.Instance.GetUIScript<FightUI>("FightUI").UpdateCardItemPos();
                Vector3 pos =
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2.5f));
                PlayCardEffect(pos);
            }
            else
            {
                base.OnEndDrag(eventData);
            }

        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
