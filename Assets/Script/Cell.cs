using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {
        
    private bool _isWalk = false;
    public RectTransform tr;
    public Sprite ground;
    public Sprite block;

    public Image image;

    public List<Cell> neighborCells = new List<Cell>();



    public bool IsWalk
    {
        get
        {
            return _isWalk;
        }
        set
        {
            _isWalk = value;
            if (_isWalk)
            {
                image.sprite = ground;
            }
            else image.sprite = block;
        }
    }


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
