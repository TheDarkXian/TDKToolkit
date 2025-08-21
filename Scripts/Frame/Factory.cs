using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace TDKToolkit
{
    [RequireComponent(typeof(PoolBase))]
    public class Factory : SingletonMono<Factory>
    {
        protected virtual void OnValidate()
        {

        }
        private void Awake()
        {
            OnValidate();

        }




    }
}
