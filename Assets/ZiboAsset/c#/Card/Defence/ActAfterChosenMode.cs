using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActAfterChosenMode : MonoBehaviour
{
    public CardClass ThisCard;
    public BattleGameMaster battleGameMaster;
    void Start()
    {
        ThisCard = gameObject.GetComponent<CardClass>();
        battleGameMaster = GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>();//在防御模式选中直接执行
    }
    public void Update()
    {
        if(ThisCard.thisCardStage == CardClass.ThisCardStage.Chosen)
        {
            List<Transform> target = new List<Transform>();
            target.Add(battleGameMaster.player.transform);
            ThisCard.UseCauseEffect(target);
        }
    }

}
