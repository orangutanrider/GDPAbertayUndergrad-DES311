using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementParameters", menuName = "Player/MovementParameters")]
public class PlayerMovementParameters : ScriptableObject
{
    [Range(0f, 1f)]
    public float facingDirectionFlipThreshold = 0;

    [Header("xMovement")]
    public float movePower = 0.75f; // moveForce/moveAcceleration
    public float moveSpeed = 15; // maxMoveSpeed
    [Space]
    [Range(0f, 1f)]
    public float pivotTime = 0.1f;
    [Range(0f, 1f)]
    public float pivotInputThreshold = 0f;
    [Space]
    public AnimationCurve dragPowerCurve; // drag power against velocity

    [Header("Jump")]
    public float jumpPower = 16;
    public float jumpXPower = 1; // it scales with the x input and pushes the player in the direction of movement
    [Space]
    [Tooltip("When the jump key is released, their y velocity will be multiplied by this value")]
    [Range(-1f, 1f)]
    public float jumpReleaseVelocityMultiply = 0;
    [Space]
    [Range(0f, 1f)]
    public float jumpBufferTime = 0.25f;
    [Range(0f, 1f)]
    public float jumpCoyoteTime = 0.1f;

    [Header("Air Physics")]
    public float baseGravity = 3;
    public float peakGravity = 6;
    public float maximumFallSpeed = 35;
}
