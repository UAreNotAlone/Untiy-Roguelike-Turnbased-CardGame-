using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetFile : UnityEditor.Editor
{


    const string assetsFolderName = "AssetFiles";
    

    [MenuItem("Editor/CreateSetingAsset")]
    public static void CreateAsset()
    {
        // 实例化类
        Setting asset = ScriptableObject.CreateInstance<Setting>();
        GameObject m =(GameObject) Resources.Load("Text");

        // 如果实例化 Bullet 类为空，返回
       
        if (!asset)
        {
            Debug.LogWarning("Bullet not found");
            return;
        }
        // 自定义资源保存路径
        string path = Application.dataPath + "/" + assetsFolderName + "/";
        GameObject n = Instantiate(m);
        PrefabUtility.SaveAsPrefabAsset(n, "Assets/"+"a.prefab");
        GameObject.DestroyImmediate(n);

        // 如果项目总不包含该路径，创建一个
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        //将类名 Setting 转换为字符串
        //拼接保存自定义资源（.asset） 路径
        path = string.Format("Assets/" + assetsFolderName + "/{0}.asset", (typeof(Setting).ToString()));
        // 生成自定义资源到指定路径
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}