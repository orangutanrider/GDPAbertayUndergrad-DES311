using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyCollisionDamage : MonoBehaviour
{
    public bool active = true;

    [Header("Required References")]
    [SerializeField] PlayerEnemyCollisionParameters playerCollisionParameters;
    public PlayerStatusChanger playerStatusChanger;
    public Collider2D playerCollider;

    bool isOverrided = false;
    public bool IsOverrided
    {
        get
        {
            return isOverrided;
        }
        set
        {
            if (value == false)
            {
                Debug.Log("The override cannot be de-activated, doing so would likely break the scripts that overrided it");
                return;
            }

            isOverrided = value;
        }
    }

    const float defaultEnemyCollisionDamage = 1f;
    const float defaultEnemyCollisionKnockbackX = 1f;
    const float defaultEnemyCollisionKnockbackY = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isOverrided == true) { return; }

        // (if layer mask doesn't contain the collision object's layer, then return)
        LayerMask collisionDamageLayerMask = playerCollisionParameters.collisionDamageLayerMask;
        if (collisionDamageLayerMask == (collisionDamageLayerMask | (1 << collision.gameObject.layer))) { }
        else { return; }
        // https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html

        TakeCollisionDamage(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOverrided == true) { return; }

        // (if layer mask doesn't contain the collision object's layer, then return)
        LayerMask collisionDamageLayerMask = playerCollisionParameters.collisionDamageLayerMask;
        if (collisionDamageLayerMask == (collisionDamageLayerMask | (1 << collision.gameObject.layer))) { }
        else { return; }
        // https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html

        TakeCollisionDamage(collision);
    }

    public void TakeCollisionDamage(Collider2D collision)
    {
        EnemyPlayerCollisionModifier modifier = null;
        bool modifierPresent = collision.gameObject.TryGetComponent(out modifier);
        if (modifierPresent == true)
        {
            // load modifier data
            float enemyDamage = modifier.CollisionModifierData.collisionDamage;
            bool enemyDoesTriggerIFrames = modifier.CollisionModifierData.collisionTriggersIFrames;
            float enemyKnockbackX = modifier.CollisionModifierData.collisionKnockback.x;
            float enemyKnockbackY = modifier.CollisionModifierData.collisionKnockback.y;
            bool enemyCasuesKnockbackStun = modifier.CollisionModifierData.collisionStunDuringKnockback;

            // apply
            ApplyCollisionDamage(collision, enemyDamage, enemyDoesTriggerIFrames);
            ApplyCollisionKnockback(collision, enemyKnockbackX, enemyKnockbackY, enemyCasuesKnockbackStun);
        }
        else
        {
            // (use defaults)
            ApplyCollisionDamage(collision);
            ApplyCollisionKnockback(collision);
        }
    }

    void ApplyCollisionDamage(Collider2D collision, float enemyDamage = defaultEnemyCollisionDamage, bool enemyDoesTriggerIFrames = true)
    {
        // load player vulnerabilities
        float damageTakenModifier = playerCollisionParameters.damageTakenModifier;

        // construct damage data
        float damage = enemyDamage * damageTakenModifier;
        Vector2 damageDirection = collision.bounds.center - playerCollider.bounds.center;
        GameObject damageSource = collision.gameObject;
        bool doesTriggerIFrames = enemyDoesTriggerIFrames;
        AtPlayerDamageData damageData = new AtPlayerDamageData(damage, damageDirection, damageSource, doesTriggerIFrames);

        // apply damage
        playerStatusChanger.ApplyDamage(damageData);
    }

    void ApplyCollisionKnockback(Collider2D collision, float enemyKnockbackX = defaultEnemyCollisionKnockbackX, float enemyKnockbackY = defaultEnemyCollisionKnockbackY, bool enemyCausesStunDuringKnockback = true)
    {
        // load player vulnerabilities
        float knockbackTakenModifier = playerCollisionParameters.knockbackTakenModifier;
        float knockbackTakenYModifier = playerCollisionParameters.knockbackTakenYModifier;

        // handle knockback vector
        Vector2 knockbackForce = new Vector2(enemyKnockbackX, enemyKnockbackY);

        // flip knockback vector, depending on what side of the source, that the player is at
        Vector2 knockbackDirection = (collision.bounds.center - playerCollider.bounds.center).normalized;
        if (knockbackDirection.x > 0)
        {
            knockbackForce.x = -knockbackForce.x;
        }

        // apply knockback vulnerabilities
        knockbackForce.y = knockbackForce.y * knockbackTakenYModifier;
        knockbackForce = knockbackForce * knockbackTakenModifier;

        // construct knockback data
        GameObject knockbackSource = collision.gameObject;
        bool stunDuringKnockback = enemyCausesStunDuringKnockback;
        AtPlayerKnockbackData knockbackData = new AtPlayerKnockbackData(knockbackForce, knockbackSource, stunDuringKnockback);

        // apply knockback
        playerStatusChanger.ApplyKnockback(knockbackData);
    }
}
