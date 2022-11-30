using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeCard : MonoBehaviour//制作这张卡牌时只用两个脚本cardClass and this
{
    //点击这张卡片后，如果在左键选择其他卡牌，则会丢弃左键卡牌（摧毁并从playerturn卡组中移除），并让系统回到noChooseCard状态
    //因为这张卡牌必然打出，所以不用考虑能量;如果要加入能量，则必须在这里操作。
    public CardClass thisCard;
    public GameMasterPlayerTurn playerTurnMaster;
    public BattleGameMaster battleGameMaster;
    public List<CardClass> cardListExceptMe;
    private void Start()
    {
        
        battleGameMaster = GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>();//为了方便以后修改，所有脚本的player最好直接从一个地方获取
        thisCard = gameObject.GetComponent<CardClass>();
        playerTurnMaster = GameObject.Find("PlayerTurn").GetComponent<GameMasterPlayerTurn>();
        foreach (CardClass card in playerTurnMaster.playerLeftCard)
        {
            if (card != thisCard)
            {
                cardListExceptMe.Add(card);
            }
        }
    }
    private void Update()
    {
        
        if (thisCard.thisCardStage == CardClass.ThisCardStage.Chosen)
        {
            cardListExceptMe = new List<CardClass>();
            foreach (CardClass card in playerTurnMaster.playerLeftCard)
            {
                if (card != thisCard)
                {
                    cardListExceptMe.Add(card);
                }
            }
            //copy playerLeftcard
            CardClass cardAroundMouse = IsMouseAroundSomeCard.IsMouseOnSomeCard(cardListExceptMe);//这个函数在调用时会改变card的状态,由于我们对这个函数在player turn的使用做了严格限制
                                                                                  //，只在nochoose 阶段使用，所以改变状态不会有影响，
                                                                                  //但是在这里，我们只想改变除了这张卡以外卡牌的状态
            //Debug.Log(cardAroundMouse.name);
            if(Input.GetMouseButtonDown(0) && cardAroundMouse != null)//丢弃卡牌操作,排除这张卡牌
            {
                //先保存此时的mouse card
                Debug.Log("choose abandoned card");
                CardClass cardTobeAbandon = cardAroundMouse;
                playerTurnMaster.playerLeftCard.Remove(cardTobeAbandon);
                playerTurnMaster.playerLeftCard.Remove(thisCard);//保险起见
                thisCard.thisCardStage = CardClass.ThisCardStage.SentOut;//这会自动把这张trader卡牌移除卡组，且没有使用UseCauseEffect;//并让系统回到noChoose状态
                battleGameMaster.player.currentEnergy += 1;
                Destroy(cardTobeAbandon.gameObject);
                Destroy(thisCard.gameObject);//注意摧毁物体而非脚本

            }
        }
        //当点击右键时由于卡牌状态会（在playerTurn系统）自动转化为nochoose，
    }
}
