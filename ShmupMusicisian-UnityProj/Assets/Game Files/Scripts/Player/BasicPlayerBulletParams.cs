using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBasicBulletParams", menuName = "Player/BasicBulletParams")]
public class BasicPlayerBulletParams : ScriptableObject
{
    public LayerMask enemyMask;
    public LayerMask bulletBoundaryMask;

    [Header("Shooter Params")]
    public float fireRate = 1;
    public int maxConcurrentBullets = 10;

    [Header("Bullet Params")]
    public float damage = 1;
    public float speed = 1;
}
