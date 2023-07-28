using System.Collections.Generic;
using UnityEngine;

public class EBulletSynth : EBulletComponent, IEBulletOnActivate
{
    [Header("Required References")]
    public EBullet bullet;
    [SerializeField] EBulletWwiseRTPCSynth synth;

    EnvelopeObj[] envelopes = null;
    EnvelopeObj[] overrideEnvelopes = null;

    bool envelopesInit = false;
    bool overrideEnvelopesInit = false;

    List<Coroutine> synthUpdateCoroutines = new List<Coroutine>();

    public override IEBulletOnActivate GetOnActivate() { return this; }
    public override IEBulletOnDeactivate GetOnDeactivate() { return null; }
    public override IEBulletOnDeactivation GetOnDeactivation() { return null; }

    #region synthUpdateCoroutines functions
    public void RemoveSynthUpdateCoroutine(Coroutine coroutine)
    {
        synthUpdateCoroutines.Remove(coroutine);
    }

    public void AddSynthUpdateCoroutine(Coroutine coroutine)
    {
        synthUpdateCoroutines.Add(coroutine);
    }

    public void StopAllSynthUpdateCoroutines()
    {
        foreach(Coroutine coroutine in synthUpdateCoroutines)
        {
            StopCoroutine(coroutine);
        }
        synthUpdateCoroutines.Clear();
    }
    #endregion

    public void Play()
    {
        if(envelopesInit == false)
        {
            envelopes = new EnvelopeObj[synth.Envelopes.Length];

            for (int loop = 0; loop < synth.Envelopes.Length; loop++)
            {
                envelopes[loop] = new EnvelopeObj();
            }

            envelopesInit = true;
        }

        for (int loop = 0; loop < synth.Envelopes.Length; loop++)
        {
            envelopes[loop].Envelope = synth.Envelopes[loop];
        }

        synth.Play(bullet, this, envelopes);
    }

    public void Play(EBulletWwiseRTPCSynth overrideSynth, bool disableEnvArrayInit = false)
    {
        if (overrideEnvelopesInit == false || disableEnvArrayInit == true)
        {
            overrideEnvelopes = new EnvelopeObj[overrideSynth.Envelopes.Length];

            for (int loop = 0; loop < synth.Envelopes.Length; loop++)
            {
                overrideEnvelopes[loop] = new EnvelopeObj();
            }

            overrideEnvelopesInit = true;
        }

        for (int loop = 0; loop < overrideSynth.Envelopes.Length; loop++)
        {
            overrideEnvelopes[loop].Envelope = overrideSynth.Envelopes[loop];
        }

        overrideSynth.Play(bullet, this, overrideEnvelopes);
    }

    void IEBulletOnActivate.OnActivate()
    {
        Play();
    }
}
