using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ItemListManager : MonoBehaviour
{
    public Dictionary<string, OneTypeItem> itemNamesToType = new Dictionary<string, OneTypeItem>();
    public List<int> remainingPositionOrder = new List<int>();

    public List<OneTypeItem> itemsList = new List<OneTypeItem>();
    //为了方便背包在重启时快速更新，最好加上一个add列表和(remove列表?),并且在背包重启后重设这两个列表
    public List<OneTypeItem> addedItemType = new List<OneTypeItem>();//
    [Header("手动设置变量（和关卡无关）")]
    public GameObject bag;
    
    // Start is called before the first frame update
    //加载存档中的物品列表
    public void Awake()
    {
        //物体被禁用后无法通过find找到
        //bag = GameObject.Find("bag");
        for(int i = 0; i < bag.transform.childCount; i++)
        {
            remainingPositionOrder.Add(i);
            
        }
        
        foreach(OneTypeItem item in itemsList)
        {
            remainingPositionOrder.Remove(item.itemOrder);
        }
        //开始时空余背包位置
        Debug.Log("bag have children" + bag.transform.childCount);
        //背包所有物品种类
        foreach(OneTypeItem item in itemsList)
        {
            itemNamesToType.Add(item.ItemName,item);
        }


    }
    public void LoadItemList()
    {
        //摧毁原有列表
        //利用存档字符列表获取ItemScriptableObject in resourcess
    }
    //添加item （输入一个实例化的Item（），创建一个scriptableObject(OneTypeItem)并加入列表,存档时记得读取这个列表
    public void AddItem(Item item)//实例化的item对象
    {
        foreach(string Iname in itemNamesToType.Keys)
        {
            Debug.Log(item.ItemName+""+Iname);
            if(item.ItemName == Iname)
            {
                Debug.Log("same name");
                itemNamesToType[item.ItemName].itemNumber += 1;
                return;
            }//当加入物品在背包中已经存在时，物品数量加一，同时剩余空位和nametype对应关系不变
        }
        if (remainingPositionOrder.Count >= 1)//当有空位且没有该item时,
        {
            
            var level = ScriptableObject.CreateInstance<OneTypeItem>();
            level.ItemName = item.ItemName;
            level.itemPrefabs = item.thisPrefab;//注意只有这个prefab可以赋值给asset
            level.itemNumber = 1;
            level.itemOrder = remainingPositionOrder[0];
            remainingPositionOrder.Remove(remainingPositionOrder[0]);
           
            itemNamesToType.Add(level.ItemName, level);
            
            itemsList.Add(level);
            addedItemType.Add(level);//待更新列表

            AssetDatabase.CreateAsset(level, @"Assets/Resources/ItemInBag/" + level.ItemName + ".asset");//在传入的路径中创建资源
            AssetDatabase.SaveAssets(); //存储资源
            AssetDatabase.Refresh();
        }
        else//没有空位时
        {
            Debug.Log("bag is full");
        }
    }
    //移除物体
    public void RemoveItem(Item item)//这里传入的参数是一个实例化的Item prefab,相当于玩家用尽一个物品(在背包里面)
    {
        Debug.Log("exe");
        List<string> list = new List<string>(itemNamesToType.Keys);
        //检查或不查itemType是否在itemlist里面似乎没有意义(如果调用方法得当的话)，
        foreach (string Iname in list)
        {
            Debug.Log(Iname + " " + item.ItemName);
            if(Iname == item.ItemName)
            {
                itemNamesToType[item.ItemName].itemNumber -= 1;
                if (itemNamesToType[item.ItemName].itemNumber <= 0)
                {
                    remainingPositionOrder.Add(itemNamesToType[item.ItemName].itemOrder);
                    remainingPositionOrder.Sort();
                    itemNamesToType.Remove(item.ItemName);
                    Destroy(item.gameObject);//删除物体
                    string path = "Assets/Resources/ItemInBag/" + item.ItemName + ".asset";
                    
                    File.Delete(path);
                    // 注意如果不摧毁相应的scriptableObject,就要在原先加一个路径检测（unity似乎会自动覆盖），
                    //最好不要在这里摧毁，这里只负责数据，在bagManager（视图）中摧毁?
                    AssetDatabase.SaveAssets(); //注意更新
                    AssetDatabase.Refresh();
                }
            }
        }

    }
}
