using UnityEngine;

public class EnemyBulletLinearMovement : EnemyBulletMovement, IEnemyBulletActivatable
{
    [Header("Required References")]
    [SerializeField] EnemyBasicBulletParams bulletParams;
    public Rigidbody2D rb2D;

    public override IEnemyBulletActivatable GetActivationInterface() { return this; }

    void IEnemyBulletActivatable.Activate()
    {
        rb2D.velocity = bulletParams.movementVector * BulletBaseParams.speedMultiply;
        UpdateBaseData(bulletParams.movementVector, rb2D.velocity);
    }
}
