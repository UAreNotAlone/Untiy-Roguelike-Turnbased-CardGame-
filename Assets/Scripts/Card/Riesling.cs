using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Riesling : CardItem
{
    public buffIconManager buffIconManager;
    public void Awake()
    {
        buffIconManager = GameObject.Find("BuffTransform").transform.GetComponent<buffIconManager>();
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (TryUseCard_Mana() == true)
        {
            
            //获取所有手部debufff并随机删除一个
            Debug.Log("Riesling is used");
            
            List<Buff> debuffs = new List<Buff>();
            foreach (Transform buff in buffIconManager.buffs)
            {
                if (buff.GetComponent<Buff>().isDebuff == true && buff.GetComponent<Buff>().buffType == Buff.BuffType.armsBuff)
                {
                    debuffs.Add(buff.GetComponent<Buff>());
                }
            }
            if (debuffs.Count != 0)//当有手部debuff可以删除时进行这样一个删除的工作
            {
                buffIconManager.RemoveBuff(debuffs[Random.Range(0, debuffs.Count)].transform);
            }
            //随机添加一个buff
            Buff addBuff = buffIconManager.totalBuff[Random.Range(0, buffIconManager.totalBuff.Count)];
            Buff buffInstance = Instantiate(addBuff, buffIconManager.transform.position, buffIconManager.transform.rotation);//实例化buff
                                                                                                                             //Debug.Log(buff.gameObject.GetComponent<IBuff>());
            buffInstance.SetActiveParameter(addBuff.startBuffTurn, addBuff.buffExecutedTurns, addBuff.endBuffTurn);//启动buff实际效果
            buffIconManager.AddBuff(buffInstance.transform);//更新图标


        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}

