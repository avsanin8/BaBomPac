using Assets.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour, IEventListener {

    public GameObject timerUIObj;
    public Text timerTextUI;

    //private float startTime;
    //public bool timerTurnOn = false;
    public Player player;


    void Awake()
    {
        NotificationManager.Instance.AddEvent(this);
    }

    private void Update()
    {
        if (!player.timerTurnOn)
            return;
        else
            TimerUpdate();
    }

    public void TimerUpdate()
    {        
        timerTextUI.text = "Time Buff left: " + player.TimerMin + " : " + player.TimerSec;

        if (player.TimeBuff <= 0)
        {            
            timerUIObj.SetActive(false);
        }
    }

    public void TurnOn()
    {
        timerUIObj.SetActive(true);        
        //timerTurnOn = true;        
        TimerUpdate();
    }

    public void HandleEvent(NotificationType aEventType)
    {
        if (aEventType == NotificationType.playerIsDied)
        {
            //player.timerTurnOn = false;
            timerUIObj.SetActive(false);
        }
    }

    void OnDestroy()
    {
        NotificationManager.Instance.RemoveEvent(this);
    }
}
