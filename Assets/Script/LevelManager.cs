using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //10:J(10) :fieldWidth, fieldHeight
    //public int etap; 
    public int fieldWidth, fieldHeight;
    //public float offset=2;
    //public GameObject groundCellPref;
    //public Transform cellParent;
    //public Sprite[] tileSpr = new Sprite[2];

    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    public Canvas GameCanvas; // not using temporaly

    public Cell grounPref;
    public Cell blokPref;
    public Point pointPref;
    Cell[,] allCells;
    public RectTransform canvas;
    public Player player;


    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            Debug.LogError("More then one singletone of LevelManager");
        }
    }

    void Start()
    {
        SetFieldCells();
        LoadLevel();
    }

    void LoadLevel()
    {
        int levIndex = SceneManager.GetActiveScene().buildIndex;
        switch (levIndex)
        {
            case 0:
                break;
            case 1:
                CreateLevel_01();
                break;
            case 2:
                CreateLevel_02();
                break;
            default:
                break;
        }
    }

    public List<Cell> CalcShortestWay(Cell pointStart, Cell pointEnd)
    {
        if (!pointEnd || !pointEnd.isWalk)
        {
            return null;
        }

        float minDist = CalcMinDist(pointStart, pointEnd);

        List<Cell> openList = new List<Cell>();
        List<Cell> closedList = new List<Cell>();
        Dictionary<Cell, Cell> from = new Dictionary<Cell, Cell>();
        Dictionary<Cell, float> weightG = new Dictionary<Cell, float>(); 
        Dictionary<Cell, float> weightF = new Dictionary<Cell, float>(); 

        weightG.Add(pointStart, 0f);
        weightF.Add(pointStart, weightG[pointStart] + minDist);

        openList.Add(pointStart);

        while (openList.Count > 0)
        {
            Cell currNode = null;
            float curMinF = 1000000.0f;
            for (int i = 0; i < openList.Count; i++)
            {
                if (weightF[openList[i]] < curMinF) 
                {
                    curMinF = CalcMinDist(openList[i], pointEnd);
                    if (currNode == pointEnd)
                    {
                        break;
                    }

                    currNode = openList[i];

                    openList.Remove(currNode);
                    closedList.Add(currNode);
                    for (int j = 0; j < currNode.neighborCells.Count; j++)
                    {
                        if (!currNode.neighborCells[j].isWalk)
                        {
                            continue;
                        }
                        if (closedList.Contains(currNode.neighborCells[j]))
                        {
                            continue;
                        }
                        float tempG = weightG[currNode] + CalcMinDist(currNode, currNode.neighborCells[j]);
                        if (!openList.Contains(currNode.neighborCells[j]) || tempG < weightG[currNode.neighborCells[j]])
                        {
                            from[currNode.neighborCells[j]] = currNode;
                            weightG[currNode.neighborCells[j]] = tempG;
                            weightF[currNode.neighborCells[j]] = weightG[currNode.neighborCells[j]] + CalcMinDist(currNode.neighborCells[j], pointEnd);
                            if (!openList.Contains(currNode.neighborCells[j]))
                                openList.Add(currNode.neighborCells[j]);
                        }
                    }
                }
            }
        }
        if (!from.ContainsKey(pointEnd))
        {
            return null;
        }

        List<Cell> minWay = new List<Cell>();
        minWay.Add(pointEnd);
        int count = 0;
        while (!minWay.Contains(pointStart))
        {
            minWay.Insert(0, from[minWay[0]]);
            count++;
            if (count > 1000)
                minWay = null;
        }

        //if (minWay.Contains(pointStart))
        //    Debug.Log("Wery good: is minWay Contains pointStart !! :)");

        return minWay;
    }

    public float CalcMinDist(Cell pointStart, Cell pointEnd)
    {
        float curMinDistNeibor = 0f;
        curMinDistNeibor = Vector2.Distance(pointStart.transform.position, pointEnd.transform.position);
        return curMinDistNeibor;
    }

    public Cell ClosestCell(Vector3 pos)
    {
        //float tempMaxMagnitude = Mathf.Infinity;        
        Vector3 tempVector3 = allCells[fieldWidth-1, fieldHeight-1].tr.position;
        //Debug.Log("tempVector3 pos.x:" + tempVector3.x + " y:" + tempVector3.y + " fieldWidth: "+ fieldWidth + " fieldHeight: "+ fieldHeight);
        Cell cellClosest = null;//allCells[fieldWidth-1, fieldHeight-1];        
        for (int x = 0; x < fieldWidth; x++)
        {            
            for (int y = 0; y < fieldHeight; y++)
            {
                if (cellClosest !=null && pos.GetHashCode() == cellClosest.GetHashCode())
                    continue;
                //(allCells[x, y].transform.position - pos).magnitude < (cellClosest.transform.position - pos).magnitude)
                if (allCells[x, y].isWalk && (allCells[x, y].tr.position - pos).magnitude < (tempVector3 - pos).magnitude)
                {
                    tempVector3 = allCells[x, y].tr.position;
                    cellClosest = allCells[x, y];
                }
            }
        }
        //Debug.Log("cellClosest x: " + cellClosest.tr.position.x + " y: " + cellClosest.tr.position.y);
        return cellClosest;
        
    }

    public Cell FindPlayerCell()
    {
        if (player)
        {            
            return ClosestCell(player.tr.position);
        }
        else
            return null;
    }

    void SetFieldCells()
    {
        allCells = new Cell[fieldWidth, fieldHeight];
    }

    void CreateLevel_01()
    {
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                Vector2 anchorPos = new Vector2(100f * x, -100f * y);
                Cell curCell;
                
                if (x == 0 || y == 0 || x == fieldWidth - 1 || y == fieldHeight - 1)
                {                    
                    curCell = Instantiate(blokPref, canvas);
                }

                else if (x % 2 == 0 && y % 2 == 0)
                {                   
                    curCell = Instantiate(blokPref, canvas);                    
                }
                else
                {
                    curCell = Instantiate(grounPref, canvas);                    
                }
                
                curCell.tr.anchoredPosition = anchorPos;
                allCells[x, y] = curCell;
            }
        }

        //Set Cell curNeighborCell;
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                if (allCells[x,y].isWalk)
                {                    
                    Cell curNeighborCell = allCells[x, y];
                    if (curNeighborCell)
                    {
                        SetNaighbor(curNeighborCell);
                    }
                }
            }
        }

        // Set Point prefab
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                Vector2 anchorPos = new Vector2(100 * x, -100 * y);
                Point curPoint;
                if (allCells[x, y].isWalk)
                {
                    curPoint = Instantiate(pointPref, canvas);
                    //curPoint.player = player;
                    curPoint.pointTr.anchoredPosition = anchorPos;
                }
            }
        }

        //print coord
        //PrintCellAllCoord();
    }

    public void SetNaighbor(Cell neighborCell)
    {
        if (neighborCell)
        {
            for (int x = 0; x < fieldWidth; x++)
            {
                for (int y = 0; y < fieldHeight; y++)
                {
                    if (neighborCell == allCells[x,y])
                    {
                        if (x > 0 && allCells[x + 1, y].isWalk && y < fieldWidth)
                        {
                            neighborCell.AddCell(allCells[x + 1, y]);
                        }
                        if (x > 0 && allCells[x - 1, y].isWalk && y < fieldWidth)
                        {
                            neighborCell.AddCell(allCells[x - 1, y]);
                        }
                        if (y > 0 && allCells[x, y + 1].isWalk && y < fieldHeight)
                        {
                            neighborCell.AddCell(allCells[x, y + 1]);
                        }
                        if (y > 0 && allCells[x, y - 1].isWalk && y < fieldHeight)
                        {
                            neighborCell.AddCell(allCells[x, y - 1]);
                        }
                    }
                }
            }
            //Debug.Log("neighborCell is SET: "+ neighborCell);
            
        }


        if (!neighborCell) Debug.Log("neighborCell is null " + neighborCell);
    }

    void CreateLevel_02()
    {
        for (int x = 0; x < fieldWidth; x++)
        {
            int rand = Random.Range(0, (fieldHeight / 2));
            for (int y = 0; y < fieldHeight; y++)
            {
                Vector2 anchorPos = new Vector2(100f * x, -100f * y);
                Cell curCell;

                if (x == 0 || y == 0 || x == fieldWidth - 1 || y == fieldHeight - 1)
                {
                    curCell = Instantiate(blokPref, canvas);
                }

                else if (rand < Random.Range(0, fieldHeight / 2))
                {
                    curCell = Instantiate(blokPref, canvas);
                }
                else
                {
                    curCell = Instantiate(grounPref, canvas);
                }

                curCell.tr.anchoredPosition = anchorPos;
                allCells[x, y] = curCell;
            }
        }

        //Cell curNeighborCell;
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                if (allCells[x, y].isWalk)
                {
                    Cell curNeighborCell = allCells[x, y];
                    if (curNeighborCell)
                    {
                        SetNaighbor(curNeighborCell);
                    }
                }
            }
        }



        //Fixed field
        for (int x = 0; x < fieldWidth; x++)
        {
            Cell cellWalkStart = null;
            for (int y = 0; y < fieldHeight; y++)
            {
                //Vector2 anchorPos = new Vector2(100f * x, -100f * y);
                //Cell curCellFixed;
                if (x == 0 || y == 0 || x == fieldWidth - 1 || y == fieldHeight - 1)
                {
                    continue;
                }
                if (allCells[x, y].isWalk)
                {
                    cellWalkStart = allCells[x, y];
                }

                if (allCells[x, y].isWalk && allCells[x, y].neighborCells.Count == 0)
                {
                    //List<Cell> curWayPoints = SetWalkingMapCells(cellWalkStart, allCells[x, y]);

                    if (cellWalkStart != null)
                        SetWalkingMapCells(cellWalkStart, allCells[x, y]);
                    
                    
                    //if (curWayPoints == null || curWayPoints.Count == 0)
                    //{
                    //    return;
                    //}
                    //else
                    //{


                    //    curCellFixed = curWayPoints[0];
                    //    curCellFixed = Instantiate(grounPref, canvas);
                    //    curCellFixed.tr.anchoredPosition = anchorPos;


                    //    curWayPoints.RemoveAt(0);                        
                    //}

                    //if (x != 0 || x != fieldWidth - 1 || x + 1 != fieldWidth - 1)
                    //{
                    //    curCellFixed = Instantiate(grounPref, canvas);
                    //    curCellFixed.tr.anchoredPosition = anchorPos;
                    //    allCells[x + 1, y] = curCellFixed;
                    //    allCells[x + 1, y].isWalk = true;
                    //    SetNaighbor(allCells[x + 1, y]);
                    //}

                }
                else
                {
                    continue;
                }
            }
        }

        // Set Point prefab
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                Vector2 anchorPos = new Vector2(100 * x, -100 * y);
                Point curPoint;
                if (allCells[x, y].isWalk)
                {
                    curPoint = Instantiate(pointPref, canvas);
                    //curPoint.player = player;
                    curPoint.pointTr.anchoredPosition = anchorPos;
                }
            }
        }

        
    }


    List<Cell> SetWalkingMapCells(Cell pointStart, Cell pointEnd)
    {
        if (!pointEnd || !pointEnd.isWalk)
        {
            return null;
        }

        float minDist = CalcMinDist(pointStart, pointEnd);

        List<Cell> openList = new List<Cell>();
        List<Cell> closedList = new List<Cell>();
        Dictionary<Cell, Cell> from = new Dictionary<Cell, Cell>();
        Dictionary<Cell, float> weightG = new Dictionary<Cell, float>();
        Dictionary<Cell, float> weightF = new Dictionary<Cell, float>();

        weightG.Add(pointStart, 0f);
        weightF.Add(pointStart, weightG[pointStart] + minDist);

        openList.Add(pointStart);

        while (openList.Count > 0)
        {
            Cell currNode = null;
            float curMinF = 1000000.0f;
            for (int i = 0; i < openList.Count; i++)
            {
                if (weightF[openList[i]] < curMinF)
                {
                    curMinF = CalcMinDist(openList[i], pointEnd);
                    if (currNode == pointEnd)
                    {
                        break;
                    }

                    currNode = openList[i];

                    openList.Remove(currNode);
                    closedList.Add(currNode);
                    //Тут подумать: шо робить с neighborCells если их нема 0
                    for (int j = 0; j < currNode.neighborCells.Count; j++)
                    {
                        if (!currNode.neighborCells[j].isWalk)
                        {
                            continue;
                        }
                        if (closedList.Contains(currNode.neighborCells[j]))
                        {
                            continue;
                        }
                        float tempG = weightG[currNode] + CalcMinDist(currNode, currNode.neighborCells[j]);
                        if (!openList.Contains(currNode.neighborCells[j]) || tempG < weightG[currNode.neighborCells[j]])
                        {
                            from[currNode.neighborCells[j]] = currNode;
                            weightG[currNode.neighborCells[j]] = tempG;
                            weightF[currNode.neighborCells[j]] = weightG[currNode.neighborCells[j]] + CalcMinDist(currNode.neighborCells[j], pointEnd);
                            if (!openList.Contains(currNode.neighborCells[j]))
                                openList.Add(currNode.neighborCells[j]);
                        }
                    }
                }
            }
        }
        if (!from.ContainsKey(pointEnd))
        {
            return null;
        }

        List<Cell> minWay = new List<Cell>();
        minWay.Add(pointEnd);
        int count = 0;
        while (!minWay.Contains(pointStart))
        {
            minWay.Insert(0, from[minWay[0]]);
            count++;
            if (count > 1000)
                minWay = null;
        }

        //if (minWay.Contains(pointStart))
        //    Debug.Log("Wery good: is minWay Contains pointStart !! :)");

        return minWay;
    }






}
