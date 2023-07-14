using UnityEngine;
using AK.Wwise;

public class EnemyBulletBasicNoteParams : ScriptableObject
{
    public AK.Wwise.Event playSynthEvent;
    public AK.Wwise.Event stopSynthEvent;

    [Header("RTPC References")]
    public RTPC pitchRTPC;      // tied to xPosition
    public RTPC pwmRTPC;        // tiedToAngle
    public RTPC transposeRTPC;  // tiedToSpeed
    public RTPC volumeRTPC;     // used to define envelope

    [Header("RTPC Connections")]
    public Vector2 pitchXPositionRange = new Vector2(0, 1);
    public Vector2 pwmAngleRange = new Vector2(-180, 180);
    public Vector2 transposeSpeedRange = new Vector2(0, 1);
}
