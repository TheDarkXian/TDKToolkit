#if UNITY_EDITOR
using UnityEditorInternal;
#endif
using UnityEngine;
using Sirenix.OdinInspector;
namespace TDKToolkit
{
    public class Topologistor : MonoBehaviour
    {
#if UNITY_EDITOR

        public enum 拓扑组件
        {
            画布,
            变换器
        }
        private void OnValidate()
        {
            enableDebug = Debuglog;
        }
        public bool Debuglog;
        public static bool enableDebug;
        [EnumToggleButtons]
        public 拓扑组件 op;
        [BoxGroup("结构拓扑器")]
        [Title("源头")]
        public Transform topuSource;
        [BoxGroup("结构拓扑器")]
        [Title("目标")]
        public Transform topuTarget;
        bool ishavesourceAandTarget
        {
            get
            {
                return topuSource && topuTarget;
            }
        }

        [BoxGroup("结构拓扑器")]
        [Button("结构拓扑器")]
        [ShowIf("ishavesourceAandTarget")]
        public void TOPURect()
        {

            topuChlid(topuSource, topuTarget);


        }
        public static void Topologis<T>(Transform source, Transform target) where T : Component
        {
            if (source == null || target == null)
            {
                return;
            }
            CopyThenPaste<T>(source, target);
            int getchild = source.childCount;
            for (int i = 0; i < getchild; i++)
            {
                Topologis<T>(source.GetChild(i), target.Find(source.GetChild(i).name));
            }

        }
        //拓扑子树，递归调用
        void topuChlid(Transform sourcetemp, Transform targettemp)
        {

            switch (op)
            {
                case 拓扑组件.画布:
                    Topologis<RectTransform>(sourcetemp, targettemp);
                    break;
                case 拓扑组件.变换器:
                    Topologis<Transform>(sourcetemp, targettemp);

                    break;
            }




        }

        //接下来开始COPY
        static void CopyThenPaste<T>(Transform source, Transform target) where T : Component
        {

            T sourceComponent = source.GetComponent<T>();
            T targetComponent = target.GetComponent<T>();
            if (sourceComponent != null && targetComponent != null)
            {

                Copy(sourceComponent);
                Paste(targetComponent);

            }
            else
            {
                if (enableDebug)
                {
                    if (sourceComponent == null)
                    {

                        Debug.Log(source + " 这里没有这个组件 " + typeof(T));

                    }
                    if (targetComponent == null) { Debug.Log(target + " 这里没有这个组件 " + typeof(T)); }
                }

            }


        }
        static void Copy(Component component)
        {
#if UNITY_EDITOR
            ComponentUtility.CopyComponent(component);
#endif
        }
        static void Paste(Component component)
        {
#if UNITY_EDITOR

            ComponentUtility.PasteComponentValues(component);
#endif

        }
#endif
    }
}
