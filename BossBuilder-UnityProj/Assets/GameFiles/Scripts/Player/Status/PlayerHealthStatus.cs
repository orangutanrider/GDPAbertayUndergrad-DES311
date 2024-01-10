using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthStatus : MonoBehaviour
{
    [Header("Required References")]
    public PlayerStatusParameters statusParameters;

    public float HealthPoints
    {
        get
        {
            return healthPoints;
        }
        set
        {
            healthPoints = value;
            CheckForPlayerDeath();
        }
    }
    float healthPoints = 1;

    private void Awake()
    {
        healthPoints = statusParameters.maxHealth;
    }

    void PlayerDeath()
    {
        Debug.Log("GameOver");
        Destroy(gameObject);
    }

    void CheckForPlayerDeath()
    {
        if (healthPoints <= 0)
        {
            PlayerDeath();
        }
    }
}
