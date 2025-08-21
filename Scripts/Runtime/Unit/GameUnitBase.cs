using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TDKToolkit
{

    public class GameUnitBase : MonoBehaviour, IPointerClickHandler
    {
        public static GameUnitBase FoucosUnit;
        [FoldoutGroup("基类设置")]
        [Header("双击时聚焦到此物体")]
        public bool focusOn;
        [FoldoutGroup("基类设置")]
        [Header("双击时聚焦到此物体")]
        [ReadOnly]
        public bool isfocusthisyet;
        [FoldoutGroup("基类设置")]
        [Header("单击时高亮/取消高亮")]
        public bool clickOnceHighLight;
        [FoldoutGroup("基类设置")]
        [Header("高亮设置")]
        public Outline _outline;
        public Outline outline
        {

            get
            {
                if (_outline == null)
                {
                    _outline = GetComponent<Outline>();
                }
                if (_outline == null)
                {
                    Debug.LogError("没有设置高亮组件");
                }
                return _outline;
            }
        }


        public void SetHighLight(bool var)
        {
            if (outline != null)
            {
                outline.enabled = var;

            }

        }
        public void FlotSetHighLight()
        {
            if (outline != null && clickOnceHighLight)
            {
                SetHighLight(!outline.enabled);

            }

        }
        public virtual void FocusOnthis()
        {
            if (FoucosUnit != null)
            {
                FoucosUnit.isfocusthisyet = false;
            }
            FoucosUnit = this;
            isfocusthisyet = true;
            CameraControl.Instance.FocusOn(this.transform);
        }
        public virtual void OnClickOne()
        {
            FlotSetHighLight();
        }
        public virtual void OnClickTwo()
        {
            if (focusOn)
            {
                FocusOnthis();
            }
        }
        public virtual void OnClickMulti()
        {

        }
        public void OnPointerClick(PointerEventData eventData)
        {
            int count = eventData.clickCount;
            if (count == 1)
            {
                OnClickOne();
            }
            else if (count == 2)
            {
                OnClickTwo();
            }
            else if (count >= 3)
            {
                OnClickMulti();
            }



        }


    }


}

