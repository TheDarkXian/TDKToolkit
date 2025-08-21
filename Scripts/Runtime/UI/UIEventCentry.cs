using System.Collections.Generic;
using System;
using UnityEngine;
namespace TDKToolkit
{
    public class UIEventCentry : Singleton<UIEventCentry>
    {
        private Dictionary<string, Action<object>> handlerDic = new Dictionary<string, Action<object>>();
        private Dictionary<string, Action> handlerDicnoparg = new Dictionary<string, Action>();
        /// <summary>
        /// 添加一个事件的监听者
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">事件处理函数</param>
        public static void AddListener(string eventName, Action<object> handler)
        {
            Dictionary<string, Action<object>> handlerDic = Instance.handlerDic;
            if (handlerDic.ContainsKey(eventName))
                handlerDic[eventName] += handler;
            else
                handlerDic.Add(eventName, handler);
        }

        public static void AddListener(string eventName, Action handler)
        {
            Dictionary<string, Action> handlerDic = Instance.handlerDicnoparg;
            if (handlerDic.ContainsKey(eventName))
                handlerDic[eventName] += handler;
            else
                handlerDic.Add(eventName, handler);
        }
        /// <summary>
        /// 移除一个事件的监听者
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">事件处理函数</param>
        public static void RemoveListener(string eventName, Action<object> handler)
        {
            Dictionary<string, Action<object>> handlerDic = Instance.handlerDic;

            if (handlerDic.ContainsKey(eventName))
                handlerDic[eventName] -= handler;
        }
        public static void RemoveListener(string eventName, Action handler)
        {
            Dictionary<string, Action> handlerDic = Instance.handlerDicnoparg;

            if (handlerDic.ContainsKey(eventName))
                handlerDic[eventName] -= handler;
        }
        /// <summary>
        /// 触发事件（无参数）
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="sender">触发源</param>
        public static void TriggerEvent(string eventName, object sender)
        {
            Dictionary<string, Action<object>> handlerDic = Instance.handlerDic;

            if (handlerDic.ContainsKey(eventName))
                handlerDic[eventName]?.Invoke(null);
        }
        /// <summary>
        /// 触发事件（有参数）
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="sender">触发源</param>
        /// <param name="args">事件参数</param>
        public static void TriggerEvent(string eventName, object sender, object args)
        {
            Dictionary<string, Action<object>> handlerDic = Instance.handlerDic;

            if (handlerDic.ContainsKey(eventName))
                handlerDic[eventName]?.Invoke(args);
        }
        /// <summary>
        /// 清空所有事件
        /// </summary>
        public void Clear()
        {
            handlerDic.Clear();
        }
    }

    public static class UIEventCentryExt
    {


        /// <summary>
        /// 触发事件（无参数）
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="sender">触发源</param>
        public static void TriggerEvent(this object sender, string eventName)
        {
            UIEventCentry.TriggerEvent(eventName, sender);
        }
        /// <summary>
        /// 触发事件（有参数）
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="sender">触发源</param>
        /// <param name="args">事件参数</param>
        public static void TriggerEvent(this object sender, string eventName, object args)
        {
            UIEventCentry.TriggerEvent(eventName, sender, args);
        }

    }

}
