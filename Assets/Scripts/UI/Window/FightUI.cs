using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


//  This will be loaded in the FightType.FightInit
public class FightUI : UIBase
{
    //  Card Txt
    [SerializeField] private Text _cardCountTxt;  //  抽牌堆
    [SerializeField] private Text _noCardCountTxt;    //  弃牌堆
    
    //  Mana Txt
    [SerializeField] private Text _manaTxt;
    
    //  hp txt and image
    [SerializeField] private Text _hpHeadTxt;
    [SerializeField] private Text _hpBodyTxt;
    [SerializeField] private Text _hpHandTxt ;
    [SerializeField] private Text _hpLegTxt;
    [SerializeField] private Image _hpHeadImg;
    [SerializeField] private Image _hpBodyImg;
    [SerializeField] private Image _hpHandImg;
    [SerializeField] private Image _hpLegImg;
    
    //  Defense Value txt
    [SerializeField] private Text _defenseTxt;

    //  Card Item Collection available on the game scene.
    [SerializeField] private List<CardItem> _cardItemList;

    //  Initial SetUp when loaded into the Scene.
    private void Start()
    {
        UpdateBodyHP();
        UpdateHandHP();
        UpdateHeadHP();
        UpdateLegHP();
        
        UpdateDefenseTxt();
        UpdateManaTxt();
        
        UpdateUsedCardCountTxt();
        UpdateCardCountTxt();
    }

    private void Awake()
    {
        //  Init CardItem list
        _cardItemList = new List<CardItem>();
        //  Bind them with corresponding component
        _cardCountTxt = transform.Find("hasCard/icon/Text").GetComponent<Text>();
        _noCardCountTxt = transform.Find("noCard/icon/Text").GetComponent<Text>();
        //  mana
        _manaTxt = transform.Find("mana/Text").GetComponent<Text>();
        //  hp txt
        _hpHeadTxt = transform.Find("hpSystem/hpHead/moneyTxt").GetComponent<Text>();
        _hpBodyTxt = transform.Find("hpSystem/hpBody/moneyTxt").GetComponent<Text>();
        _hpHandTxt = transform.Find("hpSystem/hpHands/moneyTxt").GetComponent<Text>();
        _hpLegTxt = transform.Find("hpSystem/hpLegs/moneyTxt").GetComponent<Text>();
        //  hp img
        _hpHeadImg = transform.Find("hpSystem/hpHead/fill").GetComponent<Image>();
        _hpBodyImg = transform.Find("hpSystem/hpBody/fill").GetComponent<Image>();
        _hpHandImg = transform.Find("hpSystem/hpHands/fill").GetComponent<Image>();
        _hpLegImg = transform.Find("hpSystem/hpLegs/fill").GetComponent<Image>();
        //  Defense txt
        //  [221208-yuyang]
        //  Temproraily, we choose the Body Defense txt to represent the real defense value
        //  it might be changed to that each component will have its own independent defense value
        //  in the future.
        _defenseTxt = transform.Find("hpSystem/hpBody/fangyu/Text").GetComponent<Text>();
        
        //  TurnBtn
        transform.Find("turnBtn").GetComponent<Button>().onClick.AddListener(OnChangeTurnBtn);
    }

    //  Change turn from player to enemy by Btn click
    private void OnChangeTurnBtn()
    {
        //   Check for current FightType
        if (FightManager.Instance.fightUnit is Fight_PlayerTurn)
        {
            FightManager.Instance.ChangeFightType(FightType.Enemy);
        }
    }

    //  Updating the HP of all the cyber body part
    //  Value fetcehd only from the FightManager
    public void UpdateHeadHP()
    {
        _hpHeadTxt.text = FightManager.Instance.player_curHP_head + "/" + FightManager.Instance.player_MAXHP;
        _hpHeadImg.fillAmount =
            (float)FightManager.Instance.player_curHP_head / (float)FightManager.Instance.player_MAXHP;
    }
    
    public void UpdateHandHP()
    {
        _hpHandTxt.text = FightManager.Instance.player_curHP_hand + "/" + FightManager.Instance.player_MAXHP;
        _hpHandImg.fillAmount =
            (float)FightManager.Instance.player_curHP_hand / (float)FightManager.Instance.player_MAXHP;
    }
    
