using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpritesHandler : MonoBehaviour, IRhythmBeatTrigger
{
    [Header("Required References")]
    public SquashAndStretcher mainSquashAndStretcher;
    public Animator outlineAnimator;
    public Animator backgroundAnimator;

    [Header("Parameters")]
    public float beatSquashMagnitude;
    public float beatSquashDuration;

    const int numberOfAnimatorDirections = 8;
    const string animatorDirectionParameterName = "DirectionIndex";

    void Start()
    {
        RhythmController.instance.LoadTriggerIntoRhythmController(this);
    }

    void IRhythmBeatTrigger.BeatTrigger(RhythmBeat beat)
    {
        mainSquashAndStretcher.StartSquashAndStretch(beatSquashDuration, 1f, beatSquashMagnitude, SquashAndStretcher.ScaleMethod.ExpandAndShrink, SquashAndStretcher.LerpType.Elastic);
    }

    public void PostDirectionToAnimators(Vector2 directionVector)
    {
        // starting from the right direction, then going anti-clockwise

        int animatorIndex = ConvertDirectionToAnimatorIndex(directionVector);
        outlineAnimator.SetInteger(animatorDirectionParameterName, animatorIndex);
        backgroundAnimator.SetInteger(animatorDirectionParameterName, animatorIndex);
    }

    int ConvertDirectionToAnimatorIndex(Vector2 directionVector)
    {
        float directionAngle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
        float angleIncrement = 360f / numberOfAnimatorDirections;
        float roundedAngle = Mathf.Round(directionAngle / angleIncrement) * angleIncrement;
        if(roundedAngle < 0)
        {
            roundedAngle = -roundedAngle + 180f;
        }
        return Mathf.RoundToInt(roundedAngle/angleIncrement);
    }

    Vector2 RotateAroundPivot(Vector2 point, Vector2 pivot, float angle)
    {
        angle = angle + Vector2.Angle(Vector2.up, (point - pivot).normalized);
        float angleInRad = angle * Mathf.Deg2Rad;
        float pointDistance = Vector2.Distance(point, pivot);
        float x = (Mathf.Cos(angleInRad) * pointDistance) + pivot.x;
        float y = (Mathf.Sin(angleInRad) * pointDistance) + pivot.y;

        Debug.Log(new Vector2(x, y));

        return new Vector2(x, y);
    }

    Vector3 Vector2ToVector3(Vector2 vector2)
    {
        return new Vector3(vector2.x, vector2.y, 0);
    }
}
