using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script
{
    public class BaffsManager : MonoBehaviour, IEventListener
    {
        public RectTransform bootsSpeedTr;

        void Awake()
        {
            //подписаться на рассылку событий
            //как нотификейшн манагер узнает о том, что бафс должен получить месагу?
            NotificationManager.Instance.AddEvent(this);
        }

        public void HandleEvent(NotificationType aEventType)
        {
            if (aEventType == NotificationType.levelIsGenerated)
            {
                //do stuff
                SetPosition();
            }
        }

        void SetPosition()
        {
            Cell tempCell = LevelManager.Instance.ClosestCell(bootsSpeedTr.position);
            bootsSpeedTr.position = tempCell.tr.position;
        }

        void OnDestroy()
        {
            NotificationManager.Instance.RemoveEvent(this);
        }
    }
}
