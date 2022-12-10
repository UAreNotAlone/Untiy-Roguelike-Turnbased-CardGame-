using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClass: MonoBehaviour
{
    //整个卡组中是否有卡牌已经被选中

    [Header("自动获取的变量")]
    public bool isExtended = false;

    public Player holdPlayer;
    
    public enum ThisCardStage
    {
        cardNotAroundMouse,
        cardAroundMouse,
        Chosen,
        SentOut
    }
    public ThisCardStage thisCardStage = ThisCardStage.cardNotAroundMouse;

    //位置
    public Transform positionTransform;
    //在一场战斗中放入卡槽的位置
    //所绑定的效果对象
    //是否在玩家整个卡组中
    public bool isInPlayerCardList = false;
    //是否在这一回合被抽中
    public bool isGettonThisTurn = false;
    public GameObject cardPrefab;
    public float Speed = 40;
    [Header("需要手动设置的变量")]
    public int CostEnergy = 1;
    public Vector2 CardSize = new Vector2(16,20);
    //鼠标是否放在这张卡上
    public string cardName;
    
    #region function
    public void MousePutOnThisCard()
    {
        float step = Speed * Time.deltaTime;
        //Debug.Log("MousePutOnThisCard");
        //当被鼠标放到时，会不再高亮或抬高
        if (!isExtended)
        {
            CardSize.y = CardSize.y * 2;
            isExtended = true;
        }

        transform.position = Vector2.MoveTowards(transform.position, 
            transform.parent.transform.position + new Vector3(0,0.25f*CardSize.y, 0),step);

    }
    public void notMousePutOnThisCard()
    {
        float step = Speed * Time.deltaTime;
        //Debug.Log("noMouseOnCard"+cardName);
        //没有鼠标放上时回复原状
        if (isExtended)
        {
            CardSize.y = CardSize.y * 0.5f;
            isExtended = false;
        }
        transform.position = Vector2.MoveTowards(transform.position,
           transform.parent.transform.position, step);
       
    }
    public void OnCardChoosen()
    {
        //Debug.Log(cardName + "is chosen");
        //如何让物体边缘高亮
       
    }
    public void UseCauseEffect(List<Transform> targetList)
    {
        
        //消耗能量
        //然后才能产生特殊效果
        if (holdPlayer.TryConsumeEnergy(CostEnergy) == true)//实际上我们可以定义不同的效果接口，如move，attack,
                                                            //再在这个类里面实现不同的群体调用来让参数更清楚，
                                                            //这种情况下能量判断是否应该交给其他脚本来做？这样能量判断要做很多次
        {
            thisCardStage = ThisCardStage.SentOut;//
            IEffect[] effects = gameObject.GetComponents<IEffect>();
            foreach (IEffect effect in effects) effect.CauseEffect(targetList);
            Destroy(gameObject);
        }
        else
        {
            thisCardStage = ThisCardStage.cardNotAroundMouse;
        }
        
        //实现了脚本命名，函数内容，和脚本数量的任意性；
        //Debug.Log("card is sent out");
    }
    #endregion
    public void Start()
    {
        
        cardPrefab = gameObject;
        positionTransform = this.gameObject.transform;
        
    }
    public void Update()
    {
        if(thisCardStage == ThisCardStage.Chosen)
        {
            OnCardChoosen();
        }
        if(thisCardStage == ThisCardStage.cardAroundMouse)
        {
            MousePutOnThisCard();
        }
        else if(thisCardStage == ThisCardStage.cardNotAroundMouse)
        {
            notMousePutOnThisCard();
        }
       
    }

}
