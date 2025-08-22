using System.Collections;
using System.Collections.Generic;
using TDKToolkit;
using UnityEngine;
using UnityEngine.Pool;

public class TDKPoolManger : Singleton<TDKPoolManger>
{

    Dictionary<int, ObjectPool<GameObject>> _poolDic;
    Dictionary<GameObject, int> _objBinPoolKey;
    int _poolKey;
    public void Ini()
    {
        if (_poolDic != null)
        {
            foreach (var i in _poolDic)
            {
                i.Value.Dispose();
            }
            _poolDic.Clear();
            _objBinPoolKey.Clear();
        }

        _poolDic = new Dictionary<int, ObjectPool<GameObject>>();
        _objBinPoolKey = new Dictionary<GameObject, int>();
        _poolKey = 0;
    }

    public int Register(GameObject objPrefab)
    {

        if (_objBinPoolKey.ContainsKey(objPrefab))
        {
            return _objBinPoolKey[objPrefab];
        }
        else
        {
            _objBinPoolKey.Add(objPrefab, _poolKey);
            _poolDic.Add(_poolKey,
            new ObjectPool<GameObject>(
                createFunc: () =>
                {
                    GameObject obj = GameObject.Instantiate(objPrefab);
                    obj.SetActive(false);
                    return obj;
                },
                actionOnGet: (obj) =>
                {
                    TDKIPoolObject poolobj = obj.GetComponent<TDKIPoolObject>();
                    if (poolobj != null)
                    {

                        poolobj.from = _poolDic[_objBinPoolKey[objPrefab]];
                        poolobj.OnGet();
                    }
                    obj.SetActive(true);
                },
                actionOnRelease: (obj) =>
                {
                    TDKIPoolObject tDKIPoolobject = obj.GetComponent<TDKIPoolObject>();
                    if (tDKIPoolobject != null)
                    {
                        tDKIPoolobject.OnRelease();
                        tDKIPoolobject.from.Release(obj);
                    }
                    obj.SetActive(false);
                },
                actionOnDestroy: (obj) =>
                {
                    TDKIPoolObject tDKIPoolobject = obj.GetComponent<TDKIPoolObject>();
                    if (tDKIPoolobject != null)
                    {
                        tDKIPoolobject.OnDestroy();
                    }
                    GameObject.Destroy(obj);
                },
                defaultCapacity: 10,
                maxSize: 10000
                )
            );
            _poolKey++;

            return _poolKey;
        }


    }
    public void UnRegister(GameObject objPrefab)
    {
        if (_objBinPoolKey.ContainsKey(objPrefab))
        {
            int poolid = _objBinPoolKey[objPrefab];
            _poolDic[poolid].Clear();
            _poolDic[poolid].Dispose();
            _objBinPoolKey.Remove(objPrefab);

            _poolDic.Remove(poolid);
        }
    }

    public GameObject Get(GameObject objPrefab)
    {
        GameObject temp = null;

        if (objPrefab.scene != null)
        {
            Debug.LogError("这个地方只给预制体用");
            return temp;
        }
        int objPoolId = -1;
        if (_objBinPoolKey.ContainsKey(objPrefab))
        {
            objPoolId = _objBinPoolKey[objPrefab];
        }
        else
        {
            objPoolId = Register(objPrefab);
        }

        ObjectPool<GameObject> pool = _poolDic[objPoolId];
        temp = pool.Get();


        return temp;
    }

    public void Release(GameObject obj, ObjectPool<GameObject> pool = null)
    {
        if (pool == null)
        {
            GameObject.Destroy(obj);
        }
        else
        {
            pool.Release(obj);
        }

    }

}
