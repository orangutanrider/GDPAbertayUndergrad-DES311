using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusChanger : MonoBehaviour
{
    [Header("Component Refs (nullable)")]
    public PlayerHealthStatus healthStatus;

    public void ApplyDamage(AtPlayerDamageData damageData)
    {
        if (healthStatus == null) { return; }
        healthStatus.Health -= damageData.damageAmount;
    }
}
