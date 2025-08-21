using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TDKToolkit.UI
{

    public class UIManger : SingletonMono<UIManger>
    {
        public List<UIPageBase> pageList;
        [ValueDropdown("pageList")]
        [ShowIf("enableDefualtPage")]
        [InspectorName("默认页面")]
        [BoxGroup("默认值设置")]
        public UIPageBase defualtPage;
        [BoxGroup("默认值设置")]
        [InspectorName("启用默认页")]
        public bool enableDefualtPage;
        public Dictionary<string, UIPageBase> pageKeyValue;
        Stack<UIPageBase> pageStack;
        virtual protected void OnValidate()
        {
            pageList = new List<UIPageBase>();
            Transform UInode = TDKHierarchy.UiNode;
            UIPageBase[] uIPageBases = UInode.GetComponentsInChildren<UIPageBase>(true);
            pageList.AddRange(uIPageBases);
            pageKeyValue = new Dictionary<string, UIPageBase>();
            if (Application.isPlaying)
            {
                foreach (var uIPageBase in pageList)
                {
                    uIPageBase.Ini();
                    pageKeyValue.Add(uIPageBase.gameObject.name, uIPageBase);
                }
            }

        }
        virtual protected void Awake()
        {
            OnValidate();
            transform.parent = TDKHierarchy.MangerNode;
            pageStack = new Stack<UIPageBase>();

        }
        virtual protected void Start()
        {

            if (defualtPage != null && pageList.Contains(defualtPage) && enableDefualtPage)
            {
                foreach (var i in pageList)
                {
                    i.HidePage();
                }
                callui(defualtPage);
            }

        }
        protected virtual void callui(UIPageBase uIPageBase)
        {
            if (uIPageBase == null)
            {

                return;
            }
            if (pageStack.Count > 0)
            {
                pageStack.Peek().HidePage();
            }
            pageStack.Push(uIPageBase);
            pageStack.Peek().ShowPage();

        }
        protected virtual void callui(string UIname)
        {
            UIPageBase temp = Getuipage(UIname);
            callui(temp);

        }
        public static void CallUI(UIPageBase UIname)
        {
            Instance.callui(UIname);
        }
        public static void CallUI(string UIname)
        {
            Instance.callui(UIname);

        }
        protected virtual void closeui(UIPageBase uiname)
        {

            UIPageBase temp = uiname;
            temp.HidePage();
            List<UIPageBase> uIPageBases = new List<UIPageBase>();
            uIPageBases.AddRange(pageStack.ToArray());
            uIPageBases.Reverse();
            pageStack.Clear();
            foreach (var i in uIPageBases)
            {
                if (i == temp)
                {

                }
                else
                {
                    pageStack.Push(i);

                }
            }

        }
        protected virtual void closeui(string uiname)
        {
            UIPageBase temp = Getuipage(uiname);
            closeui(temp);

        }
        public static void CloseUI(string UIname)
        {
            Instance.closeui(UIname);

        }
        protected virtual void retrunui()
        {
            if (pageStack.Count > 1)
            {
                pageStack.Pop().HidePage();
            }
            if (pageStack.Count > 0)
            {
                pageStack.Peek().ShowPage();
            }

        }
        public static void RetrunUI()
        {
            Instance.retrunui();
        }
        public virtual UIPageBase Getuipage(string Uiname)
        {

            if (pageKeyValue.ContainsKey(Uiname))
            {
                return Instance.pageKeyValue[Uiname];
            }
            else
            {
                Debug.LogError("没有这个UI" + Uiname + " 啊啊啊啊");
                return null;
            }
        }
        public static UIPageBase GetUiPage(string UIname)
        {


            return Instance.Getuipage(UIname);

        }


        public static T GetUiPage<T>() where T : UIPageBase
        {
            T temp = null;

            foreach (var i in Instance.pageList)
            {
                if (i.GetType() == typeof(T))
                {
                    temp = i as T;
                    break;
                }

            }
            if (!temp)
                Debug.LogError($"当前UI管理器中并不存在这个【{typeof(T).Name}】东西");
            return temp;
        }



    }


}
