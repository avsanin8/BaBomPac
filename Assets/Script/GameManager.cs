using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour, IEventListener{

    bool gameIsEnded = false;
    public float restartDelay = 3f;

    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance;

    public GameObject completeLevelUI;    
    public GameObject gameOver;

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            Debug.LogError("More then one singletone of GameManager");
        }

        //подписаться на рассылку событий NotificationManager
        NotificationManager.Instance.AddEventListener(this);
    }

    public void EndGame()
    {
        if (gameIsEnded == false)
        {
            gameIsEnded = true;
            Debug.Log("GAME OVER");

            // Restart the game
            //Invoke("RestartGame", restartDelay);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene("SampleScene"); // Load scene whit name
        ScoreScript.scoreValue = 0;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Load current scene
    }

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
        //completeLevelUI.GetComponent<Animator>().Play("LevelComplete");        
        
        Debug.Log(" level Won!");
    }

    public void LoadLevel()
    {
        //ScoreScript.scoreValue = 0;
        Debug.Log("Load Next Level");
        if (SceneManager.GetActiveScene().buildIndex >= 2)
            SceneManager.LoadScene(2);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // when created next level        
    }

    public void PlayerDead()
    {
        gameOver.SetActive(true);
        Debug.Log(" Sorry, you died! R.I.P. ");
    }

    public void HandleEvent(NotificationType aEventType)
    {
        if (aEventType == NotificationType.playerIsDied)
        {
            PlayerDead();
        }
    }
}
