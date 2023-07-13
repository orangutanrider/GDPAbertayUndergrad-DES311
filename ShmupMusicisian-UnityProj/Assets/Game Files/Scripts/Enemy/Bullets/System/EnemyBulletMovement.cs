using UnityEngine;

public abstract class EnemyBulletMovement : EnemyBulletComponent
{
    // All this class wants from it's inheriters is for them to update its data and to implement the speedMultiply into their movement equations
    // The parameters that need updating are MovementDirection and Velocity
    // There is a function UpdateBaseData() to make this clear

    [Header("(Base) Required References")]
    [SerializeField] EnemyBulletBaseParams bulletBaseParams;

    // Implement the speedMultiply from the base params so that groups of bullets can be edited and adjusted all at once
    public EnemyBulletBaseParams BulletBaseParams
    {
        get { return bulletBaseParams; }
    }

    // Use this to represent the overall direction of movement
    // An example of what is meant by this: Say you had a bullet moving along a sine wave pattern, and the sine wave was moving down
    // Then this would just be equal to down
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

    // Use this to represent the real velocity of the bullet
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
    
    // This is just here to make it simple
    // As all this class really needs from the other script is for them to update these values and to use the speed multiply
    protected void UpdateBaseData(Vector2 _movementDirection, Vector2 _velocity)
    {
        movementDirection = _movementDirection;
        velocity = _velocity;
    }
}
