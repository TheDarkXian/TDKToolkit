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

        public enum �������
        {
            ����,
            �任��
        }
        private void OnValidate()
        {
            enableDebug = Debuglog;
        }
        public bool Debuglog;
        public static bool enableDebug;
        [EnumToggleButtons]
        public ������� op;
        [BoxGroup("�ṹ������")]
        [Title("Դͷ")]
        public Transform topuSource;
        [BoxGroup("�ṹ������")]
        [Title("Ŀ��")]
        public Transform topuTarget;
        bool ishavesourceAandTarget
        {
            get
            {
                return topuSource && topuTarget;
            }
        }

        [BoxGroup("�ṹ������")]
        [Button("�ṹ������")]
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
        //�����������ݹ����
        void topuChlid(Transform sourcetemp, Transform targettemp)
        {

            switch (op)
            {
                case �������.����:
                    Topologis<RectTransform>(sourcetemp, targettemp);
                    break;
                case �������.�任��:
                    Topologis<Transform>(sourcetemp, targettemp);

                    break;
            }




        }

        //��������ʼCOPY
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

                        Debug.Log(source + " ����û�������� " + typeof(T));

                    }
                    if (targetComponent == null) { Debug.Log(target + " ����û�������� " + typeof(T)); }
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
