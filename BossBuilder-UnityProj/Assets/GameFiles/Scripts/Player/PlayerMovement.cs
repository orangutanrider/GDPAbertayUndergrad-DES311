using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public bool active = true;

    [Header("Required References")]
    public Rigidbody2D rb2D;
    [SerializeField] PlayerMovementParameters movementParameters;
    [SerializeField] PlayerRaycastParameters raycastParameters;

    #region Variables
    public bool Stunned { get; set; }

    FacingDirection facingDirection = FacingDirection.Right;
    public FacingDirection PlayerFacingDirection
    {
        get { return facingDirection; }
        set { facingDirection = value; }
    }

    Vector2 xyInput = Vector2.zero;
    public Vector2 XYInput
    {
        get { return xyInput; }
    }

    Vector2 velocity = Vector2.zero;

    bool grounded = true; // true while the ground cast detects ground
    public bool Grounded
    {
        get { return grounded; }
    }

    bool leavingGrounded = false; // true when the player has initiated a jump, but the groundcast is still detecting ground
    public bool LeavingGrounded
    {
        get { return leavingGrounded; }
    }

    bool jumped = false; // true when the player has succesfully jumped, is reset by unsucessful jump inputs and is also reset by grounded state (unless leavingGrounded is true)
    bool jumpReleased = false;

    float jumpBufferTimer = 0;
    float jumpCoyoteTimer = 0;
    #endregion

    void Update()
    {
        CheckForGround();
        CheckForHeadBump();

        ProcessJumpInput();

        if (active == false) { return; }
    }

    void FixedUpdate()
    {
        if (active == false) { return; }

        XMovementForce();
        XMovementDrag();
        XMovementPivot();

        FallPhysics();
    }

    public void ResetFlags()
    {
        grounded = false;
        leavingGrounded = false;
        jumpReleased = false;
        jumped = false;
        jumpBufferTimer = 0;
        jumpCoyoteTimer = 0;
    }

    #region Inputs
    public void JumpInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                float jumpBuffer = movementParameters.jumpBufferTime;
                jumpBufferTimer = jumpBuffer;
                jumpReleased = false;
                jumped = false;
                break;
            case InputActionPhase.Canceled:
                JumpRelease();
                break;
        }
    }

    public void XYMovementInput(InputAction.CallbackContext context)
    {
        if(Stunned == true) { xyInput = Vector2.zero; return; }

        xyInput = context.ReadValue<Vector2>();

        if (active == false) { return; }

        float flipThreshold = movementParameters.facingDirectionFlipThreshold;
        if(xyInput.x > flipThreshold)
        {
            facingDirection = FacingDirection.Right;
            return;
        }
        if (xyInput.x < -flipThreshold)
        {
            facingDirection = FacingDirection.Left;
            return;
        }
    }
    #endregion

    #region Raycast Checks
    RaycastHit2D GroundCast()
    {
        Vector3 position = raycastParameters.groundCheckPosition;
        Vector2 size = raycastParameters.groundCheckSize;
        LayerMask mask = raycastParameters.groundMask;

        if (facingDirection == FacingDirection.Left)
        {
            position = new Vector3(-position.x, position.y);
        }
        position = position + transform.position;


        return Physics2D.BoxCast(position, size, 0, Vector2.zero, 0, mask);
    }

    RaycastHit2D HeadBumpCast()
    {
        Vector3 position = raycastParameters.headCheckPosition;
        Vector2 size = raycastParameters.headCheckSize;
        LayerMask mask = raycastParameters.headBumpMask;

        if (facingDirection == FacingDirection.Left)
        {
            position = new Vector3(-position.x, position.y);
        }
        position = position + transform.position;

        return Physics2D.BoxCast(position, size, 0, Vector2.zero, 0, mask);
    }

    void CheckForGround()
    {
        RaycastHit2D groundCheck = GroundCast();

        if (groundCheck == true)
        {
            grounded = true;

            if(leavingGrounded == false)
            {
                jumpReleased = false;
                jumped = false;
            }
        }
        else
        {
            grounded = false;
            leavingGrounded = false;
        }
    }

    void CheckForHeadBump()
    {
        RaycastHit2D headCheck = HeadBumpCast();

        if (headCheck == true)
        {
            leavingGrounded = false;
        }
    }
    #endregion

    #region Jump
    void Jump()
    {
        // flags
        jumpBufferTimer = 0;
        jumpCoyoteTimer = 0;
        leavingGrounded = true;
        jumped = true;

        // load paramaters
        float jumpPower = movementParameters.jumpPower;
        float jumpXPower = movementParameters.jumpXPower * xyInput.x;

        // execute jump
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        rb2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        rb2D.AddForce(new Vector2(jumpXPower, 0), ForceMode2D.Impulse);
    }

    void ProcessJumpInput()
    {
        jumpBufferTimer = jumpBufferTimer - Time.deltaTime;

        if (grounded == false)
        {
            jumpCoyoteTimer = jumpCoyoteTimer - Time.deltaTime;
        }
        else
        {
            float coyoteTime = movementParameters.jumpCoyoteTime;
            jumpCoyoteTimer = coyoteTime;
        }

        if(active == false || Stunned == true) { return; }
        if(jumpBufferTimer <= 0 || jumpCoyoteTimer <= 0 || leavingGrounded == true) { return; }

        Jump();
    }

    void JumpRelease()
    {
        if(jumped == false) { return; }
        if(jumpBufferTimer > 0 || grounded == true || rb2D.velocity.y <= 0) { return; }

        // nuke current upwards yVelocity via a multiply
        float yVelocityNuke = movementParameters.jumpReleaseVelocityMultiply;
        rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * yVelocityNuke);

        // activate peak fall velocity immediately
        jumpReleased = true;
    }

    void FallPhysics()
    {
        float peakGravity = movementParameters.peakGravity;
        float baseGravity = movementParameters.baseGravity;
        float maximumFallSpeed = movementParameters.maximumFallSpeed;

        // if you are falling you fall faster (or if jump is released)
        if (rb2D.velocity.y < 0 || jumpReleased == true)
        {
            rb2D.gravityScale = peakGravity;
        }
        else
        {
            rb2D.gravityScale = baseGravity;
        }

        if (Stunned == true)
        {
            rb2D.gravityScale = baseGravity;
        }

        // maximum fall speed clamp
        rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -maximumFallSpeed, float.MaxValue));
    }
    #endregion

    #region XMovement
    void XMovementForce()
    {
        // load parameters
        float movePower = movementParameters.movePower;
        float moveSpeed = movementParameters.moveSpeed;

        // push left
        if (xyInput.x < 0 && rb2D.velocity.x > moveSpeed * xyInput.x)
        {
            rb2D.AddForce(new Vector2(xyInput.x, 0) * movePower, ForceMode2D.Impulse);
        }

        // push right
        if (xyInput.x > 0 && rb2D.velocity.x < moveSpeed * xyInput.x)
        {
            rb2D.AddForce(new Vector2(xyInput.x, 0) * movePower, ForceMode2D.Impulse);
        }
    }

    void XMovementDrag()
    {
        float dragPower = movementParameters.dragPowerCurve.Evaluate(Mathf.Abs(rb2D.velocity.x));
        if(dragPower == 0) { return; }

        float dragDirection = 1;
        if(rb2D.velocity.x > 0) { dragDirection = -1; }
        rb2D.AddForce(new Vector2(dragPower * dragDirection, 0), ForceMode2D.Impulse);
    }

    void XMovementPivot()
    {
        if (Stunned == true) { return; }

        float pivotTime = movementParameters.pivotTime;
        float pivotInputThreshold = movementParameters.pivotInputThreshold;

        // if input is 0
        if (xyInput.x < pivotInputThreshold && xyInput.x > -pivotInputThreshold)
        {
            rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, new Vector2(0, rb2D.velocity.y), ref velocity, pivotTime);
            return;
        }

        // if input is negative
        if (rb2D.velocity.x > 0 && xyInput.x < -pivotInputThreshold)
        {
            rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, new Vector2(0, rb2D.velocity.y), ref velocity, pivotTime);
            return;
        }

        // if input is positive
        if (rb2D.velocity.x < 0 && xyInput.x > pivotInputThreshold)
        {
            rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, new Vector2(0, rb2D.velocity.y), ref velocity, pivotTime);
            return;
        }
    }
    #endregion

    #region Debug
    readonly Color boolTrueColour = Color.green;
    readonly Color boolFalseColour = Color.red;

    public void DebugLeavingGrounded(SpriteRenderer spriteRenderer)
    {
        if (leavingGrounded == true)
        {
            spriteRenderer.color = boolTrueColour;
        }
        else
        {
            spriteRenderer.color = boolFalseColour;
        }
    }

    public void DebugJumped(SpriteRenderer spriteRenderer)
    {
        if (jumped == true)
        {
            spriteRenderer.color = boolTrueColour;
        }
        else
        {
            spriteRenderer.color = boolFalseColour;
        }
    }

    public void DebugGrounded(SpriteRenderer spriteRenderer)
    {
        if (grounded == true)
        {
            spriteRenderer.color = boolTrueColour;
        }
        else
        {
            spriteRenderer.color = boolFalseColour;
        }
    }

    public void DebugJumpReleased(SpriteRenderer spriteRenderer)
    {
        if (jumpReleased == true)
        {
            spriteRenderer.color = boolTrueColour;
        }
        else
        {
            spriteRenderer.color = boolFalseColour;
        }
    }
    #endregion
}
