using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGizmosDisplay : MonoBehaviour
{
    public bool active = true;

    [Header("Data References (nullable)")]
    [SerializeField] PlayerRaycastParameters raycastParameters;
    [SerializeField] PlayerGoombaStompParameters stompParameters;

    [Header("Component References (nullable)")]
    [Tooltip("Add to have the system listen to the player's facing direction (and adjust accordingly).")]
    public PlayerMovement playerMovement;

    const float lineGizmoLength = 3f;

    void OnDrawGizmos()
    {
        if (active == false) { return; }
        PlayerRaycastParametersGizmos();
        PlayerGoombaStompParametersGizmos();
    }

    Vector3 FlipXPositionBasedOnFacingDirection(Vector3 position, FacingDirection facingDirection)
    {
        if (facingDirection == FacingDirection.Left)
        {
            position = new Vector3(-position.x, position.y);
        }
        return position;
    }

    #region PlayerRaycastParameters Gizmos
    void PlayerRaycastParametersGizmos()
    {
        if (raycastParameters == null) { return; }
        GroundCastGizmo();
        HeadCastGizmo();
        StompCastGizmo();
    }

    void GroundCastGizmo()
    {
        Vector3 position = raycastParameters.groundCheckPosition;
        Vector2 size = raycastParameters.groundCheckSize;

        if (playerMovement != null)
        {
            position = FlipXPositionBasedOnFacingDirection(position, playerMovement.PlayerFacingDirection);
        }
        position = position + transform.position;

        Gizmos.DrawWireCube(position, size);
    }

    void HeadCastGizmo()
    {
        Vector3 position = raycastParameters.headCheckPosition;
        Vector2 size = raycastParameters.headCheckSize;

        if (playerMovement != null)
        {
            position = FlipXPositionBasedOnFacingDirection(position, playerMovement.PlayerFacingDirection);
        }
        position = position + transform.position;

        Gizmos.DrawWireCube(position, size);
    }

    void StompCastGizmo()
    {
        Vector3 position = raycastParameters.stompCheckPosition;
        Vector2 size = raycastParameters.stompCheckSize;

        if (playerMovement != null)
        {
            position = FlipXPositionBasedOnFacingDirection(position, playerMovement.PlayerFacingDirection);
        }
        position = position + transform.position;
        position = new Vector3(position.x, position.y - (size.y * 0.5f));

        Gizmos.DrawWireCube(position, size);
    }
    #endregion

    #region PlayerGoombaStompParameters Gizmos
    void PlayerGoombaStompParametersGizmos()
    {
        if (stompParameters == null) { return; }
        StompPointOffsetGizmo();
    }

    void StompPointOffsetGizmo()
    {
        Vector3 stompPoint = transform.position + new Vector3(0, stompParameters.stompPointOffset);
        Vector3 from = stompPoint - new Vector3(lineGizmoLength/2, 0);
        Vector3 to = stompPoint + new Vector3(lineGizmoLength/2, 0);

        Gizmos.DrawLine(from, to);
    }
    #endregion
}
