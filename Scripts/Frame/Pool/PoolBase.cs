using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace TDKToolkit
{
    [System.Serializable]
    public class PrefebsSetting
    {
        [ShowIf("@loadType==PrefebsLoadType.FromScript&&prefebs!=null")]
        [ShowIf("@loadType==PrefebsLoadType.FromResoucres")]
        [ShowIf("@loadType==PrefebsLoadType.FromDrog")]
        public GameObject prefebs;

        [EnumToggleButtons]
        public PrefebsLoadType loadType;

        [ShowIf("loadType", PrefebsLoadType.FromResoucres)]
        public string prefebPath;
        [ShowIf("loadType", PrefebsLoadType.FromResoucres)]
        public bool LoadPrefeb()
        {
            bool isload;
            GameObject temp = TDKResourcesLoad.LoadAPrefebs(prefebPath);
            if (temp == null)
            {
                if (prefebPath == null)
                {
                }
                else
                {
                    Debug.LogError("错误的路径，没有在" + TDKResourcesLoad.tDKSetting.PrefebsResources + "下找到 " + prefebPath);
                }
                isload = false;
            }
            else
            {
                prefebs = temp;
                isload = true;
            }
            return isload;
        }
        public enum PrefebsLoadType
        {
            FromScript,
            FromResoucres,
            FromDrog
        }
    }
    public class PoolBase : MonoBehaviour
    {
        public Transform poolParentActive;
        public Transform poolParentDisactive;

        public GameObject poolPrefebs
        {
            get { return poolPrefebsSetting.prefebs; }
        }
        public PrefebsSetting poolPrefebsSetting;

        int poolBeahaviorNum;
        Dictionary<int, IEPoolBeahavior> objActive;
        Queue<IEPoolBeahavior> objPool;



        private void OnValidate()
        {
            poolParentDisactive = TDKToolkitPag.FindOrGetAObj("poolParentDisactive", transform).transform;

        }
        private void Awake()
        {
            OnValidate();
            IniPool();
        }
        public void IniPool()
        {
            poolBeahaviorNum = 0;
            if (!poolPrefebs)
            {
                Debug.LogError($"{this.gameObject.name} : 这个池里什么也没有啊，记得放个OBJ重新ini一下");
                return;

            }
            IEPoolBeahavior temp = poolPrefebs.GetComponent<IEPoolBeahavior>();
            if (temp == null)
            {
                Debug.LogError($"{this.gameObject.name} : 我靠你不要把没有池行为的脚本放上来啊");
                return;
            }
            objActive = new Dictionary<int, IEPoolBeahavior>();
            objPool = new Queue<IEPoolBeahavior>();

        }
        [Button("INI")]
        public void Clone()
        {
            InifromPool();

        }
        public IEPoolBeahavior InifromPool()
        {
            IEPoolBeahavior result = null;
            if (objPool.Count <= 0)
            {
                int newobjnum = poolBeahaviorNum * 2;
                newobjnum = newobjnum == 0 ? 2 : newobjnum;
                for (int i = 0; i < newobjnum; i++)
                {
                    GameObject temp = GameObject.Instantiate(poolPrefebs);
                    result = temp.GetComponent<IEPoolBeahavior>();
                    result.gameObject = temp;
                    result.gameObject.transform.parent = poolParentDisactive;
                    result.pool = this;
                    result.poolBeahaviorID = poolBeahaviorNum++;
                    result.gameObject.SetActive(false);
                    result.gameObject.name = poolPrefebs.name + poolBeahaviorNum;
                    objPool.Enqueue(result);
                }

            }
            result = objPool.Dequeue();
            result.OnIniFromPool();
            result.gameObject.SetActive(true);
            result.gameObject.transform.parent = poolParentActive;
            objActive.Add(result.poolBeahaviorID, result);
            return result;
        }

        public void ReturnPool(IEPoolBeahavior obj)
        {
            if (objActive.ContainsKey(obj.poolBeahaviorID))
            {
                objActive.Remove(obj.poolBeahaviorID);
            }
            obj.gameObject.transform.parent = poolParentDisactive;
            obj.gameObject.SetActive(false);
            objPool.Enqueue(obj);


        }

        [Button("Clear")]
        public void ClearPool()
        {

            while (objPool.Count > 0)
            {
                IEPoolBeahavior target = objPool.Dequeue();
                GameObject.Destroy(target.gameObject);
            }
            objPool.Clear();
            objPool = null;
            objActive = null;
            poolBeahaviorNum = 0;
            objActive = new Dictionary<int, IEPoolBeahavior>();
            objPool = new Queue<IEPoolBeahavior>();
            IEPoolBeahavior[] activeobjs = poolParentActive.GetComponentsInChildren<IEPoolBeahavior>();
            foreach (var i in activeobjs)
            {
                i.poolBeahaviorID = poolBeahaviorNum++;
                objActive.Add(i.poolBeahaviorID, i);
                i.gameObject.name = poolPrefebs.name + i.poolBeahaviorID;
            }


        }

    }
    public interface IEPoolBase
    {
        IEPoolBeahavior InifromPool();
        void RenturnPool(IEPoolBeahavior target);
        void ClearPool();
    }
    public interface IEPoolBeahavior
    {
        GameObject gameObject { get; set; }
        PoolBase pool
        { get; set; }
        int poolBeahaviorID { get; set; }
        void ReturnPool();

        void OnIniFromPool();


    }

}

