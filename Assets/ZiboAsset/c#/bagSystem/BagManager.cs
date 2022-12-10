using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : MonoBehaviour
{
    
    public GameObject bag;
    public ItemListManager itemListManager;
    public Dictionary<int ,Transform> ItemPositionToOrders = new Dictionary<int, Transform>();//null is meaningless
    private void Awake()
    {
        bag = this.gameObject;
        List<Transform> itemPositions = new List<Transform>(bag.transform.GetComponentsInChildren<Transform>());
        
        itemPositions.Remove(bag.transform);
       for(int i = 0; i < itemPositions.Count; i++)
        {
            ItemPositionToOrders.Add(i, itemPositions[i]);//把背包空位和一个int order联系起来(order从0开始计算)
        }
        //注意最开始更新是加入所有物品
        itemListManager = GameObject.Find("PlayerItemList").GetComponent<ItemListManager>();
        foreach (OneTypeItem type in itemListManager.itemsList)
        {
            //生成物体
            Item obj = Instantiate(type.itemPrefabs);
            obj.transform.position = ItemPositionToOrders[type.itemOrder].transform.position;
            obj.transform.SetParent(ItemPositionToOrders[type.itemOrder].transform);
            obj.couldUsed = true;
            //更新参数
            //注意itemManager是主动更新的一方，此类型只是被动显示视图
        }
        itemListManager.addedItemType = new List<OneTypeItem>();//注意更新完之后把这个设为null
        gameObject.SetActive(false);//只有一开始是自动禁用自己
    }
    public void OnEnable()//开启关闭背包是通过反复禁用启用来实现的
    {
        //获取itemListManager里面的更新列表，更新，并将其更新列表设为none
        foreach (OneTypeItem type in itemListManager.addedItemType)
        {
            //生成物体
            Item obj = Instantiate(type.itemPrefabs);
            obj.transform.position = ItemPositionToOrders[type.itemOrder].transform.position;
            obj.transform.SetParent(ItemPositionToOrders[type.itemOrder].transform);
            obj.couldUsed = true;
        }
        itemListManager.addedItemType = new List<OneTypeItem>();
    }
    public void UseItem(Item item)
    {
        IItemEffect[] itemEffects = item.gameObject.GetComponents<IItemEffect>();
        foreach(IItemEffect itemEffect in itemEffects)
        {
            itemEffect.ItemEffect();
        }
        itemListManager.RemoveItem(item);//减少数量
    }
    public void Update()
    {
        
    }
}
