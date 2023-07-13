using UnityEngine;

public abstract class EnemyBulletHitEffect : EnemyBulletComponent
{
    [Header("(Base) Required References")]
    [SerializeField] EnemyBulletBaseParams bulletBaseParams;
    public GameObject bulletRoot;
    public EnemyBulletMovement bulletMovement;

    // Implement the damageMultiply from the base params so that groups of bullets can be edited and adjusted all at once

    #region Variables
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleBulletCollision(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleBulletCollision(collision.collider);
    }

    protected abstract void BulletPlayerHit(Collider2D collision);
    // Example implementation of a damage hit effect
    /*
        PlayerStatusChanger playerStatus = collision.GetComponent<PlayerStatusChanger>();
        if (playerStatus == null) { return; }

        DealDamage();
        DeActivateBullet();
    */

    public void DeActivateBullet()
    {
        // in the future this could ping a deactivation handler or something
        // cause there might be hit effects like an explosion or something 
        bulletRoot.SetActive(false);
    }

    CollisionResults HandleBulletCollision(Collider2D collision)
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
}
