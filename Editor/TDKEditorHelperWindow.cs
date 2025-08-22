using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
public class TDKEditorHelperWindow : OdinEditorWindow
{


    [MenuItem("TDK/TDKEditorHelperWindow")]
    private static void OpenWindow()
    {
        var window = GetWindow<TDKEditorHelperWindow>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
    }
    [MenuItem(TDKEditorLoader.CreateMeanu + "CodeFormat/C# Script UTF-8", false, 20)]
    static void CreateCodeText()
    {
        string defualtClassName = "NewBehaviourScript1";
        string selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        string codeText = "using UnityEngine;\r\n\r\npublic class " + defualtClassName + " : MonoBehaviour\r\n{\r\n\r\n}\r\n";
        File.WriteAllText(selectedPath + "/" + defualtClassName + ".cs", codeText, System.Text.Encoding.UTF8);
        AssetDatabase.Refresh();
    }

    [MenuItem(TDKEditorLoader.HeadMeanu + "code Ansi-> UTF-8")]
    private static void ReadAnsiText()
    {
        UnityEngine.Object selectedObject = Selection.activeObject;
        if (selectedObject != null && selectedObject is TextAsset)
        {
            TextAsset textAsset = selectedObject as TextAsset;

            string assetPath = AssetDatabase.GetAssetPath(textAsset);

            Encoding encoding = Encoding.GetEncoding(936);

            string text = File.ReadAllText(assetPath, encoding);

            System.IO.File.WriteAllText(assetPath, text, Encoding.UTF8);

            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogWarning("请选择一个TextAsset对象进行读取。");
        }
    }



}