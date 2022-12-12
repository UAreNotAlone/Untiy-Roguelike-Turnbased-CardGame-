using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

//  This one is for 事件监听
public class UIEventTrigger : MonoBehaviour, IPointerClickHandler
{
    public Action<GameObject, PointerEventData> onClick;



    public static UIEventTrigger GetUIEventTrigger(GameObject obj)
    {
        UIEventTrigger trigger = obj.GetComponent<UIEventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<UIEventTrigger>();
        }
        return trigger;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
        {
            onClick(gameObject, eventData);
        }
    }
}
