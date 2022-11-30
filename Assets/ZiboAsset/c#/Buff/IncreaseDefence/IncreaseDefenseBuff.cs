using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseDefenseBuff :MonoBehaviour, IBuff
{
    public Character character
    {
        set { _character = value; }
        get { return _character; }
    }

    public Character _character = new Character();//制作卡牌时，先实例化buff，再传入setCharacter,最后设置buff参数
    [Header("手动设置参数")]
    public float increaseDefense = 3;//
                                      // Start is called before the first frame update

    public void BuffEffect()
    {
        character.setDefenceAbility(character.defenceAbility + increaseDefense);
    }
}
