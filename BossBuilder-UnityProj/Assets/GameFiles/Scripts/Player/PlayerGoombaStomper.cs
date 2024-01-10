using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Windows;

public class PlayerGoombaStomper : MonoBehaviour
{
    public bool active = true;

    [Header("Override (nullable)")]
    [Tooltip(
        "(Give a reference to override it)" +
        "\r\n\r\nIf you have a goomba stomper and collision damage, the stomper needs to override the collision damage, for both systems to work properly." +
        "\r\n\r\nThe override will allow this script to do all the detection for enemy collisions, that also means that the raycast box needs to encapsulate the player completely." +
        "\r\n\r\nTo differentiate between what is and isn't a goomba stomp (when this is doing all the detection) use the stomp point offset. " +
        "That field determines a Y position (relative to the player position) at which hits below will be considered as goomba stomps; hits above it will be considered as enemies colliding into the player.")]
    public PlayerEnemyCollisionDamage playerEnemyCollisionDamage;

    [Header("Required References")]
    public PlayerMovement playerMovement;
    public Rigidbody2D rb2D;
    [SerializeField] PlayerGoombaStompParameters stompParameters;
    [SerializeField] PlayerRaycastParameters raycastParameters;

    const int numOfPastPositionsToTrack = 5;
    List<Vector3> playerPastPositions = new List<Vector3>();

    private void Awake()
    {
        playerEnemyCollisionDamage.IsOverrided = true;
    }

    private void Update()
    {
        CheckForEnemyHits();
    }

    private void FixedUpdate()
    {
        RecordPlayerPositions();
    }

    RaycastHit2D StompableCast()
    {
        Vector3 position = raycastParameters.stompCheckPosition;
        Vector2 size = raycastParameters.stompCheckSize;
        LayerMask mask = raycastParameters.stompableMask;

        if (playerMovement.PlayerFacingDirection == FacingDirection.Left)
        {
            position = new Vector3(-position.x, position.y);
        }
        position = position + transform.position;

        return Physics2D.BoxCast(position, new Vector2(size.x, 0), 0, Vector2.down, size.y, mask);
    }

    void CheckForEnemyHits()
    {
        RaycastHit2D stompCheck = StompableCast();
        if (stompCheck == false) { return; }

        float yOffset = stompParameters.stompPointOffset;
        foreach(Vector3 playerPosition in playerPastPositions)
        {
            if(stompCheck.point.y <= playerPosition.y + yOffset) 
            {
                GoombaStompJump();
                GoombaStompDamage(stompCheck);
            }
            else if(playerEnemyCollisionDamage != null)
            {
                playerEnemyCollisionDamage.TakeCollisionDamage(stompCheck.collider);
            }
        }
    }

    void RecordPlayerPositions() // fixed update function
    {
        playerPastPositions.Add(transform.position);
        if(playerPastPositions.Count > numOfPastPositionsToTrack)
        {
            playerPastPositions.RemoveAt(0);
        }
    }

    void GoombaStompJump()
    {
        playerMovement.ResetFlags();

        // load paramaters
        float jumpPower = stompParameters.stompJumpPower;
        float jumpXPower = stompParameters.stompJumpXPower * playerMovement.XYInput.x;

        // execute jump
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        rb2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        rb2D.AddForce(new Vector2(jumpXPower, 0), ForceMode2D.Impulse);
    }

    void GoombaStompDamage(RaycastHit2D stompableHit)
    {

    }
}
