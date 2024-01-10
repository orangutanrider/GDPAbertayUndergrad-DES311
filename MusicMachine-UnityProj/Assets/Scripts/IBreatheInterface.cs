using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBreatheInterface
{
    public float KnobValue
    {
        get;
        set;
    }

    public void HitByBreatheOut(float breathePower);
    public void HitByBreatheIn(float breathePower);
}
