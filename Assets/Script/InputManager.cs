using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InputManager : MonoBehaviour {

    public Player player;
    private Cell targetHitCell = null;    

    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            else
                ClickSelect();
        }
    }

    void ClickSelect()
    {
        Vector2 camVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //RaycastHit2D hit = Physics2D.Raycast(camVector, this.gameObject.transform.position, 0f, 1 << LayerMask.NameToLayer("Ground"));
        
        Collider2D hit = Physics2D.OverlapPoint(camVector,  1 << LayerMask.NameToLayer("Ground"));
        if (hit)
        {
            targetHitCell = hit.transform.GetComponent<Cell>();
            if (targetHitCell.IsWalk)
                player.DefinePath(targetHitCell);            
        }
        else
        {
            player.DefinePath(null);
        }
    }




}
