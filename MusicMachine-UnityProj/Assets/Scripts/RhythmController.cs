using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmController : MonoBehaviour
{
    [Header("Parameters")]
    public bool autoMode = false;
    public float tempo = 1;
    [Space]
    public int sequenceIndex = -1;
    public BeatType targetedBeat = BeatType.A;
    public BeatType activeBeat = BeatType.A;

    float tempoTimer = 0;
    float timeSincePreviousBeat = 0;
    bool firstFrame = true;

    const int sequenceLength = 8;
    const int maxBeatsStoredAtATime = 8;
    const float timeCutOff = 3f;

    List<IRhythmBeatTrigger> beatTriggers = new List<IRhythmBeatTrigger>();
    List<RhythmBeat> beats = new List<RhythmBeat>();

    public static RhythmController instance = null;

    public void LoadTriggerIntoRhythmController(IRhythmBeatTrigger trigger)
    {
        beatTriggers.Add(trigger);
    }

    void PostBeatTriggers(RhythmBeat beat)
    {
        foreach(IRhythmBeatTrigger beatTrigger in beatTriggers)
        {
            beatTrigger.BeatTrigger(beat);
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of this?");
        }

        instance = this;
    }

    void Update()
    {
        if(firstFrame == true)
        {
            RhythmKeyInput(BeatType.A);
            firstFrame = false;
            return;
        }

        timeSincePreviousBeat = timeSincePreviousBeat + Time.deltaTime;

        if(autoMode == true) 
        {
            AutoMode();
            return; 
        }

        if(Input.GetMouseButtonDown(0) == true)
        {
            RhythmKeyInput(BeatType.A);
        }
        else if(Input.GetMouseButtonDown(1) == true)
        {
            RhythmKeyInput(BeatType.B);
        }
        else if (Input.GetMouseButtonDown(2) == true)
        {
            RhythmKeyInput(BeatType.C);
        }
    }

    void AutoMode()
    {
        tempoTimer = tempoTimer + Time.deltaTime;

        if (Input.GetMouseButtonDown(0) == true)
        {
            targetedBeat = BeatType.A;
        }
        else if (Input.GetMouseButtonDown(1) == true)
        {
            targetedBeat = BeatType.B;
        }
        else if (Input.GetMouseButtonDown(2) == true)
        {
            targetedBeat = BeatType.C;
        }

        if(tempoTimer < tempo)
        {
            return;
        }
        tempoTimer = 0;
        RhythmKeyInput(targetedBeat);
    }

    void RhythmKeyInput(BeatType beatType)
    {
        activeBeat = beatType;

        sequenceIndex++;
        if (sequenceIndex >= sequenceLength)
        {
            sequenceIndex = 0;
        }

        RhythmBeat newBeat = new RhythmBeat(sequenceIndex, GameStateMaster.instance.gameTimer, timeSincePreviousBeat, beatType);
        PostBeatTriggers(newBeat);

        timeSincePreviousBeat = 0;
    }
}
