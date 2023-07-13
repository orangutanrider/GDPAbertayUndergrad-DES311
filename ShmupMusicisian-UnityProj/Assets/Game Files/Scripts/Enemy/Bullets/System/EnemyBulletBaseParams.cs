using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBulletBaseParams", menuName = "Enemy/BulletBaseParams")]
public class EnemyBulletBaseParams : ScriptableObject
{
    // Even though there's a damage and a speed param in here
    // It is preferable to create additional damage and speed params inside the tailored scriptable objects, created for custom bullets
    // As the intention isn't for the dev to create one of these per bullet type they create, though you can do this if you want.

    public float damageMultiply = 1;
    public float speedMultiply = 1;
    public LayerMask playerMask;
    public LayerMask bulletBoundaryMask;
}
