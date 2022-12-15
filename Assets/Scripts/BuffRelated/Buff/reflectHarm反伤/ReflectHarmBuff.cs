using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectHarmBuff : MonoBehaviour,IBuff
{


    //在写反伤卡牌时，应该先实例化buff，再设置免伤的对象和比例，最后再设置buff的参数（达到开启buff的效果）,还有更新buff图标

    //电脑执行顺序：实例化buff，设置反伤对象，开启buff，buff调用反伤效果，buff终止反伤效果
    public bool beginCheckUsed = false;
    public void BuffEffect()//在buff中被（可以延迟几回合）调用
    {
        FightManager.Instance.isPlayerReflectHarm = true;
        beginCheckUsed = true;
    }
    public void Update()
    {
        if (beginCheckUsed)
        {
            if(FightManager.Instance.isPlayerReflectHarm == false)
            {
                Debug.Log("reflect harm");
                gameObject.GetComponent<Buff>().buffTransform.GetComponent<buffIconManager>().RemoveBuff(transform);
            }
        }
    }

}
