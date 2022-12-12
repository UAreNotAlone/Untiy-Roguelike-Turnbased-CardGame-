using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  Entrance of the game (?)
public class GameApp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //  Init Config
        GameConfigManager.Instance.Init();
        //  Init Audio
        AudioManager.Instance.InitAudio();
        //  Init Role Info
        RoleManager.Instance.Init();
        //  Load the Start Menu UI
        UIManager.Instance.ShowUIByName<StartMenuUI>("StartMenuUI");
        //  Play The BGM
        AudioManager.Instance.PlayBGMByName("base_BGM");
     
        
    }
}
