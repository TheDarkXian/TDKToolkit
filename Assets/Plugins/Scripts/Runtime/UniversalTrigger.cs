using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;


public class UniversalTrigger : MonoBehaviour
{
    Collider thisCollider;
    private void OnValidate()
    {

        if (thisCollider == null)
        {
            thisCollider = transform.GetComponent<BoxCollider>();

        }

        if (thisCollider == null)
        {
            thisCollider = transform.AddComponent<BoxCollider>();

        }

        thisCollider.isTrigger = true;
    }
    public enum ʶ��ģʽ
    {
        ����ʶ��,
        ����ʶ��,
    }
    [EnumToggleButtons]
    public ʶ��ģʽ regtionModle;

    [ShowIf("regtionModle", ʶ��ģʽ.����ʶ��)]
    [ListDrawerSettings(CustomAddFunction = "CustomAdd")]
    public List<Transform> targetList;
    [ShowIf("regtionModle", ʶ��ģʽ.����ʶ��)]
    public List<string> strList;
    void CustomAdd()
    {
        Transform transform = null;
        targetList.Add(transform);
    }
    public UnityEvent InvokeOnTargetEnter;
    public UnityEvent InvokeOnTargetExit;
    public bool isgetTarget(GameObject target)
    {
        bool isTarget = false;
        switch (regtionModle)
        {
            case ʶ��ģʽ.����ʶ��:
                {
                    int count = targetList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (target == targetList[i])
                        {
                            isTarget = true;
                        }
                    }

                    break;
                }
            case ʶ��ģʽ.����ʶ��:
                {
                    int count = strList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (target.name == strList[i])
                        {
                            isTarget = true;
                        }
                    }
                    break;
                }
        }
        return isTarget;
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (isgetTarget(target))
        {
            InvokeOnTargetEnter?.Invoke();
        }

    }
    public void OnTriggerExit(Collider other)
    {
        GameObject target = other.gameObject;
        if (isgetTarget(target))
        {
            InvokeOnTargetExit?.Invoke();
        }
    }


}
