using UnityEngine;
using System.Collections.Generic;

public class EInputEmitter : MonoBehaviour
{
    public EBulletPool bulletPool;

    [Header("Emitter Inputs")]
    public List<EInputEmitterAddOn> emitterAddOns = new List<EInputEmitterAddOn>();

    public delegate void BulletEmission(EBullet bullet , EInputEmitter sender);
    public event BulletEmission bulletEmission;

    private void Start()
    {
        foreach(EInputEmitterAddOn emitterAddOn in emitterAddOns)
        {
            emitterAddOn.AddToBulletEmissionEvent(this);
        }
    }

    public void Emit()
    {
        EBullet emittedBullet = bulletPool.GetPooledBullet();
        emittedBullet.gameObject.SetActive(true);
        emittedBullet.transform.position = transform.position;

        if (bulletEmission != null)
        {
            bulletEmission?.Invoke(emittedBullet, this);
        }
        else
        {
            Debug.LogWarning("InputEmitter bullet got emitted, but nothing is doing anything to the bullet emitted.");
            emittedBullet.RequestDeactivation();
        }
    }
}
