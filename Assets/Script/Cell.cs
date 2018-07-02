using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public bool isWalk;    
    public RectTransform tr;


    public List <Cell> neighborCells = new List<Cell>();

    //void Start () {   

    //}
	
	
	//void Update () {
		
	//}

    public void AddCell(Cell cell)
    {
        neighborCells.Add(cell);
        if (cell == this)
            return;
        for (int i = 0; i < neighborCells.Count; i++)
        {
            if (neighborCells[i] == cell)
                return;
            if (!neighborCells[i])
            {
                neighborCells[i] = cell;
                return;
            }
        }
    }
}
