using UnityEngine;

public class EBulletHitDamage : EBulletComponent
{
    // This script is partially comporomised. Originally it was going to be EBulletHitEffect.
    // I didn't make it this because doing that would entail refactoring the Status system, due to the architecture that I'm trying to create in these componenets.
    // Though I kind of want to, It isn't worth re-factoring that system, I need to just finish the project for now.

    [Header("Required References")]
    [SerializeField] EBulletGlobalParams eBulletGlobalParams;

    [Header("Nullable Required")]
    public EBullet bullet;
    public Mover mover;

    public override IEBulletOnActivate GetOnActivate() { return null; }
    public override IEBulletOnDeactivate GetOnDeactivate() { return null; }
    public override IEBulletOnDeactivation GetOnDeactivation() { return null; }

    public enum CollisionResults
    {
        PlayerHit,
        BoundaryHit,
        MiscHit
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleBulletCollision(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleBulletCollision(collision.collider);
    }

    private void BulletPlayerHit(Collider2D collision)
    {
        PlayerStatusChanger playerStatus = collision.GetComponent<PlayerStatusChanger>();
        if (playerStatus == null) { return; }

        // Construct damage data
        float damage = eBulletGlobalParams.bulletDamage;
        Vector2 damageDirection = Vector2.zero;
        if(mover != null) 
        { 
            damageDirection = mover.CurrentMovement; 
        }
        Vector3 damageLocation = transform.position;
        GameObject damageSource = gameObject;
        if(bullet != null)
        {
            damageSource = bullet.gameObject;
        }

        AtPlayerDamageData damageData = new AtPlayerDamageData(damage, damageDirection, damageLocation, damageSource);

        // Apply Damage
        playerStatus.ApplyDamage(damageData);

        bullet.RequestDeactivation();
    }

    CollisionResults HandleBulletCollision(Collider2D collision)
    {
        // get params
        LayerMask playerMask = eBulletGlobalParams.playerMask;
        LayerMask bulletBoundaryMask = eBulletGlobalParams.bulletBoundaryMask;

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
            bullet.RequestDeactivation();
            return CollisionResults.BoundaryHit;
        }
        // if other hit
        return CollisionResults.MiscHit;
    }

    public CollisionResults ReturnBulletCollisionResults(Collider2D collision)
    {
        // get params
        LayerMask playerMask = eBulletGlobalParams.playerMask;
        LayerMask bulletBoundaryMask = eBulletGlobalParams.bulletBoundaryMask;

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