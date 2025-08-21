using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace TDKToolkit.UI
{
    public abstract class UIPageBase : MonoBehaviour
    {
        public static string UIPAGE_SHOW_PAGE = "UipageShowPage";
        public static string UIPAGE_HIDE_PAGE = "UipageHidePage";
        [FoldoutGroup("ҳ������")]
        public bool enableOnAwake;
        [FoldoutGroup("ҳ������")]
        public RectTransform Context;
        public UnityAction InvokeOnShow;
        public UnityAction InvokeOnHide;
        protected bool customShow;
        public virtual void OnValidate()
        {
            if (Context == null)
            {
                Transform contextTemp = transform.Find("Context");
                if (contextTemp == null)
                {
                    contextTemp = new GameObject("Context").transform;
                }
                contextTemp.transform.parent = this.transform;
                Context = contextTemp.GetComponent<RectTransform>();
                if (Context == null)
                {
                    Context = contextTemp.gameObject.AddComponent<RectTransform>();
                }
                Context.anchorMin = Vector2.zero;
                Context.anchorMax = Vector2.one;
                Context.anchoredPosition = Vector2.zero;
                Context.sizeDelta = Vector2.zero;

            }
            if (!GetComponent<RectTransform>())
            {
                RectTransform temp = this.gameObject.AddComponent<RectTransform>();
                temp.anchorMin = Vector2.zero;
                temp.anchorMax = Vector2.one;
                temp.anchoredPosition = Vector2.zero;
                temp.sizeDelta = Vector2.zero;

            }

        }
        public bool isShow
        {
            get { return Context.gameObject.activeSelf; }
        }

        public virtual void Ini()
        {

            if (enableOnAwake)
            {
                ShowPage();
            }
        }
        [BoxGroup("�����ҳ�����")]
        [Button("չʾ���ҳ��")]
        public virtual void ShowPage()
        {
            UIEventCentry.TriggerEvent(UIPAGE_SHOW_PAGE, this);
            InvokeOnShow?.Invoke();
            if (!customShow)
                Context.gameObject.SetActive(true);


        }
        [BoxGroup("�����ҳ�����")]
        [Button("�ر����ҳ��")]
        public virtual void HidePage()
        {
            UIEventCentry.TriggerEvent(UIPAGE_HIDE_PAGE, this);
            InvokeOnHide?.Invoke();
            if (!customShow)
                Context.gameObject.SetActive(false);
        }




    }
}

