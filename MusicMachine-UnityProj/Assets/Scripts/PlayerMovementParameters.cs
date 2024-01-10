using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementParameters", menuName = "Player/PlayerMovementParameters")]
public class PlayerMovementParameters : ScriptableObject
{
    [Header("Controls")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode auxLeftKey = KeyCode.LeftArrow;
    [Space]
    public KeyCode rightKey = KeyCode.D;
    public KeyCode auxRightKey = KeyCode.RightArrow;
    [Space]
    public KeyCode downKey = KeyCode.S;
    public KeyCode auxDownKey = KeyCode.DownArrow;
    [Space]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode auxJumpKey = KeyCode.W;

    [Header("xMovement")]
    public float moveSpeed = 0.75f;
    public float pivotTime = 0.1f;
    [Space]
    public float maxMoveSpeed = 15;

    [Header("Jumping and falling")]
    public float maxAcensionSpeed = 999;
    public float jumpStrength = 16;
    public float jumpReleaseVelocityNerf = 0.33f;
    [Space]
    public float fallAcceleration = 3;
    public float peakFallAcceleration = 6;
    public float maximumFallSpeed = 35;
    [Space]
    public float jumpBufferTime = 0.4f;
    public float jumpCoyoteTime = 0.1f;

    [Header("Raycast Checks")]
    public LayerMask groundCheckMask;
    public Vector3 groundCheckRelativePosition = new Vector3(0, 0);
    public Vector2 groundCheckSize = new Vector2(0.77f, 0.1f);
    [Space]
    public LayerMask bumpMask;
    public Vector3 headCheckRelativePosition = new Vector3(0, 1.6666f);
    public Vector2 headCheckSize = new Vector2(0.77f, 0.1f);
}
