using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnEnemy : MonoBehaviour, IEventListener
{

    public RectTransform[] spawnPointsTr;
    public GameObject enemy;    

    void Awake()
    {
        //подписаться на рассылку событий NotificationManager
        NotificationManager.Instance.AddEvent(this);
    }
    
    //private void Start()
    //{
    //    if (SceneManager.GetActiveScene().buildIndex == 2)
    //    {
    //        for (int i = 0; i < 4; i++)
    //        {
    //            Spawning(enemy);
    //        }
    //    }
    //    else
    //    {
    //        SetPosition();
    //        Spawning(enemy);
    //    }
    //}

    void Spawning(GameObject _enemy)
    {
        RectTransform spTr = spawnPointsTr[Random.Range(0, spawnPointsTr.Length)];
        Instantiate(_enemy, spTr);

        if (spawnPointsTr.Length == 0)
        {
            Debug.Log("Set spawnPoints! spawnPointsTr.Length == 0 ");
        }
        
    }

    void SetPosition()
    {
        for (int i = 0; i < spawnPointsTr.Length; i++)
        {
            Cell tempCell = LevelManager.Instance.ClosestCell(spawnPointsTr[i].position);
            spawnPointsTr[i].position = tempCell.tr.position;
        }
    }

    public void HandleEvent(NotificationType aEventType)
    {        
        if (aEventType == NotificationType.levelIsGenerated)
        {
            SetPosition();
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    Spawning(enemy);
                }
            }
            else
            {
                Spawning(enemy);
            }
        }
    }

    void OnDestroy()
    {
        NotificationManager.Instance.RemoveEvent(this);
    }
}
