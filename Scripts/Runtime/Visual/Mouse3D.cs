using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

namespace TDKToolkit
{
    public class Mouse3DEventData
    {
        public Vector3 mousePosistion;
        public Quaternion mouseRotation;
    }
    public class Mouse3D : SingletonMono<Mouse3D>
    {
        public GameObject visableCursorModlePrefes;
        public Mouse3DEventData data;
        public UnityEvent<Mouse3DEventData> invokeOnMouse3DAction;
        private void OnValidate()
        {
            if (visableCursorModlePrefes == null)
            {
                visableCursorModlePrefes = TDKResourcesLoad.LoadAPrefebs("Mouse3DModle");
            }
        }
        private void SpawnModelPrefes()
        {

            Render = GameObject.Instantiate(visableCursorModlePrefes, TDKHierarchy.GameNode.transform).transform;
            Render.name = visableCursorModlePrefes.name;
            Render.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            foreach (var item in Render.GetComponentsInChildren<MeshRenderer>())
            {
                item.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }


        }
        private void Awake()
        {
            OnValidate();
            data = new Mouse3DEventData();
            SpawnModelPrefes();
        }
        public Transform Render;
        private void Update()
        {
            if (Render == null) { return; }

            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePos = Vector3.zero;
            if (Physics.Raycast(ray, out var hitInfo))
            {
                mousePos = hitInfo.point;
                data.mousePosistion = mousePos;
                invokeOnMouse3DAction.Invoke(data);
            }
            Render.position = mousePos;
            Render.gameObject.SetActive(enableThemouse3D);


        }

        public bool enableThemouse3D = false;



    }


}
