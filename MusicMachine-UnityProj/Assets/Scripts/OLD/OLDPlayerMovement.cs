using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("OLD")]
public class OLDPlayerMovement : MonoBehaviour
{
    /*
    [Header("Required References")]
    public PlayerSpritesHandler spritesHandler;
    public Rigidbody2D followPointRB2D;
    public Rigidbody2D playerRB2D;

    [Header("Parameters")]
    public Vector2 sensitvity;
    public PlayerMovementParameters playerMovementParameters;

    public float distanceOffset;
    [Space]
    public float pullStrength;
    public Vector2 pullStrengthMinMax;
    [Space]
    public float oribitalMovementCounterStrength = 0;
    public AnimationCurve orbitalCounterSpeedScaleCurve;
    [Space]
    public AnimationCurve dragAgainstProximityToFollowPointCurve;

    [Header("Settings")]
    public bool debugMode = false;

    Vector2 mouseInput = Vector2.zero;

    public Vector2 FacingDirection
    {
        get { return facingDirection; }
    }
    Vector2 facingDirection = Vector2.down;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        mouseInput = GetMouseInput();
        MoveFolowPoint(mouseInput);
        MovePlayerTowardsFollowpoint();

    }

    void MoveFolowPoint(Vector2 moveVector)
    {
        followPointRB2D.AddForce(moveVector, ForceMode2D.Impulse);
    }

    void MovePlayerTowardsFollowpoint()
    {
        // load playerMovementParameters (this is just to make the code more less verbose).
        float pullStrength = playerMovementParameters.pullStrength;
        Vector2 pullStrengthMinMax = playerMovementParameters.pullStrengthMinMax;
        AnimationCurve orbitalCounterSpeedScaleCurve = playerMovementParameters.orbitalCounterSpeedScaleCurve;
        float oribitalMovementCounterStrength = playerMovementParameters.oribitalMovementCounterStrength;
        AnimationCurve dragAgainstProximityToFollowPointCurve = playerMovementParameters.dragAgainstProximityToFollowPointCurve;
        float distanceOffset = playerMovementParameters.distanceOffset;

        // offset followpoint by distance offset
        Vector2 towardsFollowpoint = followPointRB2D.transform.position - transform.position;
        Vector2 offsetFollowPoint = followPointRB2D.transform.position + Vector2ToVector3(towardsFollowpoint.normalized * distanceOffset);
        Vector2 offsetTowardsFollowpoint = Vector2ToVector3(offsetFollowPoint) - transform.position;

        // make player face movement direction
        spritesHandler.PostDirectionToAnimators(towardsFollowpoint);
        facingDirection = towardsFollowpoint;

        // pull force, pulls player towards the followpoint
        Vector2 moveVector = offsetTowardsFollowpoint * pullStrength;
        Vector2 clampedMoveVector = moveVector.normalized * Mathf.Clamp(moveVector.magnitude, pullStrengthMinMax.x, pullStrengthMinMax.y);
        playerRB2D.AddForce(clampedMoveVector, ForceMode2D.Impulse);

        // orbital movement counter
        // (to stop the player from oribiting the followpoint like a planet around a star) 
        // to do this it gets the vectors perpendicular to the direction it wants to go (towards the player)
        // and it pulls the player using those when the players velocity is alligned with one of those perpendicular vectors
        Vector2 perpClockwise = Vector2.Perpendicular(moveVector.normalized);
        Vector2 perpAntiClockwise = -perpClockwise;
        Vector2 playerOrbitVelocity = Vector2Abs(playerRB2D.velocity).normalized;

        Vector2 perpToUse = Vector2.zero; // use whatever perp is the most opposite to the velocity (this perp is selected in the if statements below) (this effectively gives the direction of the orbit)
        float magnitudedOfRelevantPerp = 0;
        float clockwiseVelocityMagnitude = (perpClockwise - playerRB2D.velocity.normalized).magnitude;
        float antiClockwiseVelocityMagnitude = (perpAntiClockwise - playerRB2D.velocity.normalized).magnitude;
        if (clockwiseVelocityMagnitude > antiClockwiseVelocityMagnitude)
        {
            perpToUse = perpClockwise;
            magnitudedOfRelevantPerp = (perpClockwise - playerRB2D.velocity).magnitude;
        }
        else if (clockwiseVelocityMagnitude < antiClockwiseVelocityMagnitude)
        {
            perpToUse = perpAntiClockwise;
            magnitudedOfRelevantPerp = (perpAntiClockwise - playerRB2D.velocity).magnitude;
        }

        float speedAdjustment = orbitalCounterSpeedScaleCurve.Evaluate(magnitudedOfRelevantPerp); ; // if it's moving faster then the counter force should be stronger
        Vector2 orbitalMovementCounterForce = playerOrbitVelocity * perpToUse * oribitalMovementCounterStrength * speedAdjustment;
        playerRB2D.AddForce(orbitalMovementCounterForce, ForceMode2D.Force);

        // overshooting counter (stops the player from going super fast and launching past the follow point)
        // (it increases drag as the player gets closer to the followpoint)
        float distanceFromFollowPoint = Vector2.Distance(transform.position, followPointRB2D.transform.position);
        float newDrag = dragAgainstProximityToFollowPointCurve.Evaluate(distanceFromFollowPoint);
        playerRB2D.drag = newDrag;

        // debug
        if (debugMode == true)
        {
            Debug.DrawRay(transform.position, offsetTowardsFollowpoint, Color.green);
            Debug.DrawRay(transform.position, orbitalMovementCounterForce, Color.magenta);
            Debug.DrawRay(transform.position, perpClockwise, Color.green);
            Debug.DrawRay(transform.position, perpAntiClockwise, Color.green);
            Debug.DrawRay(transform.position, playerRB2D.velocity, Color.blue);
        }
    }

    Vector2 GetMouseInput()
    {
        Vector2 input = Vector2.zero;
        input.x = Input.GetAxis("Mouse X") * Time.deltaTime * sensitvity.x;
        input.y = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitvity.y;
        return input;
    }

    Vector2 Vector2Abs(Vector2 vector2)
    {
        return new Vector2(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y));
    }

    Vector3 Vector2ToVector3(Vector2 vector2)
    {
        return new Vector3(vector2.x, vector2.y, 0);
    }
    */
}
