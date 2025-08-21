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
        /// ���һ���¼��ļ�����
        /// </summary>
        /// <param name="eventName">�¼���</param>
        /// <param name="handler">�¼�������</param>
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
        /// �Ƴ�һ���¼��ļ�����
        /// </summary>
        /// <param name="eventName">�¼���</param>
        /// <param name="handler">�¼�������</param>
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
        /// �����¼����޲�����
        /// </summary>
        /// <param name="eventName">�¼���</param>
        /// <param name="sender">����Դ</param>
        public static void TriggerEvent(string eventName, object sender)
        {
            Dictionary<string, Action<object>> handlerDic = Instance.handlerDic;

            if (handlerDic.ContainsKey(eventName))
                handlerDic[eventName]?.Invoke(null);
        }
        /// <summary>
        /// �����¼����в�����
        /// </summary>
        /// <param name="eventName">�¼���</param>
        /// <param name="sender">����Դ</param>
        /// <param name="args">�¼�����</param>
        public static void TriggerEvent(string eventName, object sender, object args)
        {
            Dictionary<string, Action<object>> handlerDic = Instance.handlerDic;

            if (handlerDic.ContainsKey(eventName))
                handlerDic[eventName]?.Invoke(args);
        }
        /// <summary>
        /// ��������¼�
        /// </summary>
        public void Clear()
        {
            handlerDic.Clear();
        }
    }

    public static class UIEventCentryExt
    {


        /// <summary>
        /// �����¼����޲�����
        /// </summary>
        /// <param name="eventName">�¼���</param>
        /// <param name="sender">����Դ</param>
        public static void TriggerEvent(this object sender, string eventName)
        {
            UIEventCentry.TriggerEvent(eventName, sender);
        }
        /// <summary>
        /// �����¼����в�����
        /// </summary>
        /// <param name="eventName">�¼���</param>
        /// <param name="sender">����Դ</param>
        /// <param name="args">�¼�����</param>
        public static void TriggerEvent(this object sender, string eventName, object args)
        {
            UIEventCentry.TriggerEvent(eventName, sender, args);
        }

    }

}
