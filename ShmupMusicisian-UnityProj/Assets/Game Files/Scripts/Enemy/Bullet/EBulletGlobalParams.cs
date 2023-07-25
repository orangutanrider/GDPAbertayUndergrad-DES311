using UnityEngine;

[CreateAssetMenu(fileName = "EBulletGlobalParams", menuName = "EBullet/Global")]
public class EBulletGlobalParams : ScriptableObject
{
    public bool printNegativeFlagWarnings;
    public bool allowNegativeFlags;
    [Space]
    public float speedMultiply;
    public float bulletDamage; // this is a compromise, see EBulletHitDamage definition for more information
    [Space]
    public LayerMask playerMask;
    public LayerMask bulletBoundaryMask;
}