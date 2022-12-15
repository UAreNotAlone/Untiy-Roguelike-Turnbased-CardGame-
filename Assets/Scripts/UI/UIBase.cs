using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     The Base class for all the UI 
/// </summary>
public class UIBase : MonoBehaviour
{

    //  [TODO]  This is for what?
    // ->  Get / Set the UIEventTrigger
    public UIEventTrigger Register(string name)
    {
        //  Find some UI elements in the root ?
        Transform obj = transform.Find(name);
        return UIEventTrigger.GetUIEventTrigger(obj.gameObject);
    }

    
    //  Close the UI
    public virtual void UIClose()
    {
        //  Rooted here
        UIManager.Instance.CloseUIByName(gameObject.name);
    }
    
    //  Show the UI
    public virtual void UIShow()
    {
        gameObject.SetActive(true);
    }
    
    //  Hide the UI
    public virtual void UIHide()
    {
        gameObject.SetActive(false);
    }
}
