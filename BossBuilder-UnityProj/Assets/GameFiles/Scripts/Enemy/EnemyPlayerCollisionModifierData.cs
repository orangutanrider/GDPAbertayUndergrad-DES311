using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyPlayerCollisionModifierData
{
    public float collisionDamage;

    public Vector2 collisionKnockback;

    public bool collisionStunDuringKnockback;
    public bool collisionTriggersIFrames;

    public EnemyPlayerCollisionModifierData(float _collisionDamage, Vector2 _collisionKnockback, bool _collisionStunDuringKnockback = true, bool _collisionTriggersIFrames = true)
    {
        collisionDamage = _collisionDamage;

        collisionKnockback = _collisionKnockback;

        collisionStunDuringKnockback = _collisionStunDuringKnockback;
        collisionTriggersIFrames = _collisionTriggersIFrames;
    }
}
