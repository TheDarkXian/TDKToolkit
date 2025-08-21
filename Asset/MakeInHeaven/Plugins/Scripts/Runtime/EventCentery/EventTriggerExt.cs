using System;
using UnityEngine;
namespace TDKToolkit
{


    /// <summary>
    /// ���ڴ����¼�����չ��
    /// </summary>
    public static class EventTriggerExt
    {
        /// <summary>
        /// �����¼����޲�����
        /// </summary>
        /// <param name="sender">����Դ</param>
        /// <param name="eventName">�¼���</param>
        public static void TriggerEvent(this object sender, string eventName)
        {
            TDKEventCentery.Instance.TriggerEvent(eventName, sender);
        }
        /// <summary>
        /// �����¼����в�����
        /// </summary>
        /// <param name="sender">����Դ</param>
        /// <param name="eventName">�¼���</param>
        /// <param name="args">�¼�����</param>
        public static void TriggerEvent(this object sender, string eventName, EventArgs args)
        {
            TDKEventCentery.Instance.TriggerEvent(eventName, sender, args);
        }

    }

}

