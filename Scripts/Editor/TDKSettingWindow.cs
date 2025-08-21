using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
public class TDKSettingWindow : EditorWindow
{
    public TDKSetting tDKSetting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    [MenuItem("TDKToolKit/Setting")]
    static void ShowWindow()
    {
        GetWindow<TDKSettingWindow>();
    }
    public void OnGUI()
    {

        GUILayout.Space(10);
        tDKSetting = (TDKSetting)EditorGUILayout.ObjectField("预设文件路径", tDKSetting, typeof(TDKSetting), true);

    }

}
