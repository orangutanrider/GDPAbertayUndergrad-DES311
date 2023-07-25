using UnityEngine;

[CreateAssetMenu(fileName = "SetSynthAddOn", menuName = "EBullet/EmitterAddOn/SetSynthAddOn")]
public class EInputEmitterSetSynthAddOn : EInputEmitterAddOn
{
    public EBulletWwiseRTPCSynth synth;

    public override void AddToBulletEmissionEvent(EInputEmitter inputEmitter)
    {
        inputEmitter.bulletEmission += SetSynthEvents;
    }

    void SetSynthEvents(EBullet bullet, EInputEmitter sender)
    {
        bullet.synth.Play(synth);
    }
}
