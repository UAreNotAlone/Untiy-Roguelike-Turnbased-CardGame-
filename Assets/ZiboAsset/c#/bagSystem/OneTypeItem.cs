using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OneTypeItem", menuName = "一类物品", order = 1)]
public class OneTypeItem: ScriptableObject
{
    public string ItemName;
    public int itemOrder;
    public int itemNumber;
    public Item itemPrefabs;
    
}
