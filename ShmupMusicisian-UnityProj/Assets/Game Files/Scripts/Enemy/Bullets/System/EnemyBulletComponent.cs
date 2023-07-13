using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBulletComponent : MonoBehaviour
{
    public abstract IEnemyBulletActivatable GetActivationInterface();
}
