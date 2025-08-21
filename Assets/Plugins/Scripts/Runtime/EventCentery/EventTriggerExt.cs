using System;
using UnityEngine;
namespace TDKToolkit
{


    /// <summary>
    /// 便于触发事件的扩展类
    /// </summary>
    public static class EventTriggerExt
    {
        /// <summary>
        /// 触发事件（无参数）
        /// </summary>
        /// <param name="sender">触发源</param>
        /// <param name="eventName">事件名</param>
        public static void TriggerEvent(this object sender, string eventName)
        {
            TDKEventCentery.Instance.TriggerEvent(eventName, sender);
        }
        /// <summary>
        /// 触发事件（有参数）
        /// </summary>
        /// <param name="sender">触发源</param>
        /// <param name="eventName">事件名</param>
        /// <param name="args">事件参数</param>
        public static void TriggerEvent(this object sender, string eventName, EventArgs args)
        {
            TDKEventCentery.Instance.TriggerEvent(eventName, sender, args);
        }

    }

}

