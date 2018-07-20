using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayGame()
    {
        GlobalControl.Instance.Reset();
        //SceneManager.LoadScene("SampleScene"); //Level01

        // To bild by index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        ScoreScript.scoreValue = 0;
        ScoreScript.maxScoreValue = 0;
        
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
