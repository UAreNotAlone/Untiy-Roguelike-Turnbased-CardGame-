using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alcoholic : MonoBehaviour,IBuff
{
    

    //使用buff时，先实例化buff，再传入,最后设置buff参数
    
    
    //
    // Start is called before the first frame update

    public void BuffEffect()
    {
        //跳过本回合
        //
        StartCoroutine(nameof(EndTurn));
        
    }
    public IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(3f);
        FightManager.Instance.ChangeFightType(FightType.Enemy);
    }
    
}
