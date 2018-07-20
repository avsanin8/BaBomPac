using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsSpeed : MonoBehaviour {
    public RectTransform tr;

    public float addSpeed = 100; // add speed
    public float timeBuff = 10; // sec

    //private void LateUpdate()
    //{
    //    Wait();
    //    SetPosition();
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //other.SendMessage("AddHealth", health * Time.deltaTime);
            other.GetComponent<Player>().AddSpeed(addSpeed, timeBuff);
        }
    }

    //IEnumerator Wait()
    //{        
    //    yield return new WaitForFixedUpdate();
    //}

    //void SetPosition()
    //{
    //    Cell tempCell = LevelManager.Instance.ClosestCell(tr.position);
    //    tr.position = tempCell.tr.position;
    //}
}
