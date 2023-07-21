using UnityEngine;

public class EBulletMovement : EBulletComponent, IEBulletOnActivate
{
    [Header("Required Reference")]
    [SerializeField] EBulletGlobalParams globalParams;
    public Mover mover;

    public override IEBulletOnActivate GetOnActivate() { return this; }
    public override IEBulletOnDeactivate GetOnDeactivate() { return null; }
    public override IEBulletOnDeactivation GetOnDeactivation() { return null; }

    void IEBulletOnActivate.OnActivate()
    {
        StartMovement(transform.up * globalParams.speedMultiply);
    }

    public void StartMovement(Vector2 movementVector)
    {
        mover.OneShotMove(movementVector * globalParams.speedMultiply);
    }
}
