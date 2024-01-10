using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBreatheParameters", menuName = "Player/PlayerBreatheParameters")]
public class PlayerBreatheParameters : ScriptableObject
{
    [Header("Object Spawning")]
    public GameObject breatheOutProjectilePrefab;
    public float breatheOutProjectileSpawnOffset;
    [Space]
    public GameObject breatheInWhirlwindPrefab;
    public float breatheInWhirlwindSpawnOffset;

    [Header("Resource Management")]
    public AnimationCurve powerAgainstWeight;
    public AnimationCurve weightCostCurve;
}
