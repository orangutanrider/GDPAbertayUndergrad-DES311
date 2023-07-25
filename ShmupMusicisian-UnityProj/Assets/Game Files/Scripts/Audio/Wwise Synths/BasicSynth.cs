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
    [SerializeField] RTPC waveformRTPC;
    [SerializeField] RTPC FMAmountRTPC;
    [SerializeField] RTPC NoiseLevelRTPC;

    public override Envelope Envelope
    {
        get
        {
            return envelopeParams.GetDataCopy();
        }
    }

    public override void Play(EBullet audioHost, EBulletSynth synthComponent, EnvelopeObj envelopeObj, bool printErrors = true)
    {
        // play
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
        NoiseLevelRTPC.SetValue(synthComponent.gameObject, noiseLevelValue);

        // start update coroutine
        if (synthComponent.SynthCoroutine == null)
        {
            IEnumerator updateVolume = UpdateVolume(audioHost, synthComponent, envelopeObj, printErrors);
            synthComponent.StartCoroutine(updateVolume);
        }
    }

    public override void Stop(EBullet audioHost, EBulletSynth synthComponent, EnvelopeObj envelopeObj, bool printErrors = true)
    {
        if(envelopeObj.CurrentState() == EnvelopeState.END || envelopeObj.CurrentState() == EnvelopeState.NEGATIVE)
        {
            stopEvent.Post(synthComponent.gameObject);
            return;
        }

        envelopeObj.InterruptEnvelope(synthComponent);

        if (synthComponent.SynthCoroutine == null)
        {
            IEnumerator updateVolume = UpdateVolume(audioHost, synthComponent, envelopeObj, printErrors);
            synthComponent.StartCoroutine(updateVolume);
        }
    }

    IEnumerator UpdateVolume(EBullet audioHost, EBulletSynth synthComponent, EnvelopeObj envelopeObj, bool printErrors = true)
    {
        while (envelopeObj.CurrentState() != EnvelopeState.END || envelopeObj.CurrentState() != EnvelopeState.NEGATIVE)
        {
            volumeRTPC.SetValue(synthComponent.gameObject, Mathf.Lerp(0, 100, envelopeObj.Current01Value));
            yield return null;
        }

        stopEvent.Post(synthComponent.gameObject);
        yield break;
    }
}
