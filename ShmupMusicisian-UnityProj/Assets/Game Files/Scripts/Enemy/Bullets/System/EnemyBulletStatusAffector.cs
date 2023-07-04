using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class EnemyBulletStatusAffector : MonoBehaviour
{
    [Header("(Base) Required References")]
    [SerializeField] EnemyBulletBaseParams bulletBaseParams;
    public GameObject bulletRoot;
    public EnemyBulletMovement bulletMovement;

    #region Variables
    public const float fallbackDamageAmount = 1;
    public EnemyBulletBaseParams BulletBaseParams
    {
        get { return bulletBaseParams; }
    }

    public GameObject BulletSourceObject { get; set; }

    public enum CollisionResults
    {
        PlayerHit,
        BoundaryHit,
        MiscHit
    }
    #endregion

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        HandleBulletCollision(collision);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        HandleBulletCollision(collision.collider);
    }

    public virtual void BulletPlayerHit(Collider2D collision)
    {
        PlayerStatusChanger playerStatus = collision.GetComponent<PlayerStatusChanger>();
        if (playerStatus == null) { return; }

        ApplyDamage(fallbackDamageAmount, playerStatus);

        DeActivateBullet();
    }

    public virtual void ApplyDamage(float damageAmount, PlayerStatusChanger playerStatus)
    {
        float damage = damageAmount * bulletBaseParams.damageMultiply;
        AtPlayerDamageData damageData = new AtPlayerDamageData(damage, transform.position, bulletMovement.rb2D.velocity, BulletSourceObject);
        playerStatus.ApplyDamage(damageData);
    }

    public CollisionResults HandleBulletCollision(Collider2D collision)
    {
        // get params
        LayerMask playerMask = bulletBaseParams.playerMask;
        LayerMask bulletBoundaryMask = bulletBaseParams.bulletBoundaryMask;

        int layer = collision.gameObject.layer;

        // if player hit
        if ((playerMask & (1 << layer)) != 0)
        {
            BulletPlayerHit(collision);
            return CollisionResults.PlayerHit;
        }
        // if boundary hit
        if ((bulletBoundaryMask & (1 << layer)) != 0)
        {
            DeActivateBullet();
            return CollisionResults.BoundaryHit;
        }
        // if other hit
        return CollisionResults.MiscHit;
    }

    public CollisionResults ReturnBulletCollisionResults(Collider2D collision)
    {
        // get params
        LayerMask playerMask = bulletBaseParams.playerMask;
        LayerMask bulletBoundaryMask = bulletBaseParams.bulletBoundaryMask;

        int layer = collision.gameObject.layer;

        // if player hit
        if ((playerMask & (1 << layer)) != 0)
        {
            return CollisionResults.PlayerHit;
        }
        // if boundary hit
        if ((bulletBoundaryMask & (1 << layer)) != 0)
        {
            return CollisionResults.BoundaryHit;
        }
        // if other hit
        return CollisionResults.MiscHit;
    }

    public void DeActivateBullet()
    {
        bulletRoot.SetActive(false);
    }
}
