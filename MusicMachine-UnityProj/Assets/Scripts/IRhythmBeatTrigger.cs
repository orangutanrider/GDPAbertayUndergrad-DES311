using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRhythmBeatTrigger 
{
    // All beat triggers must first be loaded via "RhythmController.instance.LoadTriggerIntoRhythmController(this);"
    public void BeatTrigger(RhythmBeat beat);
}
