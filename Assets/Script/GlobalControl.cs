using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalControl : MonoBehaviour {

    
    public int maxScoreGCValue = 0;
    public float healthUnit = 0;


    public static GlobalControl Instance { get { return _instance; } }
    private static GlobalControl _instance;


    // make transitions for the Health points and score points

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    //void OnDestroy()
    //{
    //    int a = 0;
    //    a++;
    //    Debug.Log("GlobalControl Error: " + a);
    //}

    void Start () {
		
	}
	
	
	void Update () {
		
	}

    public void Reset()
    {
        maxScoreGCValue = 0;
        healthUnit = 0;
    }
}