    public void UpdateBodyHP()
    {
        _hpBodyTxt.text = FightManager.Instance.player_curHP_body + "/" + FightManager.Instance.player_MAXHP;
        _hpBodyImg.fillAmount =
            (float)FightManager.Instance.player_curHP_body / (float)FightManager.Instance.player_MAXHP;
    }
    
    public void UpdateLegHP()
    {
        _hpLegTxt.text = FightManager.Instance.player_curHP_leg + "/" + FightManager.Instance.player_MAXHP;
        _hpLegImg.fillAmount =
            (float)FightManager.Instance.player_curHP_leg / (float)FightManager.Instance.player_MAXHP;
    }

    public void UpdateAllHP()
    {
        UpdateBodyHP();
        UpdateHandHP();
        UpdateHeadHP();
        UpdateLegHP();
    }

    //  Mana Value txt Updated
    public void UpdateManaTxt()
    {
        _manaTxt.text = FightManager.Instance.player_curMana + "/" + FightManager.Instance.player_MAXMana;
    }
    
    //  Defense Value txt Updated.
    public void UpdateDefenseTxt()
    {
        _defenseTxt.text = FightManager.Instance.player_defenseValue.ToString();
    }

    public void UpdateCardCountTxt()
    {
        _cardCountTxt.text = FightCardManager.Instance.currentCardList.Count.ToString();
    }

    public void UpdateUsedCardCountTxt()
    {
        _noCardCountTxt.text = FightCardManager.Instance.usedCardList.Count.ToString();
    }

    //  Init the CardItem with UI and Attach corresponding scripts onto it.
    public void CreateCardItemUI(int cnt)
    {
        if (cnt > FightCardManager.Instance.currentCardList.Count)
        {
            Debug.Log("[FightUI] The card item number is overflow, all the card is about to be drawn");
            cnt = FightCardManager.Instance.currentCardList.Count;
        }

        for (int i = 0; i < cnt; i++)
        {
            //  Instantiate an obj
            GameObject cardItem_obj = Instantiate(Resources.Load("UI/CardItem"), transform) as GameObject;
            cardItem_obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1000, -700);
            
            //  Assign this card with real card info(card id)
            string cardItemID = FightCardManager.Instance.PlayerDrawOneCard();
                        
            //  Fetch the data from GameConfig
            Dictionary<string, string> cardItemData = GameConfigManager.Instance.GetCardInfoById(cardItemID);
            
            
            //var cardItem_script = cardItem_obj.AddComponent<CardItem>();
            CardItem cardItem_script = cardItem_obj.AddComponent(System.Type.GetType(cardItemData["Script"])) as CardItem;
            


            cardItem_script.Init(cardItemData);
            _cardItemList.Add(cardItem_script);
        }
        
    }
    
    
    //  Update the Card Item's pos
    public void UpdateCardItemPos()
    {
        float offset = 1200.0f / _cardItemList.Count;
        Vector2 startPos = new Vector2(-_cardItemList.Count / 2.0f * offset + offset * 0.5f, -628);
        for (int i = 0; i < _cardItemList.Count; i++)
        {
            _cardItemList[i].GetComponent<RectTransform>().DOAnchorPos(startPos, 0.5f);
            startPos.x = startPos.x + offset;
            
        }
    }
    
    
    //  Delete one card by item
    public void RemoveOneCardItem(CardItem targetItem)
    {  
        AudioManager.Instance.PlayEffectByName("Cards/cardShove");
        //  Turn off the script
        targetItem.enabled = false;
        //  Add it to the used card list.
        FightCardManager.Instance.usedCardList.Add(targetItem.cardData["Id"]);
        //  Update related UI
        _noCardCountTxt.text = FightCardManager.Instance.usedCardList.Count.ToString();
        //  Remove from list
        _cardItemList.Remove(targetItem);
        UpdateCardItemPos();
        
        //  Move the card to the used card list.
        targetItem.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1000, -628), 0.25f);
        targetItem.transform.DOScale(0, 0.25f);
        Destroy(targetItem.gameObject, 1);
    }

    public void RemoveAllCards()
    {
        for (int i = _cardItemList.Count - 1; i >= 0; i--)
        {
            RemoveOneCardItem(_cardItemList[i]);
        }
    }
    
}
