using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBasicBulletParams", menuName = "Player/BasicBulletParams")]
public class BasicPlayerBulletParams : ScriptableObject
{
    [SerializeField] GameObject bulletPrefab;
    public LayerMask enemyMask;
    public LayerMask bulletBoundaryMask;

    [Header("Shooter Params")]
    public float shotInterval = 1;
    public int maxConcurrentBullets = 10;

    [Header("Bullet Params")]
    public float damage = 1;
    public float speed = 1;

    public GameObject BulletPrefab
    {
        get { return bulletPrefab; }
    }
}
