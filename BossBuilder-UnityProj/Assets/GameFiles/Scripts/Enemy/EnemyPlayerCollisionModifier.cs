using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerCollisionModifier : MonoBehaviour
{
    [SerializeField] EnemyPlayerCollisionModifierParameters collisionModifierParameters;
    public EnemyPlayerCollisionModifierData CollisionModifierData
    {
        get
        {
            float damage = collisionModifierParameters.collisionDamage;
            Vector2 knockback = collisionModifierParameters.collisionKnockback;
            bool causesStun = collisionModifierParameters.collisionStunDuringKnockback;
            bool triggersIFrames = collisionModifierParameters.collisionTriggersIFrames;

            return new EnemyPlayerCollisionModifierData(damage, knockback, causesStun, triggersIFrames);
        }
    }
}
