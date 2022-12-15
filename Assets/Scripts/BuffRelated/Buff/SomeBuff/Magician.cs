using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : MonoBehaviour,IBuff

{
    

    //使用buff时，先实例化buff，最后设置buff参数
    [Header("手动设置参数")]
    public int initialIncreaseShield = 0;
    
    
    //
    // Start is called before the first frame update

    public void BuffEffect()
    {
        AudioManager.Instance.PlayEffectByName("Effect/healspell");
        //  Enhance the Shield.
        FightManager.Instance.player_defenseValue += initialIncreaseShield + FightManager.Instance.currentTurn;
        UIManager.Instance.GetUIScript<FightUI>("FightUI").UpdateDefenseTxt();
    }
}
