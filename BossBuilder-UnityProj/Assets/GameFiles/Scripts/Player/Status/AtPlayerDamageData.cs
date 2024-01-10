using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AtPlayerDamageData
{
    public float damageAmount;
    public Vector2 damageDirection;
    public GameObject damageSource;

    public bool doesTriggerIFrames;

    public AtPlayerDamageData(float _damageAmount, Vector2 _damageDirection, GameObject _damageSource, bool _doesTriggerIFrames)
    {
        damageAmount = _damageAmount;
        damageDirection = _damageDirection.normalized;
        damageSource = _damageSource;
        doesTriggerIFrames = _doesTriggerIFrames;
    }
}
