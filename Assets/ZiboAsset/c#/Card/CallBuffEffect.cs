using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  CallBuffEffect:MonoBehaviour, IEffect//rangeAttack等调用，在buffTransform实例化buff icon,
                                   //剩下的交给buff自己来做,
                                   //仔细想一想，实际上buff的排版可以由buffIconManager来完成，我们只用在buff实例化之后把buff物体交给manager的list
{                                      //通过添加不同的buff预制件来实现不同的buff
                                        //可以控制buff持续回合，从何时开始

    
    public BattleGameMaster battleGameMaster;
    public Transform buffTransform;
    [Header("需要手动设置")]
    public Buff buffPrefab;
    public int buffStartAfter = 1;
    public int buffEndAfter = 2;//
    public int buffLastfor = 1;//可以控制buff回合，也可以让=使用buff默认参数buff完成
    // Start is called before the first frame update
    
    void Start()
    {
        battleGameMaster = GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>();
        buffTransform = GameObject.Find("BuffTransform").transform;
    }

    public void CauseEffect(List<Transform> t)
    {
        Buff buff = Instantiate(buffPrefab, buffTransform.position, buffTransform.rotation);//实例化buff
                                                                                                //Debug.Log(buff.gameObject.GetComponent<IBuff>());
        buff.SetActiveParameter(battleGameMaster.player,buffStartAfter, buffLastfor, buffEndAfter);//启动buff实际效果
        buffTransform.GetComponent<buffIconManager>().AddBuff(buff.transform);//更新buff图标
    }
    
}
