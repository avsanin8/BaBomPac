using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour {

    [SerializeField]
    protected DamageBase _damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Unit hitUnit = other.GetComponent<Unit>();
        if (hitUnit) //todo: .enabled ?
        {
            hitUnit.DoDamage(_damage);
        }
    }
}
