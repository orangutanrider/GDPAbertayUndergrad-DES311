using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBasicBulletParams", menuName = "Enemy/BasicBulletParams")]
public class EnemyBasicBulletParams : ScriptableObject
{
    public bool debugMode = false;

    [Header("Bullet Params")]
    public Vector2 movementVector = Vector2.down;
    public float damage = 1;
}
