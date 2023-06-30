using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Required References")]
    public Rigidbody2D rb2D;
    public PlayerMovementParams playerParams;

    Vector2 xyInput = Vector2.zero;

    private void FixedUpdate()
    {
        XYMovement(xyInput);
    }

    // Input event
    public void XYMovementInput(InputAction.CallbackContext context)
    {
        xyInput = context.ReadValue<Vector2>();
    }

    void XYMovement(Vector2 input)
    {
        // get params
        float moveSpeed = playerParams.moveSpeed;

        // clamp
        input.x = Mathf.Clamp(input.x, -1, 1);
        input.y = Mathf.Clamp(input.y, -1, 1);

        // move
        rb2D.position = rb2D.position + (input * moveSpeed);
    }

    Vector3 Vector2ToVector3(Vector2 vector2)
    {
        return new Vector3(vector2.x, vector2.y, 0);
    }
}
