using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recoverHealth : MonoBehaviour,IItemEffect
{
    [Header("需要手动设置的变量")]
    public List<Transform> ItemTargets;
    public float cureEffect = 10;
    public void Start()
    {
        ItemTargets.Add(GameObject.Find(nameof(BattleGameMaster)).GetComponent<BattleGameMaster>().player.transform);
    }
    public void ItemEffect()
    { 
        foreach(Transform target in ItemTargets)
        {
            if(target.GetComponent<Damagable>() != null)
            {
                target.GetComponent<Damagable>().CureHealth(10);
            }
            else
            {
                Debug.Log("no damageable to cure");
            }
        }
    }
}
