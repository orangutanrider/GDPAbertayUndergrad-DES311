using UnityEngine;

[CreateAssetMenu(fileName = "EBulletPoolParams", menuName = "Enemy/Emitter/PoolParams")]
public class EBulletPoolParams : ScriptableObject
{
    public GameObject bulletPrefab;
    [Space]
    public string heirarchyName = "UnnamedBulletPool";
    public int maxConcurrentBullets = 25;
    public Vector3 poolSpawnPoint = Vector3.zero;
    [Space]
    public bool printFails = false;
}
