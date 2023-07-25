using UnityEngine;

public class EEmitter : MonoBehaviour
{
    [Header("Required References")]
    public EBulletPool bulletPool;

    public void Emit()
    {
        EBullet emittedBullet = bulletPool.GetPooledBullet();
        emittedBullet.transform.position = transform.position;
        emittedBullet.transform.up = transform.up;
        emittedBullet.ActivateAll();
    }
}
