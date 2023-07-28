using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

[CreateAssetMenu(fileName = "CrunchSynth", menuName = "Audio/EBulletSynths/CrunchSynth")]
public class CrunchSynth : EBulletWwiseRTPCSynth
{
    [Header("Params")]
    // x min, y max
    [SerializeField] Vector2 pitchYPositionRange;
    [SerializeField] Vector2 fmXPositionRange;
    [SerializeField] Vector2 pwmAngleRange;
    [SerializeField] Vector2 transposeSpeedRange;
    [Space]
    [Range(0, 100)]
    [SerializeField] float noiseLevelValue = 50;
    [Range(0, 100)]
    [SerializeField] float unlinkedPitchValue = 50;

    [Header("Required")]
    [SerializeField] AK.Wwise.Event playEvent;
    [SerializeField] AK.Wwise.Event stopEvent;
    [Space]
    [SerializeField] RTPC volumeRTPC;
    [SerializeField] EnvelopeParams volumeEnvelope;
    [Space]
    [SerializeField] RTPC pitchRTPC;
    [SerializeField] RTPC pwmRTPC;
    [SerializeField] RTPC transposeRTPC;
    [SerializeField] RTPC FMAmountRTPC;

    [Header("Required, Unlinked RTPCs")]
    [SerializeField] RTPC unlinkedPitchRTPC;
    [SerializeField] RTPC noiseLevelRTPC;

    // this kinda sucks but the alternative is reworking the flag system as a whole.
    List<EBullet> flagBearers = new List<EBullet>();

    Envelope[] envelopeCache = new Envelope[1];

    public override Envelope[] Envelopes
    {
        get
        {
            envelopeCache[0] = volumeEnvelope.GetDataCopy();
            return envelopeCache;
        }
    }

    public override void Play(EBullet audioHost, EBulletSynth synthComponent, EnvelopeObj[] envelopeObjs, bool printErrors = true)
    {
        // Raise Flag
        flagBearers.Add(audioHost);
        audioHost.RaiseStayActiveFlag();

        // play
        playEvent.Post(synthComponent.gameObject);
        foreach (EnvelopeObj envelopeObj in envelopeObjs)
        {
            envelopeObj.TriggerEnvelopeCoroutine(synthComponent);
            volumeRTPC.SetValue(synthComponent.gameObject, Mathf.Lerp(0, 100, envelopeObj.Current01Value));  // volume
            noiseLevelRTPC.SetValue(synthComponent.gameObject, Mathf.Lerp(0, 100, envelopeObj.Current01Value));
        }

        // set linked RTPCs
        float yPositionRange01 = (audioHost.transform.position.x - pitchYPositionRange.x) / (pitchYPositionRange.y - pitchYPositionRange.x);
        pitchRTPC.SetValue(synthComponent.gameObject, Mathf.Lerp(0, 100, yPositionRange01)); // pitch (tied to y position)

        float xPositionRange01 = (audioHost.transform.position.x - fmXPositionRange.x) / (fmXPositionRange.y - fmXPositionRange.x);
        FMAmountRTPC.SetValue(synthComponent.gameObject, Mathf.Lerp(0, 100, xPositionRange01)); // FM (tied to x position)

        float angleRange01 = (audioHost.transform.eulerAngles.z - pwmAngleRange.x) / (pwmAngleRange.y - pwmAngleRange.x);
        pwmRTPC.SetValue(synthComponent.gameObject, Mathf.Lerp(0, 100, angleRange01)); // PWM (tied to z rotation in euler) (all bullets face down by default, 0 z is downwards)

        if (audioHost.mover != null)
        {
            float speedRange01 = (audioHost.mover.CurrentMovement.magnitude - transposeSpeedRange.x) / (transposeSpeedRange.y - transposeSpeedRange.x);
            transposeRTPC.SetValue(synthComponent.gameObject, Mathf.Lerp(0, 100, speedRange01)); // transpose (tied to speed)
        }
        else
        {
            if (printErrors == true)
            {
                Debug.LogWarning("AudioHost had no mover, this synth uses a mover to get velocity, to set the transpose. Have fallen back on using a value of 50.");
            }
            transposeRTPC.SetValue(synthComponent.gameObject, 50);
        }

        // set unlinked rtpcs
        noiseLevelRTPC.SetValue(synthComponent.gameObject, noiseLevelValue);
        unlinkedPitchRTPC.SetValue(synthComponent.gameObject, unlinkedPitchValue);

        // Start update coroutines
        synthComponent.StopAllSynthUpdateCoroutines();

        // volume
        CoroutineBox volumeBox = new CoroutineBox(null);
        IEnumerator updateVolume = UpdateVolume(volumeBox, audioHost, synthComponent, envelopeObjs[0], printErrors);
        volumeBox.Coroutine = synthComponent.StartCoroutine(updateVolume);
        synthComponent.AddSynthUpdateCoroutine(volumeBox.Coroutine);
    }

    /*
    public override void Stop(EBullet audioHost, EBulletSynth synthComponent, EnvelopeObj[] envelopeObjs, bool printErrors = true)
    {
        bool stopImmediatly = true;
        for(int loop = 0; loop < 2; loop++)
        {
            EnvelopeObj envelopeObj = envelopeObjs[loop];

            if (envelopeObj.CurrentState() == EnvelopeState.END || envelopeObj.CurrentState() == EnvelopeState.NEGATIVE)
            {
                // it can be stopped immedietly
            }
            else
            {
                // it can't be
                stopImmediatly = false;
            }
        }

        // can only stop immedietly if the update coroutines have already ran their course (in which case this function shouldn't even be needed)
        if (stopImmediatly == true)
        {
            // Lower Flag
            if (flagBearers.Contains(audioHost) == true)
            {
                flagBearers.Remove(audioHost);
                audioHost.LowerStayActiveFlag();
            }

            stopEvent.Post(synthComponent.gameObject);
            return;
        }

        envelopeObjs[0].InterruptEnvelope(synthComponent);
        envelopeObjs[1].InterruptEnvelope(synthComponent);

        // Start update coroutines

        synthComponent.StopAllCoroutines();

        // volume
        CoroutineBox volumeBox = new CoroutineBox(null);
        IEnumerator updateVolume = UpdateVolume(volumeBox, audioHost, synthComponent, envelopeObjs[0], printErrors);
        volumeBox.Coroutine = synthComponent.StartCoroutine(updateVolume);
        synthComponent.AddSynthUpdateCoroutine(volumeBox.Coroutine);

        // noise
        CoroutineBox noiseBox = new CoroutineBox(null);
        IEnumerator updateNoise = UpdateNoise(noiseBox, audioHost, synthComponent, envelopeObjs[1], printErrors);
        noiseBox.Coroutine = synthComponent.StartCoroutine(updateNoise);
        synthComponent.AddSynthUpdateCoroutine(noiseBox.Coroutine);

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
