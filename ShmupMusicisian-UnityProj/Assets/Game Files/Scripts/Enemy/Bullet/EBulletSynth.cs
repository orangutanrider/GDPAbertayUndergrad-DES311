public abstract class EBulletSynth : EBulletComponent, IEBulletOnActivate
{
    public override IEBulletOnActivate GetOnActivate() { return this; }
    public override IEBulletOnDeactivate GetOnDeactivate() { return null; }
    public override IEBulletOnDeactivation GetOnDeactivation() { return null; }

    public abstract void Play();
    public abstract void Play(Envelope overrideEnvelope);
    public abstract void Play(WwiseRTPCSynth overrideSynth);
    public abstract void Play(Envelope overrideEnvelope, WwiseRTPCSynth overrideSynth);

    void IEBulletOnActivate.OnActivate()
    {
        Play();
    }
}
