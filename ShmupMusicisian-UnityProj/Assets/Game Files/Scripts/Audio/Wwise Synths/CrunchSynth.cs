using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrunchSynth : EBulletWwiseRTPCSynth
{
    public override Envelope Envelope
    {
        get;
    }

    public override void Play(EBullet audioHost, EBulletSynth synthComponent, EnvelopeObj envelopeObj, bool printErrors = true)
    {

    }

    public override void Stop(EBullet audioHost, EBulletSynth synthComponent, EnvelopeObj envelopeObj, bool printErrors = true)
    {

    }
}
