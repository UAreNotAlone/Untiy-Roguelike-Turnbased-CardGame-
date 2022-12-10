using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestScriptable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var level = ScriptableObject.CreateInstance<OneTypeItem>();
        level.ItemName = "first";
        AssetDatabase.CreateAsset(level, @"Assets/Resources/ItemInBag/" + level.ItemName + ".asset");//在传入的路径中创建资源
        AssetDatabase.SaveAssets(); //存储资源
        AssetDatabase.Refresh();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
