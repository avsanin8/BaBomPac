﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {
    

    void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.gameObject.tag == "Point")
        {
            ScoreScript.scoreValue += 10;
            if (ScoreScript.scoreValue == 650)
            {                
                FindObjectOfType<GameManager>().CompleteLevel();
            }
            Destroy(other.gameObject);            
        }
    }

    

    //public Cell GetPlayerPosCell()
    //{        
    //    Cell curPosCell = levelManager.ClosestCell(tr.position);
    //    return curPosCell;
    //}

}   




