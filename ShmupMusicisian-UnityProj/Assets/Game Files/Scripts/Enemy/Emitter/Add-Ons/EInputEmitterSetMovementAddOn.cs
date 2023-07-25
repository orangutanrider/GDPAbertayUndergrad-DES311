using UnityEngine;

[CreateAssetMenu(fileName = "SetMovementAddOn", menuName = "EBullet/EmitterAddOn/SetMovementAddOn")]
public class EInputEmitterSetMovementAddOn : EInputEmitterAddOn
{
    public bool relativeToTransform = false;
    public Vector2 movementVector = Vector2.down;
    [Space]
    public float speedScale = 1;

    public override void AddToBulletEmissionEvent(EInputEmitter inputEmitter)
    {
        inputEmitter.bulletEmission += SetMovement;
    }

    void SetMovement(EBullet bullet, EInputEmitter sender)
    {
        if (relativeToTransform == false)
        {
            bullet.movement.StartMovement(movementVector * speedScale);
        }
        else
        {
            //https://matthew-brett.github.io/teaching/rotation_2d.html

            // get transform rotation
            float rotationAmount = bullet.transform.eulerAngles.z;

            float x1 = movementVector.x; float y1 = movementVector.y;

            // get rotated vector
            float x2 = Mathf.Cos(rotationAmount) * x1 - Mathf.Sin(rotationAmount) * y1;
            float y2 = Mathf.Sin(rotationAmount) * x1 + Mathf.Cos(rotationAmount) * y1;

            bullet.movement.StartMovement(new Vector2(x2, y2) * speedScale);
        }
    }
}
