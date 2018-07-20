using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit {
    public int searchRadius = 200;
    public float damage = 10f;



    //protected override void Start()
    //{        
    //    Cell tempCell = LevelManager.Instance.ClosestCell(tr.position);
    //    tr.position = tempCell.tr.position;
    //}

    protected override void Update () {

        base.Update();
        if (wayPoints == null || wayPoints.Count == 0)
        {            
            DefinePath(SearchRadius());
            return;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //other.SendMessage("TakeDamage", damage * Time.deltaTime);       // huinya     
            other.GetComponent<Player>().TakeDamage(damage * Time.deltaTime);
        }
    }


    private Cell SearchRadius()
    {
        Cell curPosPlayer = LevelManager.Instance.FindPlayerCell();
        Cell target = null;

        Vector3 randRadTarget = new Vector3(curPosPlayer.tr.position.x, curPosPlayer.tr.position.y, 0f);
        randRadTarget = Random.onUnitSphere * searchRadius;
        randRadTarget[2] = 0f;

        Vector3 curTarget = curPosPlayer.tr.position - randRadTarget; // получаем рандомный вокруг плеера        

        if (target != curPosPlayer)
            target = LevelManager.Instance.ClosestCell(curTarget);
        
        return target;


        ////Not bad
        //int index;
        //Cell target = null;
        //Cell curPosPlayer = levelManager.FindPlayerCell();
        //List<Cell> currNeighbours = curPosPlayer.neighborCells;

        //var random = new System.Random();

        //index = random.Next(0, currNeighbours.Count);
        //target = currNeighbours[index];

        //currNeighbours = currNeighbours[index].neighborCells;

        //index = random.Next(0, currNeighbours.Count);
        //if (currNeighbours[index] != curPosPlayer)
        //    target = currNeighbours[index];

        //return target;
    }

}
