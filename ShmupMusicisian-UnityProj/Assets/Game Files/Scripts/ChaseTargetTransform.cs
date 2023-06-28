using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTargetTransform : MonoBehaviour
{
    [SerializeField] bool active = true;
    public bool debugMode = false;

    public bool Active
    {
        get { return active; }
        set
        {
            active = value;
            enabled = value;
        }
    }

    [Header("Required References")]
    public Rigidbody2D rb2D;
    public ChaseTargetTransformParams scriptParams;

    [Header("Parameters")]
    public Transform target;
    [Space]
    public float pullStrengthEffector = 1;
    public float dragEffector = 1;
    public float orbitalCounterEffector = 1;

    private void FixedUpdate()
    {
        if(active == false || target == null) { return; }
        ChaseObject();
    }

    void ChaseObject()
    {
        // get params from Scriptable Object
        float distanceOffset = scriptParams.distanceOffset;
        AnimationCurve pullStrengthAgainstTargetDistance = scriptParams.pullStrengthAgainstTargetsDistance;
        AnimationCurve dragAgainstTargetDistance = scriptParams.dragAgainstTargetsDistance;
        AnimationCurve orbitalCounterAgainstSpeed = scriptParams.orbitalCounterAgainstSpeed;
        float orbitalCounterStrength = scriptParams.orbitalCounterStrength;

        // offset followpoint by distance offset
        Vector2 towardsFollowpoint = target.position - transform.position;
        Vector2 offsetFollowPoint = target.position + Vector2ToVector3(towardsFollowpoint.normalized * distanceOffset);
        Vector2 towardsOffsetFollowpoint = Vector2ToVector3(offsetFollowPoint) - transform.position;

        // pull towards the followpoint
        float targetDistance = Vector2.Distance(rb2D.position, target.position);
        float pullStrength = pullStrengthAgainstTargetDistance.Evaluate(targetDistance);
        Vector2 moveVector = towardsOffsetFollowpoint.normalized * pullStrength;
        rb2D.AddForce(moveVector * pullStrengthEffector, ForceMode2D.Impulse);

        // orbital movement counter
        // (to stop the player from oribiting the followpoint like a planet around a star) 
        // to do this it gets the vectors perpendicular to the direction it wants to go (towards the player)
        // and it pulls the player using those when the players velocity is alligned with one of those perpendicular vectors
        Vector2 perpClockwise = Vector2.Perpendicular(moveVector.normalized);
        Vector2 perpAntiClockwise = -perpClockwise;
        Vector2 playerOrbitVelocity = Vector2Abs(rb2D.velocity).normalized;

        // select the perp that is the most opposite to the velocity
        // (this perp is selected in the if statements below) (this effectively gives the direction of the orbit)
        Vector2 perpToUse = Vector2.zero;
        float magnitudedOfRelevantPerp = 0;
        float clockwiseVelocityMagnitude = (perpClockwise - rb2D.velocity.normalized).magnitude;
        float antiClockwiseVelocityMagnitude = (perpAntiClockwise - rb2D.velocity.normalized).magnitude;
        if (clockwiseVelocityMagnitude > antiClockwiseVelocityMagnitude)
        {
            perpToUse = perpClockwise;
            magnitudedOfRelevantPerp = (perpClockwise - rb2D.velocity).magnitude;
        }
        else if (clockwiseVelocityMagnitude < antiClockwiseVelocityMagnitude)
        {
            perpToUse = perpAntiClockwise;
            magnitudedOfRelevantPerp = (perpAntiClockwise - rb2D.velocity).magnitude;
        }

        float speedAdjustment = orbitalCounterAgainstSpeed.Evaluate(magnitudedOfRelevantPerp); ; // if it's moving faster then the counter force should be stronger
        Vector2 orbitalMovementCounterForce = playerOrbitVelocity * perpToUse * orbitalCounterStrength * speedAdjustment;
        rb2D.AddForce(orbitalMovementCounterForce * orbitalCounterEffector, ForceMode2D.Force);

        // overshooting counter (stops the player from going super fast and launching past the follow point)
        // (it increases drag as the player gets closer to the followpoint)
        float distanceFromFollowPoint = Vector2.Distance(transform.position, target.position);
        float newDrag = dragAgainstTargetDistance.Evaluate(distanceFromFollowPoint);
        rb2D.drag = newDrag * dragEffector;

        // debug
        if (debugMode == true)
        {
            Debug.DrawRay(transform.position, towardsOffsetFollowpoint, Color.green);
            Debug.DrawRay(transform.position, orbitalMovementCounterForce, Color.magenta);
            Debug.DrawRay(transform.position, perpClockwise, Color.green);
            Debug.DrawRay(transform.position, perpAntiClockwise, Color.green);
            Debug.DrawRay(transform.position, rb2D.velocity, Color.blue);
        }
    }

    Vector2 Vector2Abs(Vector2 vector2)
    {
        return new Vector2(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y));
    }

    Vector3 Vector2ToVector3(Vector2 vector2)
    {
        return new Vector3(vector2.x, vector2.y, 0);
    }
}
