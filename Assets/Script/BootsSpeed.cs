using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsSpeed : MonoBehaviour {
    public RectTransform tr;

    public float baffSpeed = 100; // add speed
    public float timeBuff = 10; // sec
    //public Timer timer;
    //public Player player;


    //public void HandleEvent(NotificationType eventType)
    //{
    //    throw new System.NotImplementedException();
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.GetComponent<Player>())
        {
            if (!other.GetComponent<Player>().BaffSpeedOn)
            {                
                other.GetComponent<Player>().AddSpeed(baffSpeed);                
            }
            else
                return;
        }
    }


}
