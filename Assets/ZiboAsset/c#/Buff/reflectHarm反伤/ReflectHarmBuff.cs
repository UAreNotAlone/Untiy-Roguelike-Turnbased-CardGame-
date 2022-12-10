using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectHarmBuff : MonoBehaviour,IBuff
{
    public Character character
    {
        set { _character = value; }
        get { return _character; }
    }

    public Character _character;
    public float percentage = 1f;
    public void setCharacter(Character p_cha, float p_percentage = 1)//在写反伤卡牌时，应该先实例化buff，再设置免伤的对象和比例，最后再设置buff的参数（达到开启buff的效果）,还有更新buff图标
    {
        character = p_cha;                                           //电脑执行顺序：实例化buff，设置反伤对象，开启buff，buff调用反伤效果，buff终止反伤效果
        percentage = p_percentage;//
    }
    public void BuffEffect()//在buff中被（可以延迟几回合）调用
    {
        character.isReflectHarmOnce = true;
        character.reflectProportion = percentage;
    }
    
}
