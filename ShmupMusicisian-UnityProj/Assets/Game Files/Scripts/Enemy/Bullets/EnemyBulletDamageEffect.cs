using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletDamageEffect : EnemyBulletHitEffect
{
    [Header("Required References")]
    [SerializeField] EnemyBasicBulletParams bulletParams;

    public override void BulletPlayerHit(Collider2D collision)
    {
        PlayerStatusChanger playerStatus = collision.GetComponent<PlayerStatusChanger>();
        if (playerStatus == null) { return; }

        // Deal Damage
        AtPlayerDamageData damageData = new AtPlayerDamageData(bulletParams.damage * BulletBaseParams.damageMultiply, bulletMovement.MovementDirection, transform.position, bulletRoot);
        playerStatus.ApplyDamage(damageData);

        DeActivateBullet();
    }
}
