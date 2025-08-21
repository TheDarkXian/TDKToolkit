using UnityEngine;

namespace TDKToolkit
{
    public class Singleton<T> where T : new()
    {
        static readonly object loack = new object();
        public static T Instance
        {
            get
            {
                lock (loack)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
                return _instance;
            }
        }
        private static T _instance;

    }


}

