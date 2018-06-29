using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour {

    bool gameIsEnded = false;
    public float restartDelay = 3f;

    public void EndGame()
    {
        if (gameIsEnded == false)
        {
            gameIsEnded = true;
            Debug.Log("GAME OVER");

            // Restart the game
            Invoke("RestartGame", restartDelay);
        }
    }

    void RestartGame()
    {
        //SceneManager.LoadScene("SampleScene"); // Load scene whit name
        ScoreScript.scoreValue = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Load current scene
    }

}
