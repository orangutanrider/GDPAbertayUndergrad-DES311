using UnityEngine;

public class EnemyBasicEmitter : EnemyBulletEmitter
{
    [Header("Required References")]
    [SerializeField] EnemyBasicEmitterParams emitterParams;

    protected override float EmissionRate() { return emitterParams.emissionRate; }
    public override string HeirarchyObjectName() { return emitterParams.heirarchyObjectName; }

    protected override void SpawnBulletPool()
    {
        for (int loop = 0; loop <= EmitterBaseParams.maxConcurrentBullets; loop++)
        {
            SpawnNewBulletIntoPool(emitterParams.BulletPrefab);
        }
    }

    public override void Emit()
    {
        GameObject bulletBeingEmitted = GetPooledBullet();

        if (bulletBeingEmitted == null && EmitterBaseParams.printEmissionFails == true)
        {
            Debug.Log("The emitter on gameobject '" + gameObject.name + "' couldn't get a bullet from its pool");
            return;
        }
        if (bulletBeingEmitted == null)
        {
            return;
        }

        bulletBeingEmitted.transform.position = transform.position;
        bulletBeingEmitted.SetActive(true);
    }
}
