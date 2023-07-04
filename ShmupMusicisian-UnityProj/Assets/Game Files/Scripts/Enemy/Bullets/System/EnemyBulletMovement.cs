using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBulletMovement : MonoBehaviour
{
    [Header("(Base) Required References")]
    [SerializeField] EnemyBulletBaseParams bulletBaseParams;

    // Implement the speedMultiply from the base params so that groups of bullets can be edited and adjusted all at once

    public EnemyBulletBaseParams BulletBaseParams
    {
        get { return bulletBaseParams; }
    }

    // Use this to represent the overall direction of movement
    // What that means is that say you had a bullet moving along a sine wave pattern, and the sine wave was moving down
    // Then this would be equal to down
    public Vector2 MovementDirection
    {
        get
        {
            return movementDirection.normalized;
        }
        set
        {
            movementDirection = value;
        }
    }
    Vector2 movementDirection = Vector2.down;

    // use this to represent the velocity
    public Vector2 Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            velocity = value;
        }
    }
    Vector2 velocity = Vector2.down;
    
    // this is just here to make it simple (as all this class really needs from the other script is for them to update these values and to use the speed multiply)
    public void UpdateBaseData(Vector2 _movementDirection, Vector2 _velocity)
    {
        movementDirection = _movementDirection;
        velocity = _velocity;
    }
}
