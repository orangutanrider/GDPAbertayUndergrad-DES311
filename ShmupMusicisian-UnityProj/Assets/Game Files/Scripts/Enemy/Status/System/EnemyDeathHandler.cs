using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDeathHandler : MonoBehaviour
{
    [Header("Required References")]
    public GameObject enemyRoot; // temp variable for temp death soloution of destroying the root object

    public void ExecuteDeath()
    {
        // temp destroy enemy
        Destroy(enemyRoot);
    }
}
