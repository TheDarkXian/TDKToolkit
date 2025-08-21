using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MutiplySelectOne : MonoBehaviour
{
    [ListDrawerSettings(CustomAddFunction = "addListFunc")]
    [SerializeField]
    List<Transform> _transforms;
    public List<Transform> transforms
    {
        get { return _transforms; }

    }
    [ShowIf("activeOBJ")]
    public Transform activeOBJ;
    public UnityEvent invokeOnTrun;
    [ValueDropdown("listname")]
    public string showtargetName;
    List<string> listname
    {
        get
        {
            List<string> list = new List<string>();
            foreach (Transform t in transforms)
            {
                if (t != null)
                {
                    list.Add(t.name);
                }
            }
            return list;
        }
    }
    public void AddGameObj(Transform target)
    {
        transforms.Add(target);
    }
    void addListFunc()
    {
        transforms.Add(null);
    }
    [Button("չʾ����")]
    public void ShowOne()
    {
        ShowTarget(showtargetName);

    }
    [Button("���ж���")]
    public void ShowAll()
    {
        foreach (Transform t in transforms)
        {
            if (t != null)
            {
                t.gameObject.SetActive(true);
            }

        }
    }
    public void ShowTarget(string objname)
    {

        bool istrue = false;
        foreach (Transform t in transforms)
        {
            if (t == null)
            {
                continue;
            }
            if (t.gameObject.name == objname)
            {
                invokeOnTrun?.Invoke();
                activeOBJ = t;
                t.gameObject.SetActive(true);
                istrue = true;
            }
            else
            {
                t.gameObject.SetActive(false);
            }

        }
        if (!istrue)
        {
            activeOBJ = null;
            Debug.Log(gameObject.name + "��ѡһ��" + objname + "û���������");
        }

    }
    public void ShowTarget(Transform target)
    {
        bool istrue = false;
        foreach (Transform t in transforms)
        {
            if (t == null)
            {
                continue;
            }
            if (t == target)
            {
                t.gameObject.SetActive(true);
                activeOBJ = target;
                istrue = true;
                invokeOnTrun?.Invoke();
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }
        if (!istrue)
        {
            activeOBJ = null;
            Debug.Log($"{gameObject.name}����û���������{target.name}");
        }
    }

    public void AddGameObj(GameObject gameObject)
    {
        AddGameObj(gameObject.transform);
    }
}
