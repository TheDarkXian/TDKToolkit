using UnityEditor;
using UnityEngine;
using System.IO;

/// <summary>
/// 解决Unity和Cursor同步问题的编辑器脚本
/// </summary>
public class CursorUnitySync : AssetPostprocessor
{
    /// <summary>
    /// 当资源导入完成后强制刷新项目文件
    /// </summary>
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        bool hasNewCSharpFiles = false;
        
        // 检查是否有新的C#文件被导入
        foreach (string assetPath in importedAssets)
        {
            if (assetPath.EndsWith(".cs"))
            {
                hasNewCSharpFiles = true;
                break;
            }
        }
        
        // 如果有新的C#文件，强制重新生成项目文件
        if (hasNewCSharpFiles)
        {
            RefreshProjectFiles();
        }
    }
    
    /// <summary>
    /// 强制刷新项目文件
    /// </summary>
    static void RefreshProjectFiles()
    {
        // 强制Unity重新生成.csproj文件
        EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
        
        // 刷新AssetDatabase
        AssetDatabase.Refresh();
        
        Debug.Log("已强制刷新项目文件，Cursor应该能识别新脚本了");
    }
    
    /// <summary>
    /// 手动刷新项目文件的菜单项
    /// </summary>
    [MenuItem("TDK/刷新Cursor项目文件")]
    static void ManualRefreshProjectFiles()
    {
        RefreshProjectFiles();
        
        // 额外的同步操作
        EditorApplication.delayCall += () =>
        {
            // 延迟执行，确保文件系统操作完成
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            Debug.Log("手动刷新完成！请检查Cursor是否能识别所有脚本。");
        };
    }
    
    /// <summary>
    /// 创建新脚本时自动刷新
    /// </summary>
    [MenuItem("TDK/创建C#脚本并刷新")]
    static void CreateScriptAndRefresh()
    {
        // 获取选中的文件夹路径
        string selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(selectedPath) || !AssetDatabase.IsValidFolder(selectedPath))
        {
            selectedPath = "Assets";
        }
        
        // 创建新脚本
        string scriptName = "NewScript";
        string scriptPath = AssetDatabase.GenerateUniqueAssetPath($"{selectedPath}/{scriptName}.cs");
        
        string scriptContent = @"using UnityEngine;

public class " + Path.GetFileNameWithoutExtension(scriptPath) + @" : MonoBehaviour
{
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}";
        
        File.WriteAllText(scriptPath, scriptContent);
        AssetDatabase.ImportAsset(scriptPath);
        
        // 选中新创建的脚本
        UnityEngine.Object newScript = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(scriptPath);
        Selection.activeObject = newScript;
        EditorGUIUtility.PingObject(newScript);
        
        // 强制刷新
        EditorApplication.delayCall += () =>
        {
            RefreshProjectFiles();
        };
        
        Debug.Log($"已创建脚本: {scriptPath}");
    }
}

/// <summary>
/// 编辑器启动时的初始化
/// </summary>
[InitializeOnLoad]
public class CursorSyncInitializer
{
    static CursorSyncInitializer()
    {
        // 编辑器启动时执行一次项目文件刷新
        EditorApplication.delayCall += () =>
        {
            if (!SessionState.GetBool("CursorSyncInitialized", false))
            {
                Debug.Log("初始化Cursor同步...");
                AssetDatabase.Refresh();
                EditorApplication.ExecuteMenuItem("Assets/Open C# Project");
                SessionState.SetBool("CursorSyncInitialized", true);
            }
        };
    }
}
