using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthStatus : MonoBehaviour
{
    [ReadOnly]
    [SerializeField] float health = 0;

    [Header("Required References")]
    [SerializeField] PlayerStatusParams statusParams;
    public GameOverHandler gameOverHandler;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if(health <= 0)
            {
                gameOverHandler.TriggerGameOver();
            }
        }
    }

    private void Start()
    {
        health = statusParams.maxHealth;
    }

    private void OnValidate()
    {
        health = statusParams.maxHealth;
    }
}
