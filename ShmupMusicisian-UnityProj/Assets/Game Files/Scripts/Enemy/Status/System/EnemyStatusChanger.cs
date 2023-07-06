using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStatusChanger : MonoBehaviour
{
    [Header("Hitbox Objects")]
    public List<EnemyHittable> hittables;

    [Header("Component Refs (nullable)")]
    public EnemyHealthStatus healthStatus;

    public EnemyHitManager HitManager { get; set; }

    private void Awake()
    {
        foreach (EnemyHittable hittable in hittables)
        {
            hittable.StatusChanger = this;
        }
    }

    public virtual void ApplyDamage(AtEnemyDamageData damageData)
    {
        if(healthStatus == null) { return; }
        healthStatus.Health -= damageData.damageAmount;
    }
}
