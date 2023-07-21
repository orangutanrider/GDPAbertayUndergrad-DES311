using UnityEngine;

public class EEmitter : MonoBehaviour
{
    [Header("Required References")]
    public EBulletPool bulletPool;

    public virtual void Emit(object emissionCaller)
    {
        EBullet emittedBullet = bulletPool.GetPooledBullet();
        emittedBullet.transform.position = transform.position;
        emittedBullet.ActivateAll();
    }
}
