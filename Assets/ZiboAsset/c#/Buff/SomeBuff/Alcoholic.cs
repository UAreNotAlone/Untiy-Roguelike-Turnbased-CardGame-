using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alcoholic : MonoBehaviour,IBuff
{
    public Character character
    {
        set { _character = value; }
        get { return _character; }
    }

    public Character _character = new Character();//使用buff时，先实例化buff，再传入setCharacter,最后设置buff参数
    public BattleGameMaster battleGameMaster;
    public void Start()
    {
        battleGameMaster = GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>();
    }
    //
    // Start is called before the first frame update

    public void BuffEffect()
    {
        InvokeRepeating(nameof(BanuseCard),0f,0.01f);
    }
    public void BanuseCard()
    {
        battleGameMaster.playerTurnController.stage = GameMasterPlayerTurn.Stage.NoChooseCard;
    }
}
