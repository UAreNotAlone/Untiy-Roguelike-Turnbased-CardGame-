using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  Stores information related with player for all the time.
public class RoleManager : MonoBehaviour
{
    //  Singleton
    public static RoleManager Instance { get; private set; }

    public List<string> ownedCardIdList;
    public List<string> ownedChipIdList;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void Init()
    {
        ownedCardIdList = new List<string>();
        ownedChipIdList = new List<string>();
        
        
        //  Init the Card owned
        ownedCardIdList.Add("1000");
        ownedCardIdList.Add("1000");
        ownedCardIdList.Add("1000");
        ownedCardIdList.Add("1000");
        
        ownedCardIdList.Add("1001");
        ownedCardIdList.Add("1001");
        ownedCardIdList.Add("1001");
        ownedCardIdList.Add("1001");
        
        ownedCardIdList.Add("1002");
        ownedCardIdList.Add("1002");
        
        ownedCardIdList.Add("1003");
        ownedCardIdList.Add("1004");
        ownedCardIdList.Add("1005");
        ownedCardIdList.Add("1006");
        
        ownedCardIdList.Add("1007");
        ownedCardIdList.Add("1007");
        ownedCardIdList.Add("1007");
        ownedCardIdList.Add("1007");
        ownedCardIdList.Add("1007");
        
        ownedCardIdList.Add("1008");
        ownedCardIdList.Add("1010");
        ownedCardIdList.Add("1011");
        ownedCardIdList.Add("1009");
        
        //  Init the Chip owned
        
        
    }
    
    
}
