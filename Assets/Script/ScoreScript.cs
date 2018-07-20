using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

    public static int scoreValue = 0;
    public static int maxScoreValue = 0;

    Text score;
    public Text maxScore;


    // Use this for initialization
    void Start () {
        score = GetComponent<Text>();
        maxScoreValue = GlobalControl.Instance.maxScoreGCValue;            
	}
	
	// Update is called once per frame
	void Update () {
        score.text = "Score: " + scoreValue;        
        maxScore.text = "MAX Score: " + maxScoreValue;
    }

    //Save score in GlobalControl
    void OnDestroy()
    {        
        GlobalControl.Instance.maxScoreGCValue = maxScoreValue;
    }
}
