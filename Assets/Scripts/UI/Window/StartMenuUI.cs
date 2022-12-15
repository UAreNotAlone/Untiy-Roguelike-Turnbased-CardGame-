using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartMenuUI : UIBase
{

    #region Public Methods
    public void Awake()
    {
        //  As the Game starts
        //  Register all the EventTriggerElements in the UI
        Register("Background/Main Panel - Shadow/Button Container/Play Button").onClick 
            = OnStartGameButton;
        
        //  As the Game Quits
        Register("Background/Main Panel - Shadow/Button Container/Quit Button").onClick 
            = OnQuitGameButton;
    }


    public void OnStartGameButton(GameObject obj, PointerEventData pData)
    {
        //  The Game Start Logic Here
        
        //  Close the StartMenuUI
        UIClose();
        
        //  load the map choosen scene
        
        //  Init the battle scene
        FightManager.Instance.ChangeFightType(FightType.Init);
        
    }
    
    
    
    public void OnQuitGameButton(GameObject obj, PointerEventData pData)
    {
        //  The Game Start Logic Here
        
        //  Close the StartMenuUI
        UIClose();
        
    }
    

    #endregion

}
