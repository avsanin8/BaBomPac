using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnEnemy : MonoBehaviour {

    public RectTransform[] spawnPoints;
    public GameObject enemy;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            for (int i = 0; i < 4; i++)
            {
                Spawning(enemy);
            }
        }
        else
            Spawning(enemy);            
    }

    void Spawning(GameObject _enemy)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.Log("Set spawnPoints! spawnPoints.Length == 0 ");
        }
        Transform spTr = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, spTr);
    }
}
