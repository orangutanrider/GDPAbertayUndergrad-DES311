using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AtEnemyDamageData 
{
    public float damageAmount;
    public Vector2 damageDirection;
    public Vector3 damageLocation;
    public GameObject damageDealingSource; // not the bullet itself, whatever shot the bullet should be inputted here

    public AtEnemyDamageData(float _damageAmount, Vector2 _damageDirection, Vector3 _damageLocation, GameObject _damageDealingSource)
    {
        damageAmount = _damageAmount;
        damageDirection = _damageDirection.normalized;
        damageLocation = _damageLocation;
        damageDealingSource = _damageDealingSource;
    }
}
