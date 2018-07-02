using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelComplete : MonoBehaviour {

	public void LoadNextLevel()
    {
        //ScoreScript.scoreValue = 0;
        Debug.Log("Load Next Level");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // when created next level
        SceneManager.LoadScene(1);
    }
}
