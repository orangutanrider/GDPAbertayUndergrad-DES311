using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebugger : MonoBehaviour
{
    [Header("Required References")]
    [Tooltip("This sprite renderer will be used to show states via the colour parameter.")]
    public SpriteRenderer debugSpriteRenderer;

    [Header("Component References (nullable)")]
    public PlayerMovement playerMovement;

    [Header("Debug Settings")]
    [SerializeField] PlayerMovementStates movementStateDebug;
    enum PlayerMovementStates
    {
        NONE,
        grounded,
        leavingGrounded,
        jumped,
        jumpReleased
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovementStateDebug();
    }

    void PlayerMovementStateDebug()
    {
        if(playerMovement == null || movementStateDebug != PlayerMovementStates.NONE) 
        {
            return; 
        }

        switch (movementStateDebug)
        {
            case PlayerMovementStates.grounded:
                playerMovement.DebugGrounded(debugSpriteRenderer);
                break;
            case PlayerMovementStates.leavingGrounded:
                playerMovement.DebugLeavingGrounded(debugSpriteRenderer);
                break;
            case PlayerMovementStates.jumped:
                playerMovement.DebugJumped(debugSpriteRenderer);
                break;
            case PlayerMovementStates.jumpReleased:
                playerMovement.DebugJumpReleased(debugSpriteRenderer);
                break;
        }
    }
}
