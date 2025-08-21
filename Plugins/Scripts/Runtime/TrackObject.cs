using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TDKToolkit
{


    /// <summary>
    /// 这是一个跟踪脚本
    /// </summary>
    [ExecuteAlways]
    public class TrackObject : MonoBehaviour
    {
        [SerializeField] bool runInEditor = false;
        //启用对象    
        [SerializeField] Transform _agent;
        public Transform Agent
        {

            get
            {
                _agent = _agent ?? this.transform;
                return _agent;
            }
        }

        //启用跟踪位置
        [FoldoutGroup("位置跟踪设置")]
        public bool trackPos = true;
        //跟踪偏移
        [FoldoutGroup("位置跟踪设置")]
        [ShowIf("trackPos")]
        public offseter positionOffset = offseter.one;
        //跟踪目标
        [FoldoutGroup("位置跟踪设置")]
        [ShowIf("trackPos")]
        public Transform posTarget;

        [FoldoutGroup("旋转设置")]
        public RoateTrackMode roateMode;
        [FoldoutGroup("旋转设置")]
        [HideIf("roateMode", RoateTrackMode.None)]
        public offseter roateOffset = offseter.one;
        [FoldoutGroup("旋转设置")]
        [ShowIf("roateMode", RoateTrackMode.FaceTarget)]
        public bool followMainCamera = false;
        [FoldoutGroup("旋转设置")]
        [ShowIf("roateMode", RoateTrackMode.SameWithTarget)]
        public Transform roateTarget;
        [FoldoutGroup("旋转设置")]
        [ShowIf("@roateMode== RoateTrackMode.FaceTarget&&!followMainCamera")]
        public Transform faceTarget;

        private void Update()
        {

            if (!runInEditor && !Application.isPlaying) { return; }
            HandlePos();
            HandleRoate();

        }

        void HandlePos()
        {

            if (trackPos)
            {
                if (posTarget != null)
                {
                    Agent.position = posTarget.position;
                }
            }

        }

        void HandleRoate()
        {
            switch (roateMode)
            {
                case RoateTrackMode.None:
                    {

                        break;
                    }
                case RoateTrackMode.FaceTarget:
                    {
                        Transform targetTr = faceTarget;
                        if (followMainCamera)
                        {
                            targetTr = Camera.main.transform;
                        }
                        if (targetTr == null) { break; }

                        Vector3 dir = targetTr.position - Agent.position;
                        if (roateOffset.offsetValue == Vector3.zero) { return; }
                        dir *= roateOffset;
                        dir.Normalize();

                        Vector3 targetEulerAngles = Quaternion.LookRotation(dir).eulerAngles;

                        Vector3 newEulerAngles = new Vector3(
                            roateOffset.x ? targetEulerAngles.x : Agent.eulerAngles.x,
                            roateOffset.y ? targetEulerAngles.y : Agent.eulerAngles.y,
                            roateOffset.z ? targetEulerAngles.z : Agent.eulerAngles.z
                        );

                        Agent.eulerAngles = newEulerAngles;
                        break;
                    }

                case RoateTrackMode.SameWithTarget:
                    {
                        if (roateTarget == null) { break; }
                        Agent.eulerAngles = roateOffset + roateTarget.eulerAngles;
                        break;
                    }

            }

        }
        [System.Serializable]
        public struct offseter
        {
            [HorizontalGroup]
            public bool x;
            [HorizontalGroup]
            public bool y;

            [HorizontalGroup] public bool z;
            [SerializeField]
            Vector3 offsetvalue;
            public Vector3 offsetValue
            {
                get
                {
                    return new Vector3(
                        x ? offsetvalue.x : 0,
                        y ? offsetvalue.y : 0,
                        z ? offsetvalue.z : 0
                    );
                }
                set
                {
                    offsetvalue = value;
                }
            }

            // 重载加法操作符
            public static Vector3 operator +(Vector3 v, offseter o)
            {
                return new Vector3(
                    v.x + (o.x ? o.offsetvalue.x : 0),
                    v.y + (o.y ? o.offsetvalue.y : 0),
                    v.z + (o.z ? o.offsetvalue.z : 0)
                );
            }

            public static Vector3 operator +(offseter o, Vector3 v)
            {
                return v + o; // 利用上面的重载
            }

            public static Vector3 operator *(offseter o, Vector3 v)
            {

                return new Vector3(
                o.offsetvalue.x * v.x,
                   o.offsetvalue.y * v.y,
                      o.offsetvalue.z * v.z
                );
            }
            public static Vector3 operator *(Vector3 v, offseter o)
            {

                return o * v;
            }

            public static Vector3 operator *(offseter o, float v)
            {

                return new Vector3(
                o.offsetvalue.x * v,
                   o.offsetvalue.y * v,
                      o.offsetvalue.z * v
                );
            }
            public static Vector3 operator *(float v, offseter o)
            {

                return o * v;
            }
            public static offseter one
            {

                get
                {
                    offseter temp = new offseter();
                    temp.offsetValue = Vector3.one;
                    temp.x = true;
                    temp.y = true;
                    temp.z = true;

                    return temp;


                }
            }
            public static offseter nOne
            {

                get
                {
                    offseter temp = new offseter();
                    temp.offsetValue = Vector3.one * -1;
                    temp.x = true;
                    temp.y = true;
                    temp.z = true;

                    return temp;


                }
            }

        }


        public enum RoateTrackMode
        {
            None,
            SameWithTarget,
            FaceTarget
        }
    }
}