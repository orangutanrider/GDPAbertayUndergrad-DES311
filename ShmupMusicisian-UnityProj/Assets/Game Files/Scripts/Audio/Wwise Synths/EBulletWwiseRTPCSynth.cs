using UnityEngine;

public abstract class EBulletWwiseRTPCSynth : ScriptableObject
{
    // even though EBullet contains a EBulletSynth reference field, that field is supposed to be nullable
    // so I added it to these as a required field
    // not sure if this is the best way to go about that kind of thing though
    // could probably use a EBulletSynth property with somekind of error return instead

    public abstract void Play(EBullet audioHost, EBulletSynth synthComponent, EnvelopeObj envelopeObj, bool printErrors = true);
    public abstract void Stop(EBullet audioHost, EBulletSynth synthComponent, EnvelopeObj envelopeObj, bool printErrors = true);

    public abstract Envelope Envelope { get; }
}
