using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeFour: MonoBehaviour, IAttackRange
{
    [Header("自动获取变量")]
    public Player player;
    private void Start()
    {
        player = GameObject.Find("BattleGameMaster").GetComponent<BattleGameMaster>().player.GetComponent<Player>();
    }
    // Start is called before the first frame update
    public bool isInAttackRange(MapPoint mapPoint,Character Player) 
    {
        
        Debug.Log("CheckingValidAttack");
        foreach (MapPoint mapPoint1 in Player.onMapPoint.aroundMapPoints)
        {
            if(mapPoint1 == mapPoint)
            {
                Debug.Log("ValidAttacks");
                return true;
                
            }
        }
        Debug.Log("InvalidAttack");
        return false;
       
    }
}
