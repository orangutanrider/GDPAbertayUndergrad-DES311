using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BreatheInWhirlwindParameters", menuName = "Player/BreatheInWhirlwindParameters")]
public class BreatheInWhirlwindParameters : ScriptableObject
{
    public LayerMask breatheInterfaceMask;
    [Space]
    public AnimationCurve lifetimeAgainstPower;
    public AnimationCurve totalHitsAgainstPower;
    [Space]
    public AnimationCurve lengthAgainstBreathePower;
    public AnimationCurve forceStrengthAgainstBreathePower;
    public float yDrag;
    public float gravityOverrideDuration;
}
