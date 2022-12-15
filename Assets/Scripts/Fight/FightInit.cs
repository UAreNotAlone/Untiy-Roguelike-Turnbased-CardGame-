using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  The initialization process for the fight scene.
public class FightInit : FightUnit
{
    //  This will be called in the StartGameSceneUI
    //  ->  when the start game button is pressed
    //  ->  the fightManager will change type to FightInit
    public override void Init()
    {
        
        //  Init fight data
        FightManager.Instance.Init();
        //  Switch BGM
        AudioManager.Instance.PlayBGMByName("fight_BGM");
        
        //  Load the map
        MapManager.Instance.InitMap();
        
        //  Init the Enemy - > display
        //  ->  This parameter should be changed when player wants to choose the level.
        EnemyManager.Instance.LoadEnemyResources("10003");
        
        
        //  Init the Player -> display      
        PlayerManager.Instance.LoadPlayerResources("10003");
        
        //  Init the Fight Card
        FightCardManager.Instance.Init();
        
        //  Load the Battle UI.
        UIManager.Instance.ShowUIByName<FightUI>("FightUI");
        
        
        
  
        
        //  Change to the Player's turn
        FightManager.Instance.ChangeFightType(FightType.Player);
        base.Init();
    }


    public override void OnFightUpdate()
    {
        base.OnFightUpdate();
    }

}
