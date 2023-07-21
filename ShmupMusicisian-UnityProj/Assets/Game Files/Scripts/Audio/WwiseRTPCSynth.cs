using UnityEngine;
using AK.Wwise;

public abstract class WwiseRTPCSynth 
{
    // make sure inheritors implement [System.Serializable]

    public abstract RTPC VolumeRTPC { get; protected set; }

    public abstract void Play(GameObject audioHost);
    public abstract void Stop(GameObject audioHost);
    public abstract void UpdateSynthRTPCs(GameObject audioHost);
}
