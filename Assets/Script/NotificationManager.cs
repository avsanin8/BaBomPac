using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script
{
    public interface IEventListener
    {
        void HandleEvent(NotificationType eventType);
    }

    public enum NotificationType
    {
        levelIsGenerated = 0,
        playerIsDied = 1
    }

    public class NotificationManager
    {
        public NotificationManager() { }

        private static NotificationManager _instance;
        public static NotificationManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NotificationManager();

                return _instance;
            }
        }      

        public List<IEventListener> eventListeners = new List<IEventListener>();

        public void AddEvent(IEventListener aEventListener)
        {
            eventListeners.Add(aEventListener);
        }

        public void RemoveEvent(IEventListener aEventListener)
        {
            eventListeners.Remove(aEventListener);
        }

        public void PostEvent(NotificationType aTypeEvent)
        {
            foreach (IEventListener listener in eventListeners.ToList())
            {
                listener.HandleEvent(aTypeEvent);
            }
        }

    }
}
