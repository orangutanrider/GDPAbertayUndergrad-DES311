using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AtPlayerKnockbackData 
{
    public Vector2 knockbackForce;
    public bool stunDuringAirborne;
    public GameObject knockbackSource;

    public AtPlayerKnockbackData(Vector2 _knockbackForce, GameObject _knockbackSource, bool _stunDuringAirborne = true)
    {
        knockbackForce = _knockbackForce;
        knockbackSource = _knockbackSource;
        stunDuringAirborne = _stunDuringAirborne;
    }
}
