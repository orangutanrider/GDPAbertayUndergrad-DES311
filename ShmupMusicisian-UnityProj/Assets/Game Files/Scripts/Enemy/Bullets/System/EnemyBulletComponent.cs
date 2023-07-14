using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBulletComponent : MonoBehaviour
{
    // Activate() is essentially the same as OnEnable(), the EnemyBulletActivator just allows the dev to manage the order in that they exectute.
    // is nullable
    public abstract IEnemyBulletActivatable GetActivationInterface();

    // OnDeactivate() is essentially the same as OnDisable(), the EnemyBulletDeactivationHandler just allows the dev to manage the order in that they exectute.
    // is nullable
    public abstract IEnemyBulletOnDeactivate GetOnDeactivationInterface();

    // OnDeactivating pings implementers when a deactivation request is recieved in the EnemyBulletDeactivationHandler.
    // This is different from OnDisable as the deactivation handler will wait until no flags are raised to disable the bullet.
    // is nullable
    public abstract IEnemyBulletOnDeactivating GetOnDeactivatingInterface();
}
