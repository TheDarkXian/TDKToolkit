using TDKToolkit;
using UnityEngine;

namespace TDKToolkit
{
    public class PoolableObject : MonoBehaviour, IEPoolBeahavior
    {
        GameObject IEPoolBeahavior.gameObject { get; set; }
        PoolBase IEPoolBeahavior.pool { get; set; }
        int IEPoolBeahavior.poolBeahaviorID { get; set; }
        public virtual void OnIniFromPool()
        {

        }
        public virtual void ReturnPool()
        {
            IEPoolBeahavior temp = this;
            temp.pool?.ReturnPool(this);
        }

    }
}

