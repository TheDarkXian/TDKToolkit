#if TDKToolkit
#else
#define TDKToolkit
#endif
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LowLevel;

namespace TDKToolkit.LowLevel
{

    public class TDKToolkitLowLevel : Singleton<TDKToolkitLowLevel>
    {
        List<Sanlock> rejestSanlockList;
        Action invoekeOnUpdate;
        public TDKToolkitLowLevel()
        {
            rejestSanlockList = new List<Sanlock>();
        }
        public static Sanlock DelayInvoke(float time, Action method)
        {
            Sanlock sanlock = new Sanlock(time);
            sanlock.invokeOnEnd = method;
            sanlock.StartSandlock();
            return sanlock;
        }
        public static void JoinSandLockList(Sanlock sanlock)
        {
            if (Instance.rejestSanlockList.Contains(sanlock))
            {
                return;
            }
            else
            {
                Instance.rejestSanlockList.Add(sanlock);
                Instance.invoekeOnUpdate += sanlock.Update;
            }


        }

        public static void LeveaSandLockList(Sanlock sanlock)
        {
            if (Instance.rejestSanlockList.Contains(sanlock))
            {

                Instance.invoekeOnUpdate -= sanlock.Update;
                Instance.rejestSanlockList.Remove(sanlock);
                sanlock = null;
            }

        }
        internal void Update()
        {
            if (rejestSanlockList.Count != 0)
            {
                invoekeOnUpdate?.Invoke();
            }

        }
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            var updateLoopIndex = System.Array.FindIndex(playerLoop.subSystemList, s => s.type == typeof(UnityEngine.PlayerLoop.Update));

            if (updateLoopIndex >= 0)
            {
                var updateSystem = playerLoop.subSystemList[updateLoopIndex];
                System.Array.Resize(ref updateSystem.subSystemList, updateSystem.subSystemList.Length + 1);
                updateSystem.subSystemList[updateSystem.subSystemList.Length - 1] = new PlayerLoopSystem
                {
                    type = typeof(TDKToolkitLowLevel),
                    updateDelegate = () => TDKToolkitLowLevel.Instance.Update()
                };
                playerLoop.subSystemList[updateLoopIndex] = updateSystem;
            }

            PlayerLoop.SetPlayerLoop(playerLoop);

            // ��ӳ����˳�ʱ���¼�������
            Application.quitting += OnApplicationQuit;
        }
        static void OnApplicationQuit()
        {
            // ��������ӳ����˳�ʱ���������
            if (Instance != null)
            {
                Instance.Cleanup();
            }

        }
        // ���һ��������
        public void Cleanup()
        {
            // ��������������Ƴ����е� Sanlock
            foreach (var sanlock in rejestSanlockList)
            {
                invoekeOnUpdate -= sanlock.Update;
            }
            rejestSanlockList.Clear();
        }



    }




}



