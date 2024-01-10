using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AtPlayerStunData
{
    public float stunDuration;
    public GameObject stunSource;

    public AtPlayerStunData(float _stunDuration, GameObject _stunSource)
    {
        stunDuration = _stunDuration;
        stunSource = _stunSource;
    }
}
