using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

    public float health = 10;

    private void OnTriggerStay2D(Collider2D other)
    {
        Unit hitUnit = other.GetComponent<Player>();
        if (hitUnit)
        {
            //other.SendMessage("AddHealth", health * Time.deltaTime);
            hitUnit.GetComponent<Player>().AddHealth(health * Time.fixedDeltaTime);
        }
    }

}
