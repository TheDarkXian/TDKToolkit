using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;


namespace TDKToolkit
{
    static public class TDKToolkitPag
    {
        public static Transform FindTransform(string name)
        {
            GameObject temp = GameObject.Find(name);
            Transform temptr = temp ? temp.transform : null;
            return temptr;
        }
        public static Transform FindOrGetTransform(string targetname, Transform parent = null)
        {
            Transform transform = null;
            GameObject temp = FindOrGetAObj(targetname, parent);
            if (temp != null) { transform = temp.transform; }


            return transform;
        }
        public static GameObject FindOrGetAObj(string targetname, Transform parent = null)
        {
            GameObject temp = null;
            if (parent)
            {
                Transform temp2 = null;
                temp2 = parent.Find(targetname);
                if (temp2)
                {
                    temp = temp2.gameObject;
                }
            }
            else
            {
                temp = GameObject.Find(targetname);
            }

            if (temp == null)
            {
                temp = new GameObject(targetname);
            }
            temp.transform.parent = parent;
            return temp;
        }
        public static void QuitGame()
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

        }

        public static List<string> GetBuildSceneList()
        {
            List<string> scenes = new List<string>();
#if UNITY_EDITOR
            var arraryscenes = new string[EditorBuildSettings.scenes.Length];
            for (int i = 0; i < arraryscenes.Length; i++)
            {
                arraryscenes[i] = System.IO.Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
            }
            scenes.AddRange(arraryscenes);
#endif
            return scenes;


        }

        public static T TDKGetOrAddComponent<T>(this Transform target) where T : Component
        {
            if (target == null) { return null; }
            T temp = target.GetComponent<T>();
            if (temp == null)
            {
                temp = target.AddComponent<T>();
            }
            return temp;
        }

        public static List<T> GetTsInChild<T>(this Transform target)
        {
            List<T> values = new List<T>();
            for (int i = 0; i < target.childCount; i++)
            {
                T temp = target.GetChild(i).GetComponent<T>();
                if (temp != null)
                    values.Add(temp);
            }

            return values;
        }

        #region �����Ӽ������ĺ���������
        /// <summary>
        ///  �����Ӽ�����������Ȼ�Ӽ��е�����ṹ����һģһ����
        /// </summary>
        /// <param name="source">Դͷ</param>
        /// <param name="count">������</param>
        /// <param name="prefebs">��������ʱ��Ԥ����</param>
        /// <param name="deleteUnless">�����������Ļ���ɾ���������</param>
        /// <returns></returns>
        public static List<Transform> SetChildCount(this Transform source, int count, GameObject prefebs, bool deleteUnless = false)
        {
            List<Transform> transforms = new List<Transform>();
            int childCount = source.childCount;
            int offset = count - childCount;

            if (prefebs == null)
            {
                if (childCount >= 1)
                {
                    prefebs = source.GetChild(0).gameObject;
                }
                else
                {
                    if (offset > 0)
                    {
                        Debug.LogError($"{source.gameObject.name} ��û���κ�һ���Ӽ���������ͼ�����Ӽ�ʱû�д���Ԥ����");
                        return null;
                    }

                }
            }

            if (offset > 0)
            {
                for (int i = 0; i < offset; i++)
                {
                    GameObject temp = GameObject.Instantiate(prefebs, source);
                }
            }

            for (int i = 0; i < count; i++)
            {
                Transform temp = source.GetChild(i);
                transforms.Add(temp);
                temp.gameObject.SetActive(true);
            }
            for (int i = source.childCount - 1; i >= count; i--)
            {

                Transform temp = source.GetChild(i);
                temp.gameObject.SetActive(false);
                if (deleteUnless)
                {
                    if (!Application.isPlaying)
                    {
                        GameObject.DestroyImmediate(temp.gameObject);
                    }
                    else
                    {
                        GameObject.Destroy(temp.gameObject);

                    }

                }

            }

            return transforms;
        }
        public static List<Transform> SetChildCount(this Transform source, int count, bool deleteUnless = false)
        {
            return SetChildCount(source, count, null, deleteUnless);
        }
        public static List<GameObject> SetChildCount(this GameObject source, int count, GameObject prefebs, bool deleteUnless = false)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            foreach (var i in SetChildCount(source.transform, count, prefebs, deleteUnless))
            {
                gameObjects.Add(i.gameObject);

            }

            return gameObjects;
        }
        public static List<GameObject> SetChildCount(this GameObject source, int count, bool deleteUnless = false)
        {
            return SetChildCount(source, count, null, deleteUnless);

        }
        public static List<T> SetChildCount<T>(this Transform source, int count, GameObject prefebs, bool deleteUnless = false) where T : Component
        {
            List<T> values = new List<T>();
            foreach (var i in SetChildCount(source, count, prefebs, deleteUnless))
            {
                T temp = i.GetComponent<T>();
                if (temp == null)
                {
                    Debug.LogError($"{source.name} �µġ�{i.gameObject.name} :{i.GetInstanceID()} ���в�û�д������{typeof(T).Name}");
                }
                else
                {
                    values.Add(temp);
                }

            }
            return values;
        }
        public static List<T> SetChildCount<T>(this GameObject source, int count, GameObject prefebs, bool deleteUnless = false) where T : Component
        {
            return SetChildCount<T>(source.transform, count, prefebs, deleteUnless);
        }
        public static List<T> SetChildCount<T>(this Transform source, int count, bool deleteUnless = false) where T : Component
        {

            return SetChildCount<T>(source, count, null, deleteUnless);
        }
        public static List<T> SetChildCount<T>(this GameObject source, int count, bool deleteUnless = false) where T : Component
        {

            return SetChildCount<T>(source.transform, count, deleteUnless);
        }
        public static void DeletAllChid(this Transform source)
        {
            SetChildCount(source, 0, true);
        }
        public static void DeletAllChid(this GameObject source)
        {
            source.transform.DeletAllChid();
        }
        public static void StretchFull(this RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
        #endregion


    }



}

