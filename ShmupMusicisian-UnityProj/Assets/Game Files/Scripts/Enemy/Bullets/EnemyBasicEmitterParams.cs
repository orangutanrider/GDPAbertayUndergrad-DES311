using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBasicEmitterParams", menuName = "Enemy/BasicEmitterParams")]
public class EnemyBasicEmitterParams : ScriptableObject
{
    [Header("Emitter Params")]
    [SerializeField] EnemyBulletPalette bulletPalette;
    public float emissionInterval = 1;

    public EnemyBulletPalette BulletPalette
    {
        get { return bulletPalette; }
    }
}
