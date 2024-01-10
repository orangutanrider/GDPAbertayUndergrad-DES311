using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusChanger : MonoBehaviour
{
    [Header("Component References (nullable)")]
    public PlayerHealthStatus playerHealthStatus;
    public PlayerStunStatus playerStunStatus;

    // PlayerHealth
    public void ApplyDamage(AtPlayerDamageData damageData)
    {
        if (playerHealthStatus == null) { return; }

        playerHealthStatus.HealthPoints = playerHealthStatus.HealthPoints - damageData.damageAmount;
    }

    // PlayerStunStatus
    public void ApplyStun(AtPlayerStunData stunData)
    {
        if (playerStunStatus == null) { return; }

        if (stunData.stunDuration > playerStunStatus.StunTimer)
        {
            playerStunStatus.StunTimer = stunData.stunDuration;
        }
    }

    public void ApplyKnockback(AtPlayerKnockbackData knockbackData)
    {
        if (playerStunStatus == null) { return; }

        bool airborneStunImmunity = playerStunStatus.StatusParameters.knockbackStunImmunity;
        if (airborneStunImmunity == false)
        {
            playerStunStatus.StunDuringAirborne = knockbackData.stunDuringAirborne;
        }

        playerStunStatus.rb2D.velocity = Vector2.zero;
        playerStunStatus.rb2D.AddForce(knockbackData.knockbackForce);
    }
}
