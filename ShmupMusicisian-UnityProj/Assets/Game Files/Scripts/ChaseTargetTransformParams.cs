using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChaseTargetTransformParams", menuName = "Misc/ChaseTargetTransformParams")]
public class ChaseTargetTransformParams : ScriptableObject
{
    public float distanceOffset = 0;
    [Space]
    public AnimationCurve pullStrengthAgainstTargetsDistance;
    public AnimationCurve dragAgainstTargetsDistance;
    [Space]
    [Tooltip("The animation curve creates a force that counters orbital movement. This force scales with the orbital speed, along the time axis.")]
    public AnimationCurve orbitalCounterAgainstSpeed;
    public float orbitalCounterStrength = 0;
}
