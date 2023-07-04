using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyHealthStatus : MonoBehaviour
{
    [ReadOnly]
    [SerializeField] float health = 0;

    [Header("Required References")]
    [SerializeField] EnemyStatusParams statusParams;

    [Header("Component Refs (nullable)")]
    public EnemyDeathHandler deathHandler;

    public EnemyStatusParams StatusParams
    {
        get { return statusParams; }
    }

    public virtual float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;

            if (health <= 0 && deathHandler != null)
            {
                deathHandler.ExecuteDeath();
            }
        }
    }

    public virtual void Start()
    {
        health = statusParams.maxHealth;

        if(deathHandler == null)
        {
            Debug.Log(gameObject.name + " has no death handler, if health drops below 0 this enemy will not die.");
        }
    }

    private void OnValidate()
    {
        if(statusParams != null)
        {
            health = statusParams.maxHealth;
        }
    }
}
