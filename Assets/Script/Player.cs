using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {
    

    void OnTriggerEnter2D(Collider2D point)
    {
        Debug.Log("OnTriggerEnter in :" + point.gameObject.name);
        if (point.gameObject.tag == "Point")
        {
            ScoreScript.scoreValue += 10;
            if (ScoreScript.scoreValue == 650)
            {
                Debug.Log("Victory");
                FindObjectOfType<GameManager>().EndGame();
            }
            Destroy(point.gameObject);            
        }
    }

    

    //public Cell GetPlayerPosCell()
    //{        
    //    Cell curPosCell = levelManager.ClosestCell(tr.position);
    //    return curPosCell;
    //}

}   




