using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBagSystem : MonoBehaviour
{
    public ItemListManager itemListManager;
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        itemListManager = GameObject.Find("PlayerItemList").GetComponent<ItemListManager>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
