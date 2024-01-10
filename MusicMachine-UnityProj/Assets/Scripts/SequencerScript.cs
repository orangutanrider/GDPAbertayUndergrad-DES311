using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencerScript : MonoBehaviour, IRhythmBeatTrigger
{
    [Header("Required References")]
    public LFOScript flickerLFO;
    [Space]
    public List<SequencerWindowScript> windowScriptsRowA = new List<SequencerWindowScript>();
    public List<SequencerWindowScript> windowScriptsRowB = new List<SequencerWindowScript>();
    public List<SequencerWindowScript> windowScriptsRowC = new List<SequencerWindowScript>();

    BeatType activeRow = BeatType.A;

    public float FlickerLFOValue
    {
        get { return flickerLFO.LFOValue; }
    }

    void Start()
    {
        RhythmController.instance.LoadTriggerIntoRhythmController(this);
    }

    void IRhythmBeatTrigger.BeatTrigger(RhythmBeat beat)
    {
        switch (beat.beatType)
        {
            case BeatType.A:
                RelayBeatTriggerToWindowScripts(windowScriptsRowA, beat);
                break;
            case BeatType.B:
                RelayBeatTriggerToWindowScripts(windowScriptsRowB, beat);
                break;
            case BeatType.C:
                RelayBeatTriggerToWindowScripts(windowScriptsRowC, beat);
                break;
        }

        if (beat.beatType == activeRow)
        {
            return;
        }
        switch (activeRow)
        {
            case BeatType.A:
                RelayBeatTriggerToWindowScripts(windowScriptsRowA, beat);
                break;
            case BeatType.B:
                RelayBeatTriggerToWindowScripts(windowScriptsRowB, beat);
                break;
            case BeatType.C:
                RelayBeatTriggerToWindowScripts(windowScriptsRowC, beat);
                break;
        }
        activeRow = beat.beatType;
    }

    void RelayBeatTriggerToWindowScripts(List<SequencerWindowScript> windowScripts, RhythmBeat beat)
    {
        foreach(SequencerWindowScript windowScript in windowScripts)
        {
            windowScript.RecieveRelayedBeatTrigger(beat);
        }
    }
}
