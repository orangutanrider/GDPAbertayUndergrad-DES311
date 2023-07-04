using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBulletPalette", menuName = "Enemy/BulletPalette")]
public class EnemyBulletPalette : ScriptableObject
{
    public List<WeightedBulletPrefab> bulletPrefabs = new List<WeightedBulletPrefab>();
}
