using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Fraction
{
    None = 0,
    Player = 1,
    Alien = 2,
    Neutral = 3
}

[System.Serializable]
public struct DamageBase
{

    public Fraction fraction;
    public float value;
    public bool isOverTime;
}

public class Unit : MonoBehaviour {
        
    public float speed;
    public RectTransform tr;
    public float hitPoint;
    public float maxHitPoint;
    public bool lookRight = false;
    
    protected List<Cell> wayPoints;

    //[SerializeField] GameObject _onDeathEffect;
    [SerializeField] protected Fraction _fraction;
    private bool _isDead = false;
    [HideInInspector]
    public bool UnitIsDead { get { return _isDead; } }

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


    public virtual void DoDamage(DamageBase aDamage)
    {
        if (_isDead) { return; }

        if (_fraction == aDamage.fraction)
        {
            return;
        }

        if (aDamage.isOverTime)
        {
            hitPoint -= aDamage.value * Time.fixedDeltaTime;
        }
        else
        {
            hitPoint -= aDamage.value;
        }

        if (hitPoint <= 0)
        {
            hitPoint = 0;
            _isDead = true;
            //UnitIsDead
            Destroy(this.gameObject); //todo: Efect
            //Instantiate(_onDeathEffect, tr.position, tr.rotation);
        }
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
