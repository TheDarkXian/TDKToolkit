using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface TDKIPoolObject
{
    public ObjectPool<GameObject> from { get; set; }
    public void OnGet();
    public void OnRelease();
    public void OnDestroy();

}
