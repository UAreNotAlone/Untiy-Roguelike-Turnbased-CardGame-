using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackMode : MonoBehaviour
{
    //选中卡牌后对每个敌人进行判断其是否在有效范围内，如果在则发动效果并打出卡牌，否则直接回到未选中状态（并提示玩家）//并显示可以攻击的范围
    // Start is called before the first frame update
    public CardClass ThisCard;
    public IAttackRange CheckAttackValid;
    public BattleGameMaster battleGameMaster;
    void Start()
    {
        ThisCard = gameObject.GetComponent<CardClass>();
        battleGameMaster = GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>();
        CheckAttackValid = this.gameObject.GetComponent<IAttackRange>();//这个接口里面有一个函数会检测某个地图点是否在玩家的攻击范围内
    }

    // Update is called once per frame
    void Update()
    {
        if(ThisCard.thisCardStage == CardClass.ThisCardStage.Chosen)
        {
            List<Transform> targetList = new List<Transform>();
            foreach(Enemy enemy in battleGameMaster.enemies)
            {
                if (CheckAttackValid.isInAttackRange(enemy.onMapPoint, battleGameMaster.player))
                {
                    targetList.Add(enemy.transform);
                }
            }
            if(targetList.Count == 0)
            {
                ThisCard.thisCardStage = CardClass.ThisCardStage.cardNotAroundMouse;
                Debug.Log("There is no enemy in valid ranges");
            }
            else
            {
                gameObject.GetComponent<CardClass>().UseCauseEffect(targetList);//包含了检查能耗，发动效果，设置状态，摧毁卡牌//targetList
            }
        }
    }
}
