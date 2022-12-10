using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsMouseAroundSomeCard:MonoBehaviour
{
    
    public void Enable()
    {
        
    }
    public static CardClass IsMouseOnSomeCard(List<CardClass> cardList)//检查是否鼠标放在某个卡牌上面，如果是返回该卡牌的对象并执行cardClass的MousePutOn方法，并将其他卡牌复原
    {
        
        
        
       
        for (int i = 0; i < cardList.Count; i++)
        {

            if (IsMouseOnThisCard(cardList[i]))
            {
                CardClass card = cardList[i];
                if (card != null)
                {

                    //Debug.Log(i);
                    card.thisCardStage = CardClass.ThisCardStage.cardAroundMouse;
                    return card;
                }


                //Debug.Log(CardClass.isExitMousePutOnSomeCard);

            }
            else
            {
                CardClass card = cardList[i];
                card.thisCardStage = CardClass.ThisCardStage.cardNotAroundMouse;
                card.notMousePutOnThisCard();
            }

        }
        
        //Debug.Log(CardClass.isExitMousePutOnSomeCard);
        return null;

    }
    public static bool IsMouseOnThisCard(CardClass card)
    {
        if (card == null) return false;
        Vector3 mousePos = Input.mousePosition;//注意此步骤获取的值是屏幕坐标
        Vector3 RealMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        //Debug.Log(RealMousePos);//这里的坐标z统统为-10！()
        RealMousePos = new Vector3(RealMousePos.x, RealMousePos.y, card.transform.position.z);

        if (Mathf.Abs(card.transform.position.y - RealMousePos.y) < card.CardSize.y*0.5 &&
            Mathf.Abs(card.transform.position.x - RealMousePos.x) < card.CardSize.x*0.5)
        {

            return true;
        }
        else
        {
            return false;
        }
    }
}
