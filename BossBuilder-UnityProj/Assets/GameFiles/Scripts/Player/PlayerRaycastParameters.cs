using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRaycastParameters", menuName = "Player/RaycastParameters")]
public class PlayerRaycastParameters : ScriptableObject
{
    [Header("PlayerMovement Raycasts")]
    public LayerMask groundMask;
    public Vector3 groundCheckPosition = new Vector3(0, 0);
    public Vector2 groundCheckSize = new Vector2(0.77f, 0.1f);
    [Space]
    public LayerMask headBumpMask;
    public Vector3 headCheckPosition = new Vector3(0, 1.6666f);
    public Vector2 headCheckSize = new Vector2(0.77f, 0.1f);
    [Space]
    public LayerMask stompableMask;
    public Vector3 stompCheckPosition = new Vector3(0, 1.6666f);
    public Vector2 stompCheckSize = new Vector2(0.77f, 0.1f);
}
