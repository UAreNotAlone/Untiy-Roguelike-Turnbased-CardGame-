using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectharmEndBuff : MonoBehaviour,IEndBuff
{
    //注意此脚本大概率在反伤卡牌被摧毁后，被buff物体调用，调用结果是把物体免伤效果和比例复原为false和1， 
    //实际上对于，反伤once来说，这个是不需要的，但是对于本回合免伤来说，这个是必须的；
    //在使用时再获取头buff的character对象
    public void EndBuff()
    {
        Character cha = gameObject.GetComponent<ReflectHarmBuff>().character;//配对的buff在同一物体上
        cha.isReflectHarmOnce = false;
        cha.reflectProportion = 1f;//反伤比例要不要调回呢？有这个必要吗？以后注意一下。
    }

}
