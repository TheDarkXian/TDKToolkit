using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TDKToolkit.LowLevel;


namespace TDKToolkit
{

    public class Sanlock
    {
        float _preTime;
        float _nowTime;
        float _sanlockTime;
        public float CountTime
        {

            get { return _nowTime - _preTime; }
        }
        bool _continuesOnEnd;
        bool _enable;
        bool _isPause;
        public bool IsStop { get => _isPause; }
        public bool Enable { get => _enable; }

        public Action invokeOnEnd;
        public Action invokeOnStart;
        public Action<float> invokeOnTick;
        public Func<float> GetTimeDottime;
        public float SanlockTime
        {
            get { return _sanlockTime; }
        }
        public Sanlock(float time, bool continusOnEnd = false, Func<float> customTime = null)
        {
            if (customTime != null)
            {
                GetTimeDottime = customTime;
            }
            else
            {
                GetTimeDottime = GetTime;
            }
            _preTime = GetTimeDottime();
            _sanlockTime = time;
            _continuesOnEnd = continusOnEnd;
        }
        float GetTime()
        {
            return Time.time;
        }
        public void StartSandlock()
        {
            _preTime = GetTimeDottime();
            _isPause = false;
            _enable = true;
            invokeOnStart?.Invoke();
            TDKToolkitLowLevel.JoinSandLockList(this);
        }
        public void PauseSandLock()
        {
            _isPause = true;

        }
        public void StopSandLock()
        {
            if (_enable)
            {
                TDKToolkitLowLevel.LeveaSandLockList(this);

            }
            _enable = false;

        }
        public void Continues()
        {
            float temp = CountTime;
            _preTime = GetTimeDottime() - temp;
            _isPause = false;
        }
        public void SetSandLock(float time, bool continusOnEnd = true)
        {
            _sanlockTime = time;
            _continuesOnEnd = continusOnEnd;
        }
        public float GetProcess()
        {
            return CountTime / SanlockTime;

        }
        internal void Update()
        {

            if (_enable)
            {
                if (_isPause) { return; }
                _nowTime = GetTimeDottime();
                invokeOnTick?.Invoke(GetProcess());
                if (CountTime >= _sanlockTime)
                {
                    invokeOnEnd?.Invoke();
                    StopSandLock();
                    if (_continuesOnEnd)
                    {
                        StartSandlock();
                    }
                }

            }

        }



    }

}

