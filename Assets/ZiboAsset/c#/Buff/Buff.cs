using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    
    //造成实际的buff（在开始回合结束回合之间更新），并提供更新buff图标位置的方法
    [Header("自动获取变量")]
    //当卡牌使用这个buff时需要实例化一个buff图标，并让给这个buff一些参数
    
    public bool beginAffect = false;
    private BattleGameMaster battleGameMaster;
    private int oldTurn = 0;

    public float speed = 20;
    private bool startMove = false;
    private Vector3 movePosition;
    private Transform buffTransform;
    [Header("可以手动设置的变量")]
    public int startBuffTurn = 0;//
    public int endBuffTurn = 1;
    public int buffExecutedTurns = 1;
    // Start is called before the first frame update

    public void UseBuffEffectDefault(Character cha)//提供一个调用接口,使用默认参数启动buff
    {
        
        SetActiveParameter(cha,0,1, 1);
    }
    public void SetActiveParameter(Character cha, int p_startBuffAfter,int p_executedTurns,int p_endBuffAfter)//在statBuffAfter个回合后开始启动buff，执行buff函数int p_executedTurns个回合
                                                                                         //，在endBuffTurnAfter个回合后执行buff消失函数并摧毁图标
    {                                                                                     //例如输入在第一回合使用0，1，2参数执行代表在第一回合立即执行，只执行一回合，
                                                                                          //并在一回合后（第三回合）结束buff（注意先更新回合再执行函数）
        gameObject.GetComponent<IBuff>().character = cha;
        startBuffTurn = battleGameMaster.currentTurn + p_startBuffAfter;//执行buff包含该回合
        endBuffTurn = battleGameMaster.currentTurn + p_endBuffAfter;//注意此含义是在该回合不执行该buff并摧毁实例化的buff图标
        buffExecutedTurns = p_executedTurns;//备用
        beginAffect = true;
    }
    public void GotoGradually(Vector3 position)
    {
        movePosition = position;
        startMove = true;
    }
    void Awake()
    {
        buffTransform = GameObject.Find("BuffTransform").transform;
        battleGameMaster =  GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (oldTurn != battleGameMaster.currentTurn && beginAffect)// 只需要在该turn执行一次,也就是如果下一帧还是这个turn的话就不执行，不是的话更新oldturn并执行
        {
            oldTurn = battleGameMaster.currentTurn;
            if (battleGameMaster.currentTurn <= startBuffTurn + buffExecutedTurns-1 && battleGameMaster.currentTurn >= startBuffTurn)//处于buffTurn
            {
                //把这个和Card都合成为一个prefabs,后者只在battleGameMaster初始化时合成物体（prefabs,add 4 script和接口）最好把实例化这个过程封装到某个脚本中,后者只在move card调用
                gameObject.GetComponent<IBuff>().BuffEffect();
            }
            else if (battleGameMaster.currentTurn == endBuffTurn)
            {
                gameObject.GetComponent<IEndBuff>().EndBuff();
                buffTransform.GetComponent<buffIconManager>().RemoveBuff(transform);
            }
        }

        if (startMove)
        {
            float step = Time.deltaTime * speed;
            transform.position =   Vector2.MoveTowards(transform.position, movePosition, step);
            if(Vector2.Distance(transform.position,movePosition)< 0.01f)
            {
                Debug.Log("buff reach position");
                startMove = false;
            }
        }
    }
}
