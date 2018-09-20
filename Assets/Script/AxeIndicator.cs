using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AxeIndicator : MonoBehaviour, IEventListener {
   
    public Player player;
    public Image skillIcon;
    

    void Awake()
    {
        NotificationManager.Instance.AddEvent(this);
    }

    private void Update()
    {
        skillIcon.fillAmount = player.CurWeaponCD / player.WeaponCD;
    }

    public void HandleEvent(NotificationType aEventType)
    {
        if (aEventType == NotificationType.levelIsCompleted || aEventType == NotificationType.playerIsDied)
        {
            gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        NotificationManager.Instance.RemoveEvent(this);
    }



}
