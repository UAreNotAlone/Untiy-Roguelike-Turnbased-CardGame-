using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

//  [TODO]: Move this to the ENUM folder and include here
public enum FightType
{
    None,
    Init,
    Player, //  Player's turn
    Enemy,  //  Enemy's turn
    Win,
    Loss
}

/// <summary>
/// Fight Manager
/// ->  Description:
///     ->
/// </summary>
public class FightManager : MonoBehaviour
{
    public static FightManager Instance;
    public FightUnit fightUnit;
    
    //  For Player's UI Information
    public int player_MAXHP;
    public int player_curHP_head;
    public int player_curHP_leg;
    public int player_curHP_body;
    public int player_curHP_hand;
    //  Mana
    public int player_MAXMana;
    public int player_curMana;
    //  Defense
    public int player_defenseValue;
    //增加：玩家攻击力
    public int player_AttackValue = 0;
    //增加：玩家是否能够反伤
    public bool isPlayerReflectHarm = false;
    //增加：记录回合数
    public int currentTurn;
    //增加：免除一次伤害
    public bool isCannotHurtOnce = false;
    public void Init()
    {
        player_MAXHP = 10;
        
        player_curHP_hand = 10;
        player_curHP_body = 10;
        player_curHP_head = 10;
        player_curHP_leg = 10;
        
        player_MAXMana = 3;
        player_curMana = 3;
        player_defenseValue = 10;
        //增加：初始化回合数量
        currentTurn = 0;

    }

    private void Awake()
    {
        //  Avoid duplication
        if (Instance != null && Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    //  Switch from different fight type
    public void ChangeFightType(FightType TarfightType)
    {
        //  Directly assign a new FightUnit into the fightUnit.
        switch (TarfightType)
        {
            case FightType.None:
                break;
            
            case FightType.Init:
                fightUnit = new FightInit();
                break;
            
            case FightType.Player:
                fightUnit = new Fight_PlayerTurn();
                break;

            case FightType.Enemy:
                fightUnit = new Fight_EnemyTurn();
                break;
            
            case FightType.Win:
                fightUnit = new Fight_Win();
                break;
            
            case FightType.Loss:
                fightUnit = new Fight_Loss();
                break;
            
        }
        
        //  Init the fightUnit
        fightUnit.Init();
        
    }

    //  Should be triggered when player is within range.
    public void GetPlayerHit(int calculated_val)
    {
        //免除伤害判断
        if (isCannotHurtOnce)
        {
            calculated_val = 0;
            isCannotHurtOnce = false;
        }
        //  Defense
        if (player_defenseValue >= calculated_val)
        {
            player_defenseValue -= calculated_val;
        }
        else
        {
            calculated_val -= player_defenseValue;
            player_defenseValue = 0;
            int cyberBorgChoosen = UnityEngine.Random.Range(0, 4);
            
            //  Random a cyber body to attack.
            switch (cyberBorgChoosen)
            {
                case 0:
                    //  Head
                    player_curHP_head -= calculated_val;
                    if (player_curHP_head <= 0)
                    {
                        player_curHP_head = 0;
                    }
                    break;
                case 1 :
                    //  Body
                    player_curHP_body -= calculated_val;
                    if (player_curHP_body <= 0)
                    {
                        player_curHP_body = 0;
                    }
                    break;
                case 2:
                    //  Hands
                    player_curHP_hand -= calculated_val;
                    if (player_curHP_hand <= 0)
                    {
                        player_curHP_hand = 0;
                    }
                    break;
                case 3:
                    //  Legs
                    player_curHP_leg -= calculated_val;
                    if (player_curHP_leg <= 0)
                    {
                        player_curHP_leg = 0;
                    }
                    break;
                    
                    
            }
            
            //  Player DEAD
            if (player_curHP_body == 0 && player_curHP_hand == 0 && player_curHP_head == 0 && player_curHP_leg == 0)
            {
                ChangeFightType(FightType.Loss);
            }
            
            
        }
        //  Update UI.
        UIManager.Instance.GetUIScript<FightUI>("FightUI").UpdateAllHP();
        UIManager.Instance.GetUIScript<FightUI>("FightUI").UpdateDefenseTxt();
    }
    
    private void Update()
    {
        if (fightUnit != null)
        {
            fightUnit.OnFightUpdate();
        }
        
    }
}
