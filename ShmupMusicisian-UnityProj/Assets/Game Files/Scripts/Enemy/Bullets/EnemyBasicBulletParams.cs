using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBasicBulletParams", menuName = "Enemy/BasicBulletParams")]
public class EnemyBasicBulletParams : ScriptableObject
{
    public Vector2 movementVector = Vector2.down;
    public float damage = 1;
}
