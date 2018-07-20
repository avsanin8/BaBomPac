using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {
    public Animator animator;

    protected override void Start()
    {
        if (GlobalControl.Instance.healthUnit > 0)
            hitPoint = GlobalControl.Instance.healthUnit;
    }

    void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.gameObject.tag == "Point")
        {
            ScoreScript.scoreValue += 10;
            ScoreScript.maxScoreValue += 10;
            if (ScoreScript.scoreValue == LevelManager.Instance.PointScoreWins * 10)
            {                
                //FindObjectOfType<GameManager>().CompleteLevel();
                GameManager.Instance.CompleteLevel();
            }
            Destroy(other.gameObject);            
        }
    }

    protected override void Update()
    {
        Vector3 curPosPlayer = tr.position; // for speed anim
        base.Update();
        if (Input.GetMouseButtonDown(1))
        {
            if (Random.Range(0f, 1.0f) > 0.5f)
                animator.SetTrigger("attack");
            else
                animator.SetTrigger("special");
        }
        animator.SetFloat("speed", (transform.position - curPosPlayer).magnitude / Time.deltaTime);
    }
    public void TakeDamage(float damage)
    {
        hitPoint -= damage;
        if (hitPoint < 0)
        {
            hitPoint = 0;
            //FindObjectOfType<GameManager>().PlayerDead();
            //GameManager.Instance.PlayerDead();
            NotificationManager.Instance.PostEventListener(NotificationType.playerIsDied);
            Debug.Log("Dead!");
        }
    }

    public void AddHealth(float heal)
    {
        // need add some healStaf 
        hitPoint += heal;
        if (hitPoint > maxHitPoint)
        {
            hitPoint = maxHitPoint;
        }
    }

    public void AddSpeed(float buffSpeed, float buffTime)
    {
        speed += buffSpeed;
        //Timer.TurnOn(buffTime);
        NotificationManager.Instance.PostEventListener(NotificationType.timerOn);
    }

    //Save current data to GlobalControl   
    void OnDestroy()
    {
        GlobalControl.Instance.healthUnit = hitPoint;
    }


}   




