using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBasicEmitterParams", menuName = "Enemy/BasicEmitterParams")]
public class EnemyBasicEmitterParams : ScriptableObject
{
    [SerializeField] GameObject bulletPrefab;
    public float emissionRate = 1;
    public string heirarchyObjectName = "BasicEmitter";

    public GameObject BulletPrefab
    {
        get { return bulletPrefab; }
    }
}
