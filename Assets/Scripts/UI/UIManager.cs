using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Mono.Cecil;
using UnityEngine;
using DG.Tweening;
//using UnityEditor.Compilation;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
  public static UIManager Instance { get; private set; }

  [SerializeField] private Transform canvasTF;  //  Just a transform for getting the position to position the UI.
  private List<UIBase> uiList;  //  A list which stores all the loaded UI

  private void Awake()
  {
    //  Make sure there can only be one Instance of UIManager in the scene
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }
    else
    {
      Instance = this;
      canvasTF = GameObject.Find("Canvas").transform;
      uiList = new List<UIBase>();
    }
    
  }



  #region Public Methods

  public T GetUIScript<T>(string UIName) where T : UIBase
  {
    UIBase ui = FindUIByName(UIName);
    if (ui != null)
    {
      return ui.GetComponent<T>();
    }

    return null;
  }
  
  //  Show UI On the Screen
  public UIBase ShowUIByName<T>(string UIName) where T : UIBase
  {
    UIBase ui = FindUIByName(UIName);
    //  Change the Background
    if (UIName == "FightUI")
    {
      //  Better way is to load the background image just like loading a prefab/ui .
      GameObject gameScrollingBackground = GameObject.Find("QuadForground");
      gameScrollingBackground.GetComponent<MeshRenderer>().enabled = false;
      
      GameObject gameScrollingBackgroundsub = GameObject.Find("QuadForground (1)");
      gameScrollingBackgroundsub.GetComponent<MeshRenderer>().enabled = false;

    }
    if (ui == null)
    {
      ui = LoadUIByName<T>(UIName);
      uiList.Add(ui);
    }
    else
    {
      //  Show UI
      ui.UIShow();
    }
    return ui;
  }
  
  
  //  Hide UI On the Screen
  public void HideUIByName(string UIName)
  {
    UIBase ui = FindUIByName(UIName);
    if (ui != null)
    {
      ui.UIHide();
    }
    else
    {
      Debug.Log("[WANING][HideUIByName]:This UI is not loaded, you can not hide it");
      return;
    }
  }

  public void CloseAllUI()
  {
    for (int i = uiList.Count - 1; i >= 0; i--)
    {
      //  Invoke this rather then UIClose() is quite convenient and efficient
      //  although it is quite weird
      Destroy(uiList[i].gameObject);
    }
    uiList.Clear();
    
  }
  
  
  //  Close UI on the Screen
  public void CloseUIByName(string UIName)
  {
    UIBase ui = FindUIByName(UIName);
    if (ui != null)
    {
      uiList.Remove(ui);
      Destroy(ui.gameObject);
    }
    else
    {
      Debug.Log("[WANING][CloseUIByName]:This UI is not loaded, you can not close it");
      return;
    }
  }
  
  
  
  
  //  Load the UI from ./Resource/UI by the name of the UI
  public UIBase LoadUIByName<T>(string UIName) where T : UIBase
  {
      UIBase ui;
      GameObject uiObj = Instantiate(Resources.Load("UI/" + UIName), canvasTF) as GameObject;
      uiObj.name = UIName;
      //  Add the Scripts we need according to the type of the UI.
      ui = uiObj.AddComponent<T>();
      return ui;
  }
 
  
  
  //  Find the UI with name from the uiList in the UIManager.cs
  public UIBase FindUIByName(string UIName)
  {
    for (int i = 0; i < uiList.Count; i++)
    {
      if (uiList[i].name == UIName)
      {
        return uiList[i];
      }
    }
    return null;
  }

  //  Showing what the enemy will do
  public GameObject LoadActionIcon()
  {
    GameObject actionIcon_obj = Instantiate(Resources.Load("UI/EnemyActionIcon"), canvasTF) as GameObject;
    actionIcon_obj.transform.SetAsFirstSibling();
    return actionIcon_obj;
  }


  //  Load the HP UI component to the Enemy.
  public GameObject LoadHpItem()
  {
    GameObject hpItem_obj = Instantiate(Resources.Load("UI/EnemyHpItem"), canvasTF) as GameObject;
    hpItem_obj.transform.SetAsFirstSibling();
    return hpItem_obj;
  }

  //
  public void ShowTurnInformation(string message, Color color, System.Action callback = null)
  {
    GameObject turnInfo_obj = Instantiate(Resources.Load("UI/Tips"), canvasTF) as GameObject;
    turnInfo_obj.transform.SetAsLastSibling();
    
    Text turnInfo_text = turnInfo_obj.transform.Find("bg/Text").GetComponent<Text>();
    turnInfo_text.color = color;
    turnInfo_text.text = message;
    //  Generate Scaling Animation
    Tween scale1 = turnInfo_obj.transform.Find("bg").DOScaleY(1, 0.5f);
    Tween scale2 = turnInfo_obj.transform.Find("bg").DOScaleY(0, 0.5f);

    Sequence seq = DOTween.Sequence();
    seq.Append(scale1);
    seq.AppendInterval(1.5f);
    seq.Append(scale2);
    seq.AppendCallback(delegate()
    {
      if (callback != null)
      {
        callback();
      }
    });
    MonoBehaviour.Destroy(turnInfo_obj, 2);

  }
  

  #endregion
  
  
}
