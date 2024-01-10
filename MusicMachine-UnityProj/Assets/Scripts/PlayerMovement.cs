using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool active = false;

    [Header("Required References")]
    public Rigidbody2D rb2D;

    [Header("Parameters")]
    public PlayerMovementParameters movementParameters;
    public bool gizmosOn = false;

    float gravityOverrideTimer = 0;
    float jumpBufferTimer = 0;
    float jumpCoyoteTimer = 0;
    float xInput = 0;
    bool grounded = false;
    bool exitedGround = false;
    bool breathingIn = false;
    bool jumped = false;

    Vector2 velocity = Vector2.zero;

    public bool Grounded
    {
        get { return grounded; }
    }

    public float GravityOverrideDuration
    {
        set
        {
            gravityOverrideTimer = value;
        }
    }

    #region Execution
    private void Update()
    {
        if (active == false)
        {
            return;
        }

        RaycastChecks();

        JumpInputAndBuffer();
        GetPlayerXInput();

        ReleaseAirPhysics();
        Jump();
    }

    private void FixedUpdate()
    {
        if (active == false)
        {
            return;
        }

        XdampBackTo0v();
        Xmove();
        AirPhysics();
    }

    private void OnDrawGizmos()
    {
        if (gizmosOn == false) { return; }

        Vector3 groundCheckRelativePosition = movementParameters.groundCheckRelativePosition;
        Vector2 groundCheckSize = movementParameters.groundCheckSize;
        Vector3 headCheckRelativePosition = movementParameters.headCheckRelativePosition;
        Vector2 headCheckSize = movementParameters.headCheckSize;

        Gizmos.DrawWireCube(transform.position + groundCheckRelativePosition, groundCheckSize); // ground check
        Gizmos.DrawWireCube(transform.position + headCheckRelativePosition, headCheckSize); // head check
    }
    #endregion

    #region xMovement
    private void GetPlayerXInput()
    {
        KeyCode leftKey = movementParameters.leftKey;
        KeyCode auxLeftKey = movementParameters.auxLeftKey;
        KeyCode rightKey = movementParameters.rightKey;
        KeyCode auxRightKey = movementParameters.auxRightKey;

        if (Input.GetKey(leftKey) == true || Input.GetKey(auxLeftKey) == true)
        {
            xInput = -1;
        }
        else if (Input.GetKey(rightKey) == true || Input.GetKey(auxRightKey) == true)
        {
            xInput = 1;
        }
        else
        {
            xInput = 0;
        }
    }

    private void Xmove()
    {
        float moveSpeed = movementParameters.moveSpeed;
        float maxMoveSpeed = movementParameters.maxMoveSpeed;

        if (xInput > 0 && rb2D.velocity.x < maxMoveSpeed * xInput)
        {
            rb2D.AddForce(new Vector2(xInput, 0) * moveSpeed, ForceMode2D.Impulse);
        }

        if (xInput < 0 && rb2D.velocity.x > maxMoveSpeed * xInput)
        {
            rb2D.AddForce(new Vector2(xInput, 0) * moveSpeed, ForceMode2D.Impulse);
        }
    }

    private void XdampBackTo0v()
    {
        float pivotTime = movementParameters.pivotTime;

        if (xInput == 0 && grounded == true)
        {
            rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, new Vector2(0, rb2D.velocity.y), ref velocity, pivotTime);
        }
        if (rb2D.velocity.x > 0 && xInput == -1)
        {
            rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, new Vector2(0, rb2D.velocity.y), ref velocity, pivotTime);
        }
        if (rb2D.velocity.x < 0 && xInput == 1)
        {
            rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, new Vector2(0, rb2D.velocity.y), ref velocity, pivotTime);
        }
    }
    #endregion

    #region Jumping and y movment
    private void RaycastChecks()
    {
        Vector3 groundCheckRelativePosition = movementParameters.groundCheckRelativePosition;
        Vector2 groundCheckSize = movementParameters.groundCheckSize;
        Vector3 headCheckRelativePosition = movementParameters.headCheckRelativePosition;
        Vector2 headCheckSize = movementParameters.headCheckSize;
        LayerMask groundCheckMask = movementParameters.groundCheckMask;
        LayerMask bumpMask = movementParameters.bumpMask;

        RaycastHit2D groundCheckData = Physics2D.BoxCast(transform.position + groundCheckRelativePosition, groundCheckSize, 0, Vector2.down, 0, groundCheckMask);
        RaycastHit2D headBumpData = Physics2D.BoxCast(transform.position + headCheckRelativePosition, headCheckSize, 0, Vector2.up, 0, bumpMask);

        if (groundCheckData.collider != null)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        if (headBumpData.collider != null)
        {
            exitedGround = false;
        }
    }

    private void JumpInputAndBuffer()
    {
        KeyCode downKey = movementParameters.downKey;
        KeyCode auxDownKey = movementParameters.auxDownKey;
        KeyCode jumpKey = movementParameters.jumpKey;
        float jumpBufferTime = movementParameters.jumpBufferTime;
        float jumpCoyoteTime = movementParameters.jumpCoyoteTime;

        if (Input.GetKey(downKey) == false && Input.GetKey(auxDownKey) == false && Input.GetKeyDown(jumpKey) == true)
        {
            jumpBufferTimer = jumpBufferTime;
        }
        if ((Input.GetKey(downKey) == true || Input.GetKey(auxDownKey) == true) && Input.GetKeyDown(jumpKey) == true)
        {
            jumpBufferTimer = 0;
        }

        if (grounded == true && exitedGround == false)
        {
            jumpCoyoteTimer = jumpCoyoteTime;
        }

        // the timers constantly decrease
        if (jumpBufferTimer > 0)
        {
            jumpBufferTimer = jumpBufferTimer - Time.deltaTime;
        }

        // coyote timer decreases while the player isn't grounded
        if (grounded == false)
        {
            jumpCoyoteTimer = jumpCoyoteTimer - Time.deltaTime;
            exitedGround = false;
        }
    }

    private void Jump()
    {
        if (jumpCoyoteTimer > 0 && jumpBufferTimer > 0 && exitedGround == false)
        {
            float jumpStrength = movementParameters.jumpStrength;

            rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
            rb2D.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);

            jumpBufferTimer = 0;
            jumpCoyoteTimer = 0;
            exitedGround = true;
            jumped = true;
        }
    }

    void ReleaseAirPhysics()
    {
        KeyCode jumpKey = movementParameters.jumpKey;
        float jumpReleaseVelocityNerf = movementParameters.jumpReleaseVelocityNerf;

        if (jumped == true && rb2D.velocity.y > 0 && Input.GetKeyUp(jumpKey) == true)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, rb2D.velocity.y * jumpReleaseVelocityNerf);
        }

        if (rb2D.velocity.y < 0 || Input.GetKeyUp(jumpKey) == true)
        {
            jumped = false;
        }
    }

    private void AirPhysics()
    {
        KeyCode jumpKey = movementParameters.jumpKey;
        float peakFallAcceleration = movementParameters.peakFallAcceleration;
        float fallAcceleration = movementParameters.fallAcceleration;
        float maximumFallSpeed = movementParameters.maximumFallSpeed;
        float maxAcensionSpeed = movementParameters.maxAcensionSpeed;

        if (gravityOverrideTimer > 0)
        {
            gravityOverrideTimer = gravityOverrideTimer - Time.deltaTime;
        }

        // if you are falling you fall faster
        if ((rb2D.velocity.y < 0 || Input.GetKey(jumpKey) == false) && breathingIn == false && gravityOverrideTimer <= 0)
        {
            rb2D.gravityScale = peakFallAcceleration;
        }
        else
        {
            rb2D.gravityScale = fallAcceleration;
        }

        // maximum fall speed clamp
        rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, -maximumFallSpeed, maxAcensionSpeed));
    }

    void ClearJumpFlags()
    {
        jumpBufferTimer = 0;
        jumpCoyoteTimer = 0;
        jumped = false;
        exitedGround = false;
    }
    #endregion
}
