using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestClik : MonoBehaviour
{

    private Button btn_Start;//定义一个Button类型的变量

    private void Start()
    {
        btn_Start = GameObject.Find("Button").GetComponent<Button>();//通过Find查找名称获得我们要的Button组件
        //Debug.Log(GameObject.Find("Button").GetComponent<Button>() == null);
        btn_Start.onClick.AddListener(OnStartButtonClick);//监听点击事件
    }
    /// <summary>
    /// 点击的之后调用的方法
    /// </summary>
    private void OnStartButtonClick()
    {
        Debug.Log("我是大聪明");
    }

///————————————————
//版权声明：本文为CSDN博主「木木娅.」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
//原文链接：https://blog.csdn.net/muziiii/article/details/118995029

   
}
