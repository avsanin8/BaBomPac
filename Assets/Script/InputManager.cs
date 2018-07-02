using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InputManager : MonoBehaviour {

    public Player player;
    
    //private float targetDistance;

    void Update () {
        if (Input.GetMouseButtonDown(0)) // && !EventSystem.current.IsInvoking() - not work
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            else
                ClickSelect();
        }
    }

    void ClickSelect()
    {       

        //Converting Mouse Pos to 2D (vector2) World Pos
        Vector2 camVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //RaycastHit2D hit = Physics2D.Raycast(camVector, this.gameObject.transform.position, 0f, 1 << LayerMask.NameToLayer("Ground"));


        Collider2D hit = Physics2D.OverlapPoint(camVector,  1 << LayerMask.NameToLayer("Ground"));
        if (hit)
        {            
            //Debug.Log(hit.transform.name);                            
            player.DefinePath(hit.transform.GetComponent<Cell>());            
        }
        else
        {
            player.DefinePath(null);
        }        
    }




}
