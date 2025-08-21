using UnityEngine;
using UnityEditor;
using TDKToolkit;
using TDKToolkit.UI;
using UnityEngine.EventSystems;
namespace TDKEditor
{

    public class ContextMenu : Editor
    {

        //ÐÂ½¨¿ò¼Ü
        [MenuItem("GameObject/TDKTookit/BuildFrame", false, 10)]
        static void BuildFrame()
        {
            GameObject UI = TDKToolkitPag.FindOrGetAObj("[UI]");
            GameObject SenceObj = TDKToolkitPag.FindOrGetAObj("[SenceObject]");
            GameObject GameObj = TDKToolkitPag.FindOrGetAObj("[GameObject]");
            GameObject Manger = TDKToolkitPag.FindOrGetAObj("[Manger]");
            GameObject Env = TDKToolkitPag.FindOrGetAObj("[Env]");
            GameObject Effect = TDKToolkitPag.FindOrGetAObj("[Effect]");

            if (!GameObject.FindAnyObjectByType<TDKApplication>())
            {
                GameObject tdkApplication = new GameObject(typeof(TDKApplication).Name);
                tdkApplication.transform.parent = Manger.transform;
                tdkApplication.AddComponent<TDKApplication>();
            }
            if (!GameObject.FindAnyObjectByType<UIManger>())
            {
                GameObject uimanger = new GameObject(typeof(UIManger).Name);
                uimanger.transform.parent = Manger.transform;
                uimanger.AddComponent<UIManger>();
            }





        }




    }

}

