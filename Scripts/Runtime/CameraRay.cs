using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TDKToolkit
{
    [RequireComponent(typeof(PhysicsRaycaster))]
    public class CameraRay : MonoBehaviour
    {
        public event Action<GameObject> InvokeOnGetTarget;
        // Update is called once per frame
        void Update()
        {
            if (Camera.current)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Vector2 screenPoint = Input.mousePosition;
                    Ray ray = Camera.main.ScreenPointToRay(screenPoint);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        InvokeOnGetTarget?.Invoke(hit.collider.gameObject);
                        Debug.Log("ddddddddddddddddd");
                    }
                }
            }


        }



    }
}

