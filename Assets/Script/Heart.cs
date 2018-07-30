using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

    public float health = 10;

    private void OnTriggerStay2D(Collider2D other)
    {
        //if (other.tag == "Player")// todo: remove .tag
        if (other.GetComponent<Player>())
        {
            //other.SendMessage("AddHealth", health * Time.deltaTime);
            other.GetComponent<Player>().AddHealth(health * Time.deltaTime);
        }
    }

}
