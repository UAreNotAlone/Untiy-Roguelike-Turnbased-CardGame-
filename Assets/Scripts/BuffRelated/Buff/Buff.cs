using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Buff : MonoBehaviour
{

    //造成实际的buff（在开始回合结束回合之间更新），并提供更新buff图标位置的方法
    [Header("自动获取变量")]
    //当卡牌使用这个buff时需要实例化一个buff图标，并让给这个buff一些参数


    public bool beginAffect = false;

    private int oldTurn = 0;

    
    private bool startMove = false;
    private Vector3 movePosition;
    public Transform buffTransform;
    public GameObject textPrefab;
    public GameObject shownText;
    public Transform canvas;
    [Header("手动设置变量")]
    public float speed = 100;
    public string description;
    public enum BuffType
    {
        headBuff,
        bodyBuff,
        legsBuff,
        armsBuff
    }
    [Header("可以手动设置的变量")]
    public int startBuffTurn = 0;//
    public int endBuffTurn = 1;
    public int buffExecutedTurns = 1;

    public bool isDebuff = false;
    public BuffType buffType;

    // Start is called before the first frame update

   
    public void SetActiveParameter(int p_startBuffAfter,int p_executedTurns,int p_endBuffAfter)//在statBuffAfter个回合后开始启动buff，执行buff函数int p_executedTurns个回合
                                                                                         //，在endBuffTurnAfter个回合后执行buff消失函数并摧毁图标
    {                                                                                     //例如输入在第一回合使用0，1，2参数执行代表在第一回合立即执行，只执行一回合，
                                                                                          //并在一回合后（第三回合）结束buff（注意先更新回合再执行函数）
        
        startBuffTurn =FightManager.Instance.currentTurn + p_startBuffAfter;//执行buff包含该回合
        endBuffTurn = FightManager.Instance.currentTurn + p_endBuffAfter;//注意此含义是在该回合不执行该buff并摧毁实例化的buff图标
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
        textPrefab =(GameObject) Resources.Load("Prefabs/textPrefab");
        canvas = GameObject.Find("Canvas").transform;

        
    }

    public bool DistanceCriterion(Vector3 v1, Vector3 v2)//判断鼠标是否在buff图标上面的标准
    {
        if(Mathf.Abs(v1.y -v2.y) < 10 && Mathf.Abs(v1.x - v2.x) < 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // Update is called once per frame
    void Update()
    {   
        if (oldTurn != FightManager.Instance.currentTurn && beginAffect)// 只需要在该turn执行一次,也就是如果下一帧还是这个turn的话就不执行，不是的话更新oldturn并执行
        {
            oldTurn = FightManager.Instance.currentTurn;
            if (FightManager.Instance.currentTurn <= startBuffTurn + buffExecutedTurns-1 && FightManager.Instance.currentTurn >= startBuffTurn)//处于buffTurn
            {
                //把这个和Card都合成为一个prefabs,后者只在battleGameMaster初始化时合成物体（prefabs,add 4 script和接口）最好把实例化这个过程封装到某个脚本中,后者只在move card调用
                gameObject.GetComponent<IBuff>().BuffEffect();
            }
            else if (FightManager.Instance.currentTurn == endBuffTurn)
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

        if (IsMouseOnTransform.IsMouseOnThisTransform(transform, DistanceCriterion))
        {
            if (shownText == null)
            {
                Debug.Log("Mouse On this position");
                RectTransform rec = Instantiate(textPrefab).GetComponent<RectTransform>();
                rec.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(-1, 0, 0) * 10);
                //Debug.Log(transform.position);
                rec.GetChild(1).GetComponent<Text>().text = description;
                Transform bg = rec.GetChild(0);
                bg.localScale = new Vector3(bg.localScale.x, bg.localScale.y * (Mathf.Floor(description.Length / 20) + 1), bg.localScale.z);
                //Debug.Log(rec.gameObject.name);
                rec.SetParent(canvas);
                shownText = rec.gameObject;
            }
        }
        else
        {
            if (shownText != null)
            {
                Destroy(shownText);
                Debug.Log("Destroy Description");
                shownText = null;
            }
        }
    }
}
