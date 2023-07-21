using UnityEngine;

public abstract class EBulletComponent : MonoBehaviour
{
    // OnActivate() is essentially the same as OnEnable(), it exists so a dev can use the OnActivate stack to determine the execution order of the components.
    // is nullable
    public abstract IEBulletOnActivate GetOnActivate();

    // OnDeactivate() is essentially the same as OnDisable(), it exists so a dev can use the OnDeactivate stack to determine the execution order of the components.
    // is nullable
    public abstract IEBulletOnDeactivate GetOnDeactivate();

    // OnDeactivating pings implementers when a deactivation request is recieved in the EBullet component
    // This is different from OnDeactivate as the EBullet will wait until there are no raised flags to disable the bullet.
    // is nullable
    public abstract IEBulletOnDeactivation GetOnDeactivation();
}
