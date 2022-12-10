using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterPlayerTurn : MonoBehaviour
{
    [Header("自动获取的变量")]
    public GameObject EndTurnButton;
    public Player player;
    //public BattleGameMaster gameMaster;
    public CardClass cardAroundMouse;
    public bool isSomeBodyAnimation = false;


    public enum Stage
    {       drawCard,
            NoChooseCard,
            ChosenCard,//选中卡片
            CardAniamtion//把卡打出后
    }
    public Stage stage;

    public BattleGameMaster battleGameMaster;

    public CardClass cardChoosen;
    public List<CardClip> cardClips;

    public int numberOfCardATurn;
    public List<CardClass> playerTotalCard;
    public List<CardClass> playerLeftCard = new List<CardClass>();

    // Start is called before the first frame update
   
    private void Start()//在新的回合反复enable master时重新调用，发卡
    {
        EndTurnButton = GameObject.Find("EndTurnButtonUI");
        battleGameMaster = GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>();
        player = GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>().player;
        cardClips = battleGameMaster.cardClips;
        numberOfCardATurn = battleGameMaster.numberOfCardATurn;
        playerTotalCard = battleGameMaster.playerTotalCard;

}
    public void OnEnable()
    {
        //回复能量
        stage = Stage.drawCard;
        playerLeftCard = new List<CardClass>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stage == Stage.drawCard)
        {
            player.RecoverEnergy();
            Initial();
            stage = Stage.NoChooseCard;//注意由于后续本回合循环设计不会再回到抽卡阶段，所以实际上这段只会执行一次
                                        //实际上我做的地图点检测是每秒执行一次，所以可以考虑破坏地形一说
        }
        if (stage == Stage.NoChooseCard)  //判断玩家是否在某张卡附近，不在已经选择阶段执行并终止判断是否完成动画
        {
            EndTurnButton.SetActive(true);
            if (IsInvoking(nameof(isPerformAnimation)))
            {
                CancelInvoke(nameof(isPerformAnimation));
            }
            cardAroundMouse = IsMouseAroundSomeCard.IsMouseOnSomeCard(playerLeftCard);
        }
        if (stage != Stage.CardAniamtion &&stage != Stage.drawCard)//用否定句式是不是在未来更改时不太安全
        {
            PlayerChoseOrGiveUpCard();//左键某张卡选择，右键回复原装
        }
        
        //Debug.Log(enemies[0].gameObject == null);//中间的打出过程由卡牌具体完成
        
        if(stage == Stage.ChosenCard && cardChoosen.thisCardStage == CardClass.ThisCardStage.SentOut)//打出卡牌后，不选择这张卡牌,并禁用player操作
        {
            Debug.Log("Card is Sent out");
            
            InvokeRepeating(nameof(isPerformAnimation),0.1f,0.5f);//由于卡牌的动画种类很多，调用的方法也不尽相同，因此最好把卡牌带来的动画定义为卡牌效果来细化
            //因此这个方程仅作提醒意义，不在这里实现功能//注意在卡牌动画事件里把GameMasterPlayerTrun 的stage调整
            //我废除前面说的话，因为可以在卡牌和玩家回合控制者之间用player作桥梁，一方面card所需所有的动画都可以player申请调用，
            //另一方面player有一个属性是否完成动画（对于trigger 和bool控制的动画有不同的处理效果）来帮助回合控制者判断是否要进入下一阶段
            //我想写一个战斗UI类，加入所有的UI元素，并把这些UI分类（由public完成），在分类完成后，统一在某个阶段对某类UI进行禁用调用（例如button ）
            //这个0.1fs是为了给player和enemy足够的反应时间来开启动画
            CardIsSentOut();
            

        }
        
        
        if(playerLeftCard.Count == 0)//回合结束条件
        {
            EndTurnButton.SetActive(false);
            BattleGameMaster.IsPlayerTurn = false;
            
        }
        //结束回合按钮

}
    public void CardIsSentOut()
    {
        EndTurnButton.SetActive(false);
        stage = Stage.CardAniamtion;
        playerLeftCard.Remove(cardChoosen);
        UpdateCardPosition(cardChoosen);
        
    }
    public void UpdateCardPosition(CardClass card)
    {
        //去除打出的卡牌
        //移动现有卡牌位置，获取playerLefCard的所有父物体卡槽，按照x坐标重拍，把某个点作为中心，按一定距离确定移动点
    }
    public void PlayerChoseOrGiveUpCard()
    {
        
        //Debug.Log(Enemy.isMouseAroundSomeEnemy);

        if (Input.GetMouseButtonDown(0) && cardAroundMouse != null && stage == Stage.NoChooseCard)//如果点击鼠标左键且如果有鼠标放在某张卡上
        {

            //Debug.Log(CardClass.isExistCardChosen);
            
            //CardClass.isExistCardChosen = true;
            //Debug.Log("we will draw a line from card to mouse");
            cardChoosen = IsMouseAroundSomeCard.IsMouseOnSomeCard(playerLeftCard);
            Debug.Log("CARD is choosen" + cardChoosen.name);

            stage = Stage.ChosenCard;
            cardChoosen.thisCardStage = CardClass.ThisCardStage.Chosen;

        }
        if (Input.GetMouseButtonDown(1) && stage == Stage.ChosenCard)
        {
            stage = Stage.NoChooseCard;
        }
    }

    public void isPerformAnimation()
    {
        Debug.Log("Judging is Animation");
       //判断是否由敌人或玩家正在进行动画
       if(Enemy.AllEnemyFinishAniamtion && player.IsPlayerFinishAnimation)
        {
            if(stage ==Stage.CardAniamtion)//保险起见
            {
                stage = Stage.NoChooseCard;
            }
        }

        
    }
    
    private void Initial()
    {
        stage = Stage.NoChooseCard;
        for (int i = 0; i < numberOfCardATurn; i++)
        {
            int randomInt = Random.Range(0, playerTotalCard.Count);
            //Debug.Log(randomInt);
            if (cardClips[i].GetComponentsInChildren<CardClass>().Length != 0)
            {
                //Debug.Log(cardClips[i].GetComponentsInChildren<CardClass>().Length);
                Destroy(cardClips[i].transform.GetChild(0).gameObject);
            }
            CardClass card = Instantiate(playerTotalCard[randomInt], cardClips[i].transform.position, cardClips[i].transform.rotation);
            card.transform.SetParent(cardClips[i].transform);//在卡槽处生成的卡牌并作为卡槽的子物体；//注意保证发出卡牌的数量少于卡牌数量
            playerLeftCard.Add(card);
            card.holdPlayer = player;
                //Debug.Log(card.holdPlayer+player.name);
            
        }
    }
}