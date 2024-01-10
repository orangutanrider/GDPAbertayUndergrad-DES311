using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmBeat 
{
    public int triggeredAtIndex = 0;
    public float triggeredAt = 0;
    public float triggeredInXSecondsOfPreviousBeat = 0;
    public BeatType beatType = BeatType.A;

    public RhythmBeat(int _triggeredAtIndex, float _triggeredAt, float _triggeredInXSecondsOfPreviousBeat, BeatType _beatType)
    {
        triggeredAtIndex = _triggeredAtIndex;
        triggeredAt = _triggeredAt;
        triggeredInXSecondsOfPreviousBeat = _triggeredInXSecondsOfPreviousBeat;
        beatType = _beatType;
    }
}