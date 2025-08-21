using UnityEngine;
namespace TDKToolkit
{

    public class SingletonMono<T> : MonoBehaviour where T : Component, new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance != null && _instance.gameObject.scene == null)
                {
                    _instance = null;
                }
                if (_instance == null)
                {
                    _instance = GameObject.FindAnyObjectByType<T>();

                    if (_instance == null)
                    {
                        GameObject temp = new GameObject(typeof(T).Name);
                        T tempComponet = temp.AddComponent<T>();
                        _instance = tempComponet;

                    }
                }


                return _instance;
            }

        }



    }


}

