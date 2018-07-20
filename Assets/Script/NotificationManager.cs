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
        playerIsDied = 1,
        timerOn = 2
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

        public void AddEventListener(IEventListener aEventListener)
        {
            eventListeners.Add(aEventListener);
        }

        public void RemoveEventListener(IEventListener aEventListener)
        {
            eventListeners.Remove(aEventListener);
        }

        public void PostEventListener(NotificationType aTypeEvent)
        {
            foreach (IEventListener listener in eventListeners)
            {
                listener.HandleEvent(aTypeEvent);
            }
        }

    }
}
