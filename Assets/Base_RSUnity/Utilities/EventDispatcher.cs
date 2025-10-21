using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Wigi.Utilities
{
    /**
	* - description:
	* this is a base class for using Observer Pattern, receive and forward information.
	*/

    public enum DispatchMode
    {
        Default = 1,
        Immediate = 1,
        Queued = 2
    }

    /// <summary>
    /// Class Event Dispatcher for process event with UI thread and multi thread
    /// Key is string, data is object
    /// </summary>
    public class EventModeDispatcher : Singleton<EventModeDispatcher>
    {
        struct Event
        {
            public string key;
            public object data;
            public Event(string key, object data)
            {
                this.key = key;
                this.data = data;
            }
        }

        private static Dictionary<string, Action<object>> listeners = new Dictionary<string, Action<object>>();
        private List<Event> eventQueue = new List<Event>();

        public static void AddListener(string eventID, Action<object> callback)
        {
            if (listeners.ContainsKey(eventID))
            {
                if(CheckDelegateAdded(listeners[eventID], callback))
                {
                    listeners[eventID] += callback;
                } 
            }
            else
            {
                listeners.Add(eventID, null);
                listeners[eventID] += callback;
            }
        }
        public static bool CheckDelegateAdded(Action<object> delegateCheck, Action<object> callback)
        {
            if (delegateCheck == null)
                return true;
            var listDelegate = delegateCheck.GetInvocationList();
            if (listDelegate != null)
            {
                foreach (var existingHandler in delegateCheck.GetInvocationList())
                {
                    if (existingHandler.Equals(callback))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static void Dispatch(string eventID, object data = null, DispatchMode mode = DispatchMode.Immediate)
        {
            
            switch (mode)
            {
                case DispatchMode.Queued:
                    DispatchQueue(eventID, data);
                    break;
                default:
                    try
                    {
                        DispatchImmediate(eventID, data);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                    break;
            }
        }
        /// <summary>
        /// Call to process Immediate event
        /// Warning: careful with logic in multi thread
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="data"></param>
        public static void DispatchImmediate(string eventID, object data = null)
        {
            if (!listeners.ContainsKey(eventID))
                return;

            var callback = listeners[eventID];
            if (callback != null)
            {
                //check callback is valid, target of callback is enabled
                foreach (var handler in callback.GetInvocationList())
                {
                    //check target listener is enable
                    //todo: add feature remove listener
                    if (handler.Target.GetType() == typeof(MonoBehaviour)
                        && ((MonoBehaviour)handler.Target).isActiveAndEnabled)
                    {
                        handler.DynamicInvoke(data);
                    }
                }
                callback(data);
            }
            else
            {
                listeners.Remove(eventID);
            }
        }
        /// <summary>
        /// Call to process event in UI thread.
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="data"></param>
        public static void DispatchQueue(string eventID, object data = null)
        {
            Instance.eventQueue.Add(new Event(eventID, data));
        }

        private void Update()
        {
            if(eventQueue.Count > 0)
            {
                try
                {
                    foreach (var evt in eventQueue)
                    {
                        DispatchImmediate(evt.key, evt.data);
                    }
                }catch(Exception e)
                {
                    Debug.LogException(e);
                }
                eventQueue.Clear();
            }
        }

        public static void RemoveListener(string eventID, Action<object> callback)
        {
            if (listeners.ContainsKey(eventID))
                listeners[eventID] -= callback;
        }
        public static void ClearAllListener()
        {
            listeners.Clear();
        }
    }
}
