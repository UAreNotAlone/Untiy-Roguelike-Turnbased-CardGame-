using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blind : MonoBehaviour,IBuff
{
    public Character character
    {
        set { _character = value; }
        get { return _character; }
    }

    public Character _character = new Character();//使用buff时，先实例化buff，再传入setCharacter,最后设置buff参数
    
        //
         // Start is called before the first frame update

    public void BuffEffect()
    {
        character.isCannotHurtOnce = true;
    }
}
