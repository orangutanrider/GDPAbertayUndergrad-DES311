using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerCollisionModifierParameters : ScriptableObject
{
    public float collisionDamage = 1;
    [Space]
    public Vector2 collisionKnockback = new Vector2(1, 1);
    [Space]
    public bool collisionStunDuringKnockback = true;
    public bool collisionTriggersIFrames = true;
}
