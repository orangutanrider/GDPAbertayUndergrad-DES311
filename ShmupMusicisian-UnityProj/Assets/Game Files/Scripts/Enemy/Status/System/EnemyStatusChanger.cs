using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class EnemyStatusChanger : MonoBehaviour
{
    [Header("Component Refs (nullable)")]
    public EnemyHealthStatus healthStatus;

    public void ApplyDamage(AtEnemyDamageData damageData)
    {
        if(healthStatus == null) { return; }
        healthStatus.Health -= damageData.damageAmount;
    }
}
