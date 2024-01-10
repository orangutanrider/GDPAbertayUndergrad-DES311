using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BreatheOutBoomParameters", menuName = "Player/BreatheOutBoomParameters")]
public class BreatheOutBoomParameters : ScriptableObject
{
    public LayerMask projectileHitMask;
    public LayerMask explosionHitMask;
    public GameObject explosionPrefab;
    [Space]
    public AnimationCurve explosionForceAgainstPower;
    public AnimationCurve projectileSizeAgainstPower;
    public AnimationCurve projectileRangeAgainstPower;
    public float explosionSizeEffector;
    public float projectileSpeed;
}
