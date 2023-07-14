using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBulletDeactivationHandlerParams", menuName = "Enemy/BulletDeactivationHandlerParams")]
public class EnemyBulletDeactivationHandlerParams : ScriptableObject
{
    public bool printNegativeFlagWarnings = true;
    public bool allowNegativeFlags = false;
}
