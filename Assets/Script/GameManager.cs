using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour {

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load next level
        Debug.Log(" level Won!");
    }

    public void PlayerDead()
    {
        gameOver.SetActive(true);
        Debug.Log(" Sorry, you died! R.I.P. ");
    }
}
