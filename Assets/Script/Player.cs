using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {
    public Animator animator;
    private float currSpeed;
    //private bool _isDead = false;
    //public bool IsDead { get { return _isDead; } }
    private AudioSource audioEfectPoint;
    public Collider2D weponAxeCol;
    

    //timerBuff
    public Timer timer; //todo: remove this
    public BootsSpeed bootsSpeed;
    private bool _baffSpeedOn = false;
    public bool timerTurnOn = false;
    private float _timeBuff;
    private string _timerMin;
    private string _timerSec;
    public bool BaffSpeedOn { get { return _baffSpeedOn; } }
    public float TimeBuff { get { return _timeBuff; } }
    public string TimerMin { get { return _timerMin; } }
    public string TimerSec { get { return _timerSec; } }


    protected override void Start()
    {
        audioEfectPoint = GetComponent<AudioSource>();        
        weponAxeCol.enabled = false;

        if (GlobalControl.Instance.healthUnit > 0)
            hitPoint = GlobalControl.Instance.healthUnit;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Point>())
        {
            ScoreScript.scoreValue += 10;
            ScoreScript.maxScoreValue += 10;
            if (ScoreScript.scoreValue == LevelManager.Instance.PointScoreWins * 10)
            {
                GameManager.Instance.CompleteLevel();
            }
            audioEfectPoint.Play();
            Destroy(other.gameObject);
        }
    }
    [SerializeField]
    protected float _weaponCD;
    private float _curWeaponCD = 0.0f;
    [HideInInspector]
    public float WeaponCD { get { return _weaponCD; } }
    public float CurWeaponCD { get { return _curWeaponCD; } }

    protected override void Update()
    {        
        Vector3 curPosPlayer = tr.position; // for speed anim
        base.Update();
        if (Input.GetMouseButtonDown(1) && _curWeaponCD <= 0.0f) //Attack
        {
            if (Random.Range(0f, 1.0f) > 0.5f)
            {
                animator.SetTrigger("attack");
            }
            else
            {
                animator.SetTrigger("special");                                
            }
            _curWeaponCD = _weaponCD;
        }
        animator.SetFloat("speed", (transform.position - curPosPlayer).magnitude / Time.deltaTime);

        if ((!IsAnimationPlaying("attack") || !IsAnimationPlaying("special attack")) && weponAxeCol.enabled)
        {
            weponAxeCol.enabled = false;            
        }
        if (IsAnimationPlaying("attack") || IsAnimationPlaying("special attack") && !weponAxeCol.enabled)
        {
            weponAxeCol.enabled = true;            
        }
        if (_curWeaponCD > 0.0f)
            _curWeaponCD -= Time.deltaTime;


        if (BaffSpeedOn)
        {
            TimerUpdate();
        }
        else if (_timeBuff <= 0)
        {
            timerTurnOn = false;
        }
        else return;
    }

    //public void TakeDamage(float damage)
    //{
    //    if (_isDead)
    //        return;

    //    hitPoint -= damage;

    //    if (hitPoint <= 0)
    //    {
    //        hitPoint = 0;
    //        _isDead = true;            
    //        NotificationManager.Instance.PostEvent(NotificationType.playerIsDied);
    //    }
    //}
    //public override void DoDamage(DamageBase aDamage)
    //{
    //    base.DoDamage(aDamage);
    //    NotificationManager.Instance.PostEvent(NotificationType.playerIsDied);
    //}

    public void AddHealth(float heal)
    {
        // need add some healStaf 
        hitPoint += heal;
        if (hitPoint > maxHitPoint)
        {
            hitPoint = maxHitPoint;
        }
    }

    public void AddSpeed(float buffSpeed)
    {
        _baffSpeedOn = true;
        currSpeed = speed;
        OnOfTimerBuff();
        speed += buffSpeed;
    }

    void OnOfTimerBuff()
    {
        timerTurnOn = true;
        _timeBuff = bootsSpeed.timeBuff;
    }

    void TimerUpdate()
    {
        _timeBuff -= Time.deltaTime;
        _timerMin = ((int)_timeBuff / 60).ToString();
        _timerSec = (_timeBuff % 60).ToString("f2");        

        if (_timeBuff <= 0)
        {
            _timeBuff = 0;            
            _baffSpeedOn = false;
            ReturnSpeed();
        }
    }

    public void ReturnSpeed()
    {
        speed = currSpeed;
    }

    public bool IsAnimationPlaying(string animationName)
    {
        // берем информацию о состоянии
        var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // смотрим, есть ли в нем имя какой-то анимации, то возвращаем true
        if (animatorStateInfo.IsName(animationName))
            return true;

        return false;
    }

    //Save current data to GlobalControl   
    void OnDestroy()
    {
        GlobalControl.Instance.healthUnit = hitPoint;
    }


}   




