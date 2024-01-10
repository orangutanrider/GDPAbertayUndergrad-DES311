using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEnemyCollisionParameters", menuName = "Player/EnemyCollisionParameters")]
public class PlayerEnemyCollisionParameters : ScriptableObject
{
    public LayerMask collisionDamageLayerMask;

    [Header("Vulnerability Modifiers")]
    public float damageTakenModifier = 1;
    [Space]
    public float knockbackTakenModifier = 1;
    public float knockbackTakenYModifier = 1;
}
