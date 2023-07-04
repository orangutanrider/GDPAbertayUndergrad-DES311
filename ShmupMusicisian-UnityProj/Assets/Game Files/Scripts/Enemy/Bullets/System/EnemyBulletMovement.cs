using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBulletMovement : MonoBehaviour
{
    [Header("(Base) Required References")]
    [SerializeField] EnemyBulletBaseParams bulletBaseParams;
    public Rigidbody2D rb2D;

    public const float fallbackSpeed = 1;

    public EnemyBulletBaseParams BulletBaseParams
    {
        get { return bulletBaseParams; }
    }

    public Vector2 MovementDirection
    {
        get
        {
            return movementDirection;
        }
        set
        {
            movementDirection = value;
        }
    }
    Vector2 movementDirection = Vector2.down;

    public virtual void OnEnable()
    {
        BulletMovement();
    }

    public virtual void BulletMovement()
    {
        // it is down to the dev to abbide and implement the speedMultiply parameter, though this isn't to say there are no scenarios where it shouldn't be implemented
        rb2D.velocity = movementDirection * fallbackSpeed * bulletBaseParams.speedMultiply;
    }
}
