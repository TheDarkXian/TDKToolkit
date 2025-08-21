using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace TDKToolkit
{

    public class CameraControl : SingletonMono<CameraControl>
    {
        public CameraMovement cameraMovement;


        public void FocusOn(Transform target, float foucsTime = 0.5f)
        {

            if (cameraMovement != null)
            {
                cameraMovement.CameraFocusOn(target, foucsTime);
            }

        }



    }


}
