using UnityEngine;

public class EnemyBulletLinearMovement : EnemyBulletMovement, IEnemyBulletActivatable
{
    [Header("Required References")]
    [SerializeField] EnemyBasicBulletParams bulletParams;
    public Rigidbody2D rb2D;

    public override IEnemyBulletActivatable GetActivationInterface() { return this; }
    public override IEnemyBulletOnDeactivating GetOnDeactivatingInterface() { return null; }
    public override IEnemyBulletOnDeactivate GetOnDeactivationInterface() { return null; }

    void IEnemyBulletActivatable.Activate()
    {
        rb2D.velocity = bulletParams.movementVector * BulletBaseParams.speedMultiply;
        UpdateBaseData(bulletParams.movementVector, rb2D.velocity);
    }
}
