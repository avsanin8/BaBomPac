using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    // Use this for initialization

    public float speed;

    public RectTransform tr;
    public float hitPoint;
    public float maxHitPoint;
    public bool lookRight = false;
    

    //public HealthBar healthBar;

    protected List<Cell> wayPoints;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        if (wayPoints == null || wayPoints.Count == 0)
        {
            return;
        }
        else
        {
            Vector2 itemPos = wayPoints[0].tr.position;

            if (itemPos.x < tr.position.x && lookRight)
                Turn();
            if (itemPos.x > tr.position.x && !lookRight)
                Turn();

            tr.position = Vector2.MoveTowards(tr.position, itemPos, speed * Time.deltaTime);
            if (Vector2.Distance(tr.position, itemPos) == 0)
            {
                wayPoints.RemoveAt(0);
            }
        }
    }

    public void DefinePath(Cell aTarget)
    {
        Cell startTarget = LevelManager.Instance.ClosestCell(tr.position);
        wayPoints = LevelManager.Instance.CalcShortestWay(startTarget, aTarget);        
    }


    //public void TakeDamage(float damage)
    //{
    //    hitPoint -= damage;
    //    if (hitPoint < 0)
    //    {
    //        hitPoint = 0;
    //        //FindObjectOfType<GameManager>().PlayerDead();
    //        GameManager.Instance.PlayerDead();
    //        Debug.Log("Dead!");
    //    }        
    //}

    //public void AddHealth(float heal)
    //{
    //    // need add some healStaf 
    //    hitPoint += heal;
    //    if (hitPoint > maxHitPoint)
    //    {
    //        hitPoint = maxHitPoint;            
    //    }        
    //}

    //public void AddSpeed(float buffSpeed, float buffTime)
    //{
    //    speed += buffSpeed;
    //    Timer.Instance.TurnOn(buffTime);
    //}

    public void Turn()
    {
        Vector3 tempPos = tr.localScale;
        tempPos.x *= -1;
        tr.localScale = tempPos;
        lookRight = !lookRight;
    }

    




}
