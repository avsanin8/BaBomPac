using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour, IEventListener{

    public GameObject timerUIObj;
    public Text timerTextUI;
    private float startTime;
    private bool timerTurnOn = false;

    void Awake()
    {
        NotificationManager.Instance.AddEventListener(this);
    }

    private void Update()
    {
        if (!timerTurnOn)
            return;
        timerUIObj.SetActive(true);
        float curTime = startTime - Time.time;
        string min = ((int)curTime / 60).ToString();
        string sec = (curTime % 60).ToString("f2");

        timerTextUI.text = "Time Buff left: " + min + " : " + sec;

        if (curTime <= 0)
        {
            curTime = startTime = 0;
            timerTurnOn = false;
            timerUIObj.SetActive(false);
        }
    }

    public void TurnOn(float buffTime)
    {
        startTime = buffTime;
        timerTurnOn = true;
    }

    public void HandleEvent(NotificationType aEventType)
    {
        if (aEventType == NotificationType.timerOn)
        {
            TurnOn(10);
        }
    }
}
