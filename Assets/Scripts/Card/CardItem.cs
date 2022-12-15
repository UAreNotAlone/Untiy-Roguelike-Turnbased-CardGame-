using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class CardItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //  
    public Dictionary<string, string> cardData;
    private int _cardIndexInSibling;
    //  Drag Var
    private Vector2 _cardInitPosForDrag;
    //  Been called by the method in FightUI by Fight_PlayerTurn
    public void Init(Dictionary<string, string> data)
    {
        //  TODO: Destroy if there is one cardData.
        this.cardData = data;
    }

    private void Start()
    {
        //  Id Name Script Type Des BgIcon Icon Expend Arg0 Effects
        
        //  Load the card base background pic.  
        transform.Find("bg").GetComponent<Image>().sprite = Resources.Load<Sprite>(cardData["BgIcon"]);
        
        //  Load the picture of this card.
        //transform.Find("bg/icon").GetComponent<Image>().sprite = Resources.Load<Sprite>(cardData["Icon"]);
        
        //  Load the Decsription msg of this card.
        transform.Find("bg/msgTxt").GetComponent<Text>().text = string.Format(cardData["Des"], cardData["Arg0"]);
        //  Load the name of the card.
        transform.Find("bg/nameTxt").GetComponent<Text>().text = cardData["Name"];
        //  Load the mana cost of the card.
        transform.Find("bg/useTxt").GetComponent<Text>().text = cardData["Expend"];
        //  Load the card type.
        transform.Find("bg/Text").GetComponent<Text>().text =
            GameConfigManager.Instance.GetCardTypeInfoById(cardData["Type"])["Name"];
        
        //  Set the material for the bg
        transform.Find("bg").GetComponent<Image>().material =
            Instantiate(Resources.Load<Material>("Materials/outline"));

    }

    //  Required Member Func
    //  Mouse in
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.5f, 0.25f);
        _cardIndexInSibling = transform.GetSiblingIndex();
        transform.SetAsLastSibling();
        
        transform.Find("bg/HighLight").gameObject.SetActive(true);
        transform.Find("bg").GetComponent<Image>().material.SetColor("_lineColor", Color.yellow);
        transform.Find("bg").GetComponent<Image>().material.SetFloat("_lineWidth", 10.0f);

    }
    //  Mouse out
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1.4f, 0.25f);
        transform.SetSiblingIndex(_cardIndexInSibling);
                
        transform.Find("bg/HighLight").gameObject.SetActive(false);
        transform.Find("bg").GetComponent<Image>().material.SetColor("_lineColor", Color.black);
        transform.Find("bg").GetComponent<Image>().material.SetFloat("_lineWidth", 1.0f);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _cardInitPosForDrag = transform.GetComponent<RectTransform>().anchoredPosition;
        AudioManager.Instance.PlayEffectByName("Cards/draw");
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out pos))
        {
            transform.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(transform.GetComponent<RectTransform>().anchoredPosition, pos, 1.2f);


        }

    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.GetComponent<RectTransform>().anchoredPosition = _cardInitPosForDrag;
        transform.SetSiblingIndex(_cardIndexInSibling);
    }
    
    //  Tries to use some card.
    public virtual bool TryUseCard_Mana()
    {
        //  Mana required by this card.
        int manaRequired = int.Parse(cardData["Expend"]);

        if (manaRequired > FightManager.Instance.player_curMana)
        {
            //  Not enough mana for this card.
            AudioManager.Instance.PlayEffectByName("Cards/useFailed");
            
            //  Show Tips
            UIManager.Instance.ShowTurnInformation("Not Enough Mana", Color.red);

            return false;
        }
        else
        {
            //  Update the mana for player
            FightManager.Instance.player_curMana -= manaRequired;
            UIManager.Instance.GetUIScript<FightUI>("FightUI").UpdateManaTxt();
            
            //  Del the used card.
            UIManager.Instance.GetUIScript<FightUI>("FightUI").RemoveOneCardItem(this);

            return true;
        }
    }

    public virtual bool TryUseCard_Range(Enemy tar)
    {
        //  Get From 
        int range = int.Parse(cardData["Range"]);
        //  Mana required by this card.
        int manaRequired = int.Parse(cardData["Expend"]);
        

        
        //  Get distance
        List<BoardUnit> path =
            MapManager.Instance.FindPath(Player.Instance.PlayerActiveBoardUnit, tar.EnemyActiveBoardUnit);
        if (path.Count > range)
        {
            //  Not enough mana for this card.
            AudioManager.Instance.PlayEffectByName("Cards/useFailed");
            
            //  Show Tips
            UIManager.Instance.ShowTurnInformation("Not In Range", Color.red);

            return false;
        }
        else
        {
            //  Update the mana for player
            //FightManager.Instance.player_curMana -= manaRequired;
            return true;
        }

        

    }
    
    
    //  Play card Effect after use
    public void PlayCardEffect(Vector3 pos)
    {
        GameObject  effect_obj = Instantiate(Resources.Load(cardData["Effects"])) as GameObject;
        effect_obj.transform.position = pos;
        Destroy(effect_obj, 2);
    }
    
}
