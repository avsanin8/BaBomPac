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
            NotificationManager.Instance.AddEventListener(this);
        }

        public void HandleEvent(NotificationType aEventType)
        {
            if (aEventType == NotificationType.levelIsGenerated)
            {
                //do stuff
                SetPositions();
            }
        }

        void SetPositions()
        {
            Cell tempCell = LevelManager.Instance.ClosestCell(bootsSpeedTr.position);
            bootsSpeedTr.position = tempCell.tr.position;
        }
    }
}
