using System.Collections;
using System.Collections.Generic;
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

        // Nifty little trick to quickly position the window in the middle of the editor.
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
    }


}