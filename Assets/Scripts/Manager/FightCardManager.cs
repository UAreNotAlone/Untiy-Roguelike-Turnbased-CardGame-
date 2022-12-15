using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCardManager
{
    public static FightCardManager Instance = new FightCardManager();
    public List<string> currentCardList;
    public List<string> usedCardList;
    
    
    
    

    public void Init()
    {
        currentCardList = new List<string>();
        usedCardList = new List<string>();
        
        //  Initial Draw Card from player owned card list.
        
        //  temp
        List<string> tempList = new List<string>();
        tempList.AddRange(RoleManager.Instance.ownedCardIdList);

        while (tempList.Count > 0)
        {
            //  Init card is taken by random order.
            //  Generate a random index
            int tempIndex = Random.Range(0, tempList.Count);
            
            //  Add to the current card list.
            currentCardList.Add(tempList[tempIndex]);
            
            tempList.RemoveAt(tempIndex);
            
        }
    }


    public bool isPlayerHasCard()
    {
        return currentCardList.Count > 0;
    }


    public string PlayerDrawOneCard()
    {
        string ChoosenCardID = currentCardList[currentCardList.Count - 1];
        currentCardList.RemoveAt(currentCardList.Count - 1);
        return ChoosenCardID;
    }
}
