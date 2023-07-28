using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using AK.Wwise;

[CreateAssetMenu(fileName = "BasicSynth", menuName = "Audio/EBulletSynths/BasicSynth")]
public class BasicSynth : EBulletWwiseRTPCSynth
{
    [Header("Params")]
    // x min, y max
    [SerializeField] Vector2 pitchPositionRange;
    [SerializeField] Vector2 pwmAngleRange;
    [SerializeField] Vector2 transposeSpeedRange;
    [Space]
    [Range(0, 100)]
    [SerializeField] float waveformValue = 50;
    [Range(0, 100)]
    [SerializeField] float fmValue = 50;
    [Range(0, 100)]
    [SerializeField] float noiseLevelValue = 50;
    [Range(0, 100)]
    [SerializeField] float unlinkedPitchValue = 50;

    [Header("Required")]
    [SerializeField] AK.Wwise.Event playEvent;
    [SerializeField] AK.Wwise.Event stopEvent;
    [Space]
    [SerializeField] RTPC volumeRTPC;
    [SerializeField] EnvelopeParams envelopeParams;
    [Space]
    [SerializeField] RTPC pitchRTPC;
    [SerializeField] RTPC pwmRTPC;
    [SerializeField] RTPC transposeRTPC;

    [Header("Required, Unlinked RTPCs")]
    [SerializeField] RTPC unlinkedPitchRTPC;
    [SerializeField] RTPC waveformRTPC;
    [SerializeField] RTPC FMAmountRTPC;
    [SerializeField] RTPC noiseLevelRTPC;

    // this kinda sucks but the alternative is reworking the flag system as a whole.
    List<EBullet> flagBearers = new List<EBullet>();

    Envelope[] envelopeCache = new Envelope[1];

    public override Envelope[] Envelopes
    {
        get
        {
            envelopeCache[0] = envelopeParams.GetDataCopy();
            return envelopeCache;
        }
    }

    public override void Play(EBullet audioHost, EBulletSynth synthComponent, EnvelopeObj[] envelopeObjs, bool printErrors = true)
    {
        EnvelopeObj envelopeObj = envelopeObjs[0];

        // Raise Flag
        flagBearers.Add(audioHost);
        audioHost.RaiseStayActiveFlag();

        // play
        playEvent.Post(synthComponent.gameObject);
        envelopeObj.TriggerEnvelopeCoroutine(synthComponent);

        // set linked RTPCs
        volumeRTPC.SetValue(synthComponent.gameObject, Mathf.Lerp(0, 100, envelopeObj.Current01Value)); // volume

        float xPositionRange01 = (audioHost.transform.position.x - pitchPositionRange.x) / (pitchPositionRange.y - pitchPositionRange.x);
        pitchRTPC.SetValue(synthComponent.gameObject, Mathf.Lerp(0, 100, xPositionRange01)); // pitch (tied to x position)

        float angleRange01 = (audioHost.transform.eulerAngles.z - pwmAngleRange.x) / (pwmAngleRange.y - pwmAngleRange.x);
        pwmRTPC.SetValue(synthComponent.gameObject, Mathf.Lerp(0, 100, angleRange01)); // PWM (tied to z rotation in euler) (all bullets face down by default, 0 z is downwards)

        if (audioHost.mover != null)
        {
            float speedRange01 = (audioHost.mover.CurrentMovement.magnitude - transposeSpeedRange.x) / (transposeSpeedRange.y - transposeSpeedRange.x);
            transposeRTPC.SetValue(synthComponent.gameObject, Mathf.Lerp(0, 100, speedRange01)); // transpose (tied to speed)
        }
        else
        {
            if(printErrors == true)
            {
                Debug.LogWarning("AudioHost had no mover, this synth uses a mover to get velocity, to set the transpose. Have fallen back on using a value of 50.");
            }
            transposeRTPC.SetValue(synthComponent.gameObject, 50); 
        }

        // set unlinked rtpcs
        waveformRTPC.SetValue(synthComponent.gameObject, waveformValue);
        FMAmountRTPC.SetValue(synthComponent.gameObject, fmValue);
        noiseLevelRTPC.SetValue(synthComponent.gameObject, noiseLevelValue);
        unlinkedPitchRTPC.SetValue(synthComponent.gameObject, unlinkedPitchValue);

        // start update coroutine
        synthComponent.StopAllSynthUpdateCoroutines();

        CoroutineBox newBox = new CoroutineBox(null);
        IEnumerator updateVolume = UpdateVolume(newBox, audioHost, synthComponent, envelopeObj, printErrors);
        newBox.Coroutine = synthComponent.StartCoroutine(updateVolume);
        synthComponent.AddSynthUpdateCoroutine(newBox.Coroutine);
    }

    /*
    public override void Stop(EBullet audioHost, EBulletSynth synthComponent, EnvelopeObj[] envelopeObjs, bool printErrors = true)
    {
        EnvelopeObj envelopeObj = envelopeObjs[0];

        // can only stop immediatly if the update coroutines have already ran their course (in which case this function shouldn't even be needed)
        if (envelopeObj.CurrentState() == EnvelopeState.END || envelopeObj.CurrentState() == EnvelopeState.NEGATIVE)
        {
            // Lower Flag
            if (flagBearers.Contains(audioHost) == true)
            {
                flagBearers.Remove(audioHost);
                audioHost.LowerStayActiveFlag();
            }

            // Stop playing
            stopEvent.Post(synthComponent.gameObject);

            return;
        }

        envelopeObj.InterruptEnvelope(synthComponent);

        // start update coroutine
        synthComponent.StopAllSynthUpdateCoroutines();

        CoroutineBox newBox = new CoroutineBox(null);
        IEnumerator updateVolume = UpdateVolume(newBox, audioHost, synthComponent, envelopeObj, printErrors);
        newBox.Coroutine = synthComponent.StartCoroutine(updateVolume);
        synthComponent.AddSynthUpdateCoroutine(newBox.Coroutine);

        // the update coroutines are self stopping, hence no stop even post or anything like that
    }
    */

    IEnumerator UpdateVolume(CoroutineBox box, EBullet audioHost, EBulletSynth synthComponent, EnvelopeObj envelopeObj, bool printErrors = true)
    {
        while (envelopeObj.CurrentState() != EnvelopeState.END && envelopeObj.CurrentState() != EnvelopeState.NEGATIVE)
        {
            volumeRTPC.SetValue(synthComponent.gameObject, Mathf.Lerp(0, 100, envelopeObj.Current01Value));
            yield return null;
        }

        yield return null;

        // Lower Flag
        if (flagBearers.Contains(audioHost) == true)
        {
            flagBearers.Remove(audioHost);
            audioHost.LowerStayActiveFlag();
        }

        // Stop playing
        stopEvent.Post(synthComponent.gameObject);
        synthComponent.RemoveSynthUpdateCoroutine(box.Coroutine);
        synthComponent.StopCoroutine(box.Coroutine);

        yield break;
    }
}
