using UnityEngine;

public class EBulletSynth : EBulletComponent, IEBulletOnActivate
{
    [Header("Required References")]
    public EBullet bullet;
    [SerializeField] EBulletWwiseRTPCSynth synth;

    EnvelopeObj envelopeObj = new EnvelopeObj();
    EnvelopeObj overrideEnvelope = new EnvelopeObj();

    public override IEBulletOnActivate GetOnActivate() { return this; }
    public override IEBulletOnDeactivate GetOnDeactivate() { return null; }
    public override IEBulletOnDeactivation GetOnDeactivation() { return null; }

    public Coroutine SynthCoroutine { get; set; }

    public void Play()
    {
        envelopeObj.Envelope = synth.Envelope;
        synth.Play(bullet, this, envelopeObj);
    }

    public void Play(EBulletWwiseRTPCSynth synth)
    {
        overrideEnvelope.Envelope = synth.Envelope;
        synth.Play(bullet, this, overrideEnvelope);
    }

    void IEBulletOnActivate.OnActivate()
    {
        Play();
    }
}
