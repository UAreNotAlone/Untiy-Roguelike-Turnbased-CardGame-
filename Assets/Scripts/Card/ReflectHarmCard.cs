using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReflectHarmCard : CardItem
{
    public buffIconManager buffIconManager;
    public void Awake()
    {
        buffIconManager = GameObject.Find("BuffTransform").transform.GetComponent<buffIconManager>();
    }
    //  Use the card
    public override void OnEndDrag(PointerEventData eventData)
    {
        //  Detect whether the mouse is one the player's position.

        if (TryUseCard_Mana() == true)
        {
            int effect_val = int.Parse(cardData["Arg0"]);
            //找到resource里面的反伤buff并实例化
            Debug.Log("reflectharmcard is used");
            
            Buff buffPrefab = Resources.Load<Buff>("Buff/reflectHarmBuff");
            Buff buffInstance = Instantiate(buffPrefab, buffIconManager.transform.position, buffIconManager.transform.rotation);//实例化buff
                                                                                                                             //Debug.Log(buff.gameObject.GetComponent<IBuff>());
            buffInstance.SetActiveParameter(buffPrefab.startBuffTurn, buffPrefab.buffExecutedTurns, buffPrefab.endBuffTurn);//启动buff实际效果
            buffIconManager.AddBuff(buffInstance.transform);//更新图标
        }
        else
        {
            base.OnEndDrag(eventData);
        }


    }
}