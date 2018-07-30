using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour , IEventListener
{

    void Awake()
    {
        NotificationManager.Instance.AddEvent(this);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void HandleEvent(NotificationType aEventType)
    {
        if (aEventType == NotificationType.playerIsDied)
        {
            LoadMenu();
        }
    }

    void OnDestroy()
    {
        NotificationManager.Instance.RemoveEvent(this);
    }

}
