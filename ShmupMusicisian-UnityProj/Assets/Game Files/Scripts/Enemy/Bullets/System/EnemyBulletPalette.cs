using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBulletPalette", menuName = "Enemy/BulletPalette")]
public class EnemyBulletPalette : ScriptableObject
{
    [System.Serializable]
    public class WeightedBulletPrefab
    {
        [SerializeField] GameObject bulletPrefab;
        [Range(0f, 1f)]
        public float weight = 1;

        public GameObject BulletPrefab
        {
            get
            {
                return bulletPrefab;
            }
        }

        public WeightedBulletPrefab(GameObject _bulletPrefab, float _weight)
        {
            bulletPrefab = _bulletPrefab;
            weight = _weight;
        }
    }

    public List<WeightedBulletPrefab> bulletPrefabs = new List<WeightedBulletPrefab>();
}
