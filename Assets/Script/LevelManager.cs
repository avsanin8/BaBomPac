using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //10:J(10) :fieldWidth, fieldHeight
    //public int etap; 
    public int fieldWidth, fieldHeight;
    public float percentBlocks = 50f;
    private int countBlocks = 0;
    //public GameObject groundCellPref;
    //public Transform cellParent;
    //public Sprite[] tileSpr = new Sprite[2];

    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    //public Canvas GameCanvas; // not using temporaly

    public Cell cell;
    public RectTransform heartTr;
    
    public Point pointPref;
    Cell[,] allCells;
    public RectTransform canvas;
    public Player player;

    public int PointScoreWins { get { return _pointScoreWins; } }
    private int _pointScoreWins = 0;

    private Cell cellWalkStart;
    private Cell curClosestCell;
    private Cell minWayLast;

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
        LoadPlayerPos();
    }

    void LoadPlayerPos()
    {
        if (!cellWalkStart)
            player.tr.position = FindPlayerCell().tr.position;
        else
            player.tr.position = cellWalkStart.tr.position;
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
                CreateLevel_02();
                break;
        }
    }

    public List<Cell> CalcShortestWay(Cell pointStart, Cell pointEnd)
    {
        if (!pointEnd || !pointEnd.IsWalk)
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
                        if (!currNode.neighborCells[j].IsWalk)
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
        Vector3 tempVector3 = allCells[fieldWidth - 1, fieldHeight - 1].tr.position;

        Cell cellClosest = null;
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                if (cellClosest != null && pos.GetHashCode() == cellClosest.GetHashCode())
                    continue;
                //(allCells[x, y].transform.position - pos).magnitude < (cellClosest.transform.position - pos).magnitude)
                if (allCells[x, y].IsWalk && (allCells[x, y].tr.position - pos).magnitude < (tempVector3 - pos).magnitude)
                {
                    tempVector3 = allCells[x, y].tr.position;
                    cellClosest = allCells[x, y];
                }
            }
        }
        return cellClosest;

    }

    // Using for enemy search and First Level pos player
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
        ScoreScript.scoreValue = 0;
        _pointScoreWins = 0;

        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                Vector2 anchorPos = new Vector2(100f * x, -100f * y);
                Cell curCell;

                if (x == 0 || y == 0 || x == fieldWidth - 1 || y == fieldHeight - 1)
                {
                    curCell = Instantiate(cell, canvas);
                    curCell.IsWalk = false; //block
                }

                else if (x % 2 == 0 && y % 2 == 0)
                {
                    curCell = Instantiate(cell, canvas);
                    curCell.IsWalk = false;
                }
                else
                {
                    curCell = Instantiate(cell, canvas);
                    curCell.IsWalk = true;
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
                if (allCells[x, y].IsWalk)
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
                if (allCells[x, y].IsWalk)
                {
                    curPoint = Instantiate(pointPref, canvas);
                    //curPoint.player = player;
                    curPoint.pointTr.anchoredPosition = anchorPos;                    
                    _pointScoreWins++;
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
                    if (neighborCell == allCells[x, y])
                    {
                        if (x > 0 && allCells[x + 1, y].IsWalk && y < fieldWidth)
                        {
                            neighborCell.AddCell(allCells[x + 1, y]);
                        }
                        if (x > 0 && allCells[x - 1, y].IsWalk && y < fieldWidth)
                        {
                            neighborCell.AddCell(allCells[x - 1, y]);
                        }
                        if (y > 0 && allCells[x, y + 1].IsWalk && y < fieldHeight)
                        {
                            neighborCell.AddCell(allCells[x, y + 1]);
                        }
                        if (y > 0 && allCells[x, y - 1].IsWalk && y < fieldHeight)
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

    //Random Generator Field
    void CreateLevel_02()
    {
        ScoreScript.scoreValue = 0;
        _pointScoreWins = 0;

        countBlocks = Mathf.RoundToInt((fieldWidth * fieldHeight) * percentBlocks / 100f);
        //Debug.Log("percentBlocks: " + percentBlocks + " countBlocks: " + countBlocks);

        for (int x = 0; x < fieldWidth; x++)
        {                        
            for (int y = 0; y < fieldHeight; y++)
            {                
                Vector2 anchorPos = new Vector2(100f * x, -100f * y);
                Cell curCell;

                if (x == 0 || y == 0 || x == fieldWidth - 1 || y == fieldHeight - 1)
                {
                    curCell = Instantiate(cell, canvas);
                    curCell.IsWalk = false;
                }

                else
                {
                    curCell = Instantiate(cell, canvas);
                    curCell.IsWalk = true;                    
                }

                curCell.tr.anchoredPosition = anchorPos;
                allCells[x, y] = curCell;
            }
        }
        //Genered blocks
        while (countBlocks > 0)
        {
            int randX = Random.Range(1, (fieldWidth - 2));
            int randY = Random.Range(1, (fieldHeight - 2));

            if (allCells[randX, randY].IsWalk == false)
                continue;

            allCells[randX, randY].IsWalk = false;
            countBlocks--;
        }

        //Debug.Log("after genered countBlocks : " + countBlocks);

        //passableCellsCopy =  new Dictionary<Cell, bool>(passableCells);

        //Fixed field
        SetAllNaighbor();
        cellWalkStart = null;     // start pos Player
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                if (x == 0 || y == 0 || x == fieldWidth - 1 || y == fieldHeight - 1)
                {
                    continue;
                }

                if (allCells[x, y].IsWalk && !cellWalkStart)
                {
                    cellWalkStart = allCells[x, y];
                    //Debug.Log("cell Walk Start pos x: " + cellWalkStart.tr.position.x + " y:" + cellWalkStart.tr.position.y);
                    break;
                }
            }
        }
        curClosestCell = ClosestWalkableCell(cellWalkStart); //Closest Walkable Cell

        SetWalkingMapCells(cellWalkStart, curClosestCell);

        //Debug.Log("closestCell pos x:" + curClosestCell.tr.position.x + " y: " + curClosestCell.tr.position.y);

        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                if (x == 0 || y == 0 || x == fieldWidth - 1 || y == fieldHeight - 1)
                {
                    continue;
                }
                if (CalcShortestWay(cellWalkStart, allCells[x, y]) == null && allCells[x, y].IsWalk)
                {
                    curClosestCell = ClosestWalkableCell(allCells[x, y]);

                    if (CalcShortestWay(cellWalkStart, curClosestCell) != null) // if we can walk to curClosestCell
                    {
                        SetWalkingMapCells(curClosestCell, allCells[x, y]);
                    }
                    else
                    {
                        // find the closest cell of curClosestCell to which we can walk
                        Cell tepmNearestCell = ClosestWhichCanWalk(curClosestCell);

                        SetWalkingMapCells(tepmNearestCell, allCells[x, y]);
                    }
                }
                else
                    continue;
            }
        }

        // Set Point prefab
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                Vector2 anchorPos = new Vector2(100 * x, -100 * y);
                Point curPoint;
                if (allCells[x, y].IsWalk)
                {
                    curPoint = Instantiate(pointPref, canvas);                    
                    curPoint.pointTr.anchoredPosition = anchorPos;                    
                    _pointScoreWins++;
                }
            }
        }

        //Set Heart pos
        Cell tempCellHeart = ClosestCell(heartTr.position);
        heartTr.position = tempCellHeart.tr.position;
        
        NotificationManager.Instance.PostEventListener(NotificationType.levelIsGenerated);
    }

    Cell ClosestWhichCanWalk(Cell curClosestCell)
    {
        Cell nearestPassCell = null;
        float mag = Mathf.Infinity;
                    
        if (CalcShortestWay(cellWalkStart, curClosestCell) != null)
        {
            nearestPassCell = curClosestCell;
        }
        else
        {            
            for (int x = 0; x < fieldWidth; x++)
            {
                for (int y = 0; y < fieldHeight; y++)
                {
                    if (CalcShortestWay(cellWalkStart, allCells[x,y]) != null)
                    {                        
                        if ((allCells[x, y].tr.position - curClosestCell.tr.position).magnitude < mag)
                        {                            
                            mag = allCells[x, y].tr.position.magnitude;
                            nearestPassCell = allCells[x, y];
                        }   
                    }
                }
            }
        }

        if (!nearestPassCell)
        {
            int a = 0;
            a++;
            Debug.Log("Trable in nearestPassCell: " + a);
        }
        return nearestPassCell;
    }
    

    void SetWalkingMapCells(Cell pointStart, Cell pointEnd)
    {
        if (!pointStart || !pointEnd)
        {
            Debug.Log("!pointEnd: " + pointStart + " || !pointEnd Not Found : "+ pointEnd);            
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
            Debug.Log("pointEnd NOT Found");
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
        
        if (minWay.Count != 0)
        {
            //Debug.Log("minWay.Count: " + minWay.Count);
            foreach (Cell item in minWay)
            {                
                item.IsWalk = true;              
                
            }
            minWayLast = minWay[minWay.Count - 1];            
        }

        if (minWay.Count == 0)
        {
            Debug.Log("minWay.Count == 0");
        }

        if (!minWayLast)
            Debug.Log("minWayLast is null: " + minWayLast);
    }

    public Cell ClosestWalkableCell(Cell startCell)
    {           
        float mag = Mathf.Infinity;
        Cell cellClosest = null;
        
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                if (startCell != allCells[x, y]) //&& passableCells.ContainsKey(allCells[x, y]) && !passableCells[allCells[x, y]])
                {
                    if (allCells[x, y].IsWalk && (allCells[x, y].tr.position - startCell.tr.position).magnitude < mag)
                    {
                        mag = allCells[x, y].tr.position.magnitude;
                        cellClosest = allCells[x, y];
                    }
                }
            }
        }
        if (!cellClosest)
        {
            int a = 0;
            a++;
            Debug.Log("Trable:" + a + "cellClosest is NULL " + cellClosest);
        }
        return cellClosest;

    }

    public void SetAllNaighbor()
    {
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int y = 0; y < fieldHeight; y++)
            {
                if (x == 0 || y == 0 || x == fieldWidth - 1 || y == fieldHeight - 1)
                {
                    continue;
                }
                if (x > 0 && x + 1 != fieldWidth-1 && y < fieldHeight)
                {
                    allCells[x, y].AddCell(allCells[x + 1, y]);
                }
                if (x > 0 && x - 1 != 0 && y < fieldHeight)
                {
                    allCells[x, y].AddCell(allCells[x - 1, y]);
                }
                if (y > 0 && y + 1 != fieldHeight - 1 &&  y < fieldHeight)
                {
                    allCells[x, y].AddCell(allCells[x, y + 1]);
                }
                if (y > 0 && y - 1 != 0 && y < fieldHeight)
                {
                    allCells[x, y].AddCell(allCells[x, y - 1]);
                }
            }
        }
    }



    }
