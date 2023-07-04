using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicEmitter : EnemyBulletEmitter
{
    [Header("Required References")]
    [SerializeField] EnemyBasicEmitterParams emitterParams;

    const string heirarchyObjectName = "EnemyBasicEmitter Bullets";

    public override void Start()
    {
        CreateHeirarchyObject(heirarchyObjectName);

        // get params
        List<WeightedBulletPrefab> bulletPrefabs = emitterParams.BulletPalette.bulletPrefabs;

        // spawn bullet pool
        for (int prefabLoop = 0; prefabLoop < bulletPrefabs.Count; prefabLoop++)
        {
            for (int spawnLoop = 0; spawnLoop <= EmitterBaseParams.maxConcurrentBullets / bulletPrefabs.Count; spawnLoop++)
            {
                SpawnNewBulletIntoPool(bulletPrefabs[prefabLoop].BulletPrefab);
            }
        }
    }

    public override void Update()
    {
        EmissionTimerUpdate();

        if (activeAndFiring == false || EmissionTimer < emitterParams.emissionInterval) { return; }

        Emit();
        EmissionTimer = 0;
    }
}
