using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour,IBuff
{
    

    
    
    //
    // Start is called before the first frame update

    public void BuffEffect()
    {

        FightManager.Instance.GetPlayerHit(4);
        Debug.Log("execute dead");
      
    }
}
