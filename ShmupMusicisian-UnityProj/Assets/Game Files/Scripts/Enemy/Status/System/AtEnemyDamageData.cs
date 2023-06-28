using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AtEnemyDamageData 
{
    public float damageAmount;
    public Vector2 damageDirection;
    public GameObject damageSource;

    public AtEnemyDamageData(float _damageAmount, Vector2 _damageDirection, GameObject _damageSource)
    {
        damageAmount = _damageAmount;
        damageDirection = _damageDirection.normalized;
        damageSource = _damageSource;
    }
}
