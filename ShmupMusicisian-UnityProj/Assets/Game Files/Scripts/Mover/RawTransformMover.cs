using System;
using UnityEngine;

public class RawTransformMover : Mover
{
    public float moveSpeed = 1;

    Vector2 currentMoveSpeed = Vector2.zero;
    const float speedAdjustment = 0.01f; // this is here because ideally all movers should move at a similar speed, when given the same moveInput

    public override event EventHandler OnMovementUpdated;

    public override Vector2 CurrentMovement 
    {
        get { return currentMoveSpeed; }
    }

    public override void OneShotMove(Vector2 moveInput)
    {
        moveInput = moveInput * moveSpeed;
        OnMovementUpdated?.Invoke(this, EventArgs.Empty);
        currentMoveSpeed = moveInput;
    }

    public override void UpdateMove(Vector2 moveInput)
    {
        moveInput = moveInput * moveSpeed;
        OnMovementUpdated?.Invoke(this, EventArgs.Empty);
        currentMoveSpeed = moveInput;
    }

    private void FixedUpdate()
    {
        transform.position = transform.position + (new Vector3(currentMoveSpeed.x, currentMoveSpeed.y) * speedAdjustment);
    }
}