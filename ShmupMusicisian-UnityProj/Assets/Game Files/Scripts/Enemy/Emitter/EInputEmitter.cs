
public class EInputEmitter : EEmitter
{
    public delegate EBullet BulletEmission(EInputEmitter sender, object emissionCaller);
    public event BulletEmission bulletEmission;

    public override void Emit(object emissionCaller)
    {
        EBullet emittedBullet = bulletPool.GetPooledBullet();
        emittedBullet.transform.position = transform.position;
        bulletEmission?.Invoke(this, emissionCaller);
    }
}
