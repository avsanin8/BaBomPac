using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour {

    [SerializeField]
    protected DamageBase _damage;    
    private List<Unit> listHitUnit = new List<Unit>();
    private Color weaponColor;
    public Player player;


    private void Awake()
    {
        weaponColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    private void Update()
    {        
        if (!player.weponAxeCol.enabled)
        {
            ResetHit();
        }
        if (player.IsAnimationPlaying("attack") || player.IsAnimationPlaying("special attack"))
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            
        }
        else gameObject.GetComponent<SpriteRenderer>().color = weaponColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Unit hitUnit = other.GetComponent<Unit>();

        Unit tempUnit = other.GetComponent<Unit>();

        if (tempUnit)
        {
            if (listHitUnit.Contains(tempUnit))
            {
                return;
            }
            else
            {
                listHitUnit.Add(tempUnit);
                tempUnit.DoDamage(_damage);
                
            }
            
        }        
    }

    public void ResetHit()
    {
        if (listHitUnit != null)
            listHitUnit.Clear();

        
    }

}
