using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blind : MonoBehaviour,IBuff
{

    public bool beginCheckUsed = false;
    public void BuffEffect()//在buff中被（可以延迟几回合）调用
    {
        FightManager.Instance.isCannotHurtOnce = true;
        beginCheckUsed = true;
    }
    public void Update()
    {
        if (beginCheckUsed)
        {
            if (FightManager.Instance.isCannotHurtOnce == false)
            {
                Debug.Log("used cannot hurt once");
                gameObject.GetComponent<Buff>().buffTransform.GetComponent<buffIconManager>().RemoveBuff(transform);
            }
        }
    }
}
