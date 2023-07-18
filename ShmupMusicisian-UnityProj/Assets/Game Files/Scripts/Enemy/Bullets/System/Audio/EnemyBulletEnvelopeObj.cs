using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemyBulletEnvelopeObj
{
    public EnemyBulletEnvelopeObj(EnemyBulletEnvelope envelope)
    {
        Envelope = envelope;
    }

    [Range(0f, 1f)]
    [SerializeField] float magnitude = 1;
    [Range(0f, 10f)]
    [SerializeField] float holdTime = 0;
    [Space]
    [Range(0f, 1f)]
    [SerializeField] float attack = 0.1f;
    [Range(0f, 10f)]
    [SerializeField] float decay = 1f;
    [Range(0f, 1f)]
    [SerializeField] float sustain = 0.66f;
    [Range(0f, 10f)]
    [SerializeField] float release = 1f;

    #region Variables
    public EnemyBulletEnvelopeEditorObj GetEditorEnvelope()
    {
        return new EnemyBulletEnvelopeEditorObj(Envelope);
    }

    public EnemyBulletEnvelope Envelope
    {
        get
        {
            return new EnemyBulletEnvelope(magnitude, holdTime, attack, decay, sustain, release);
        }
        set
        {
            holdTime = value.holdTime;
            attack = value.attack;
            decay = value.decay;
            sustain = value.sustain;
            release = value.release;
        }
    }

    public float TotalDuration
    {
        get
        {
            return attack + decay + holdTime + release;
        }
    }

    const float cancelLerpDuration = 0.1f;

    public Coroutine CurrentCoroutine { get; private set; }
    public MonoBehaviour CurrentTriggerSource { get; private set; }
    public float Current01Value { get; private set; }
    public float CurrentTime { get; private set; }

    EnemyBulletEnvelopeState currentStatus = EnemyBulletEnvelopeState.END;
    public EnemyBulletEnvelopeState CurrentStatus()
    {
        return currentStatus;
    }
    #endregion

    public void TriggerEnvelopeCoroutineLerp(MonoBehaviour triggerSource)
    {
        // automatically handle the cancelling of that one
        if(CurrentCoroutine != null)
        {
            // cancel the current one (it's a lerp to avoid audio popping)
            triggerSource.StartCoroutine(CancelLerp());

            // start a new one on a delay so that it starts as the cancel lerp ends
            IEnumerator delayedFollowEnvelope = DelayFollowEnvelope(cancelLerpDuration, triggerSource); 
            CurrentCoroutine = triggerSource.StartCoroutine(delayedFollowEnvelope);

            CurrentTriggerSource = triggerSource;
            return;
        }

        Current01Value = 0;
        CurrentTime = 0;
        currentStatus = EnemyBulletEnvelopeState.Attack;

        CurrentTriggerSource = triggerSource;
        CurrentCoroutine = triggerSource.StartCoroutine(FollowEnvelope());
    }

    public void InterruptEnvelopeLerp(MonoBehaviour triggerSource)
    {
        if (CurrentTriggerSource == null) { return; }
        CurrentTriggerSource.StopCoroutine(CurrentCoroutine);
        CurrentCoroutine = null;
        CurrentTriggerSource = null;

        triggerSource.StartCoroutine(CancelLerp());
    }

    IEnumerator FollowEnvelope()
    {
        Current01Value = 0;
        CurrentTime = 0;

        while (CurrentTime < TotalDuration)
        {
            CurrentTime += Time.deltaTime;
            Current01Value = Envelope.LerpEnvelope(CurrentTime, out EnemyBulletEnvelopeState status);
            currentStatus = status;
            yield return null;
        }

        Current01Value = 0;
        CurrentTime = TotalDuration;
        currentStatus = EnemyBulletEnvelopeState.END;

        CurrentCoroutine = null;
        CurrentTriggerSource = null;

        yield break;
    }

    IEnumerator CancelLerp()
    {
        float volumeAtCall = Current01Value;

        CurrentTime = 0;
        while (CurrentTime < cancelLerpDuration)
        {
            CurrentTime += Time.deltaTime;
            Current01Value = Mathf.Lerp(volumeAtCall, 0, CurrentTime / TotalDuration);
            currentStatus = EnemyBulletEnvelopeState.CANCELLING;
            yield return null;
        }

        Current01Value = 0;
        CurrentTime = TotalDuration;
        currentStatus = EnemyBulletEnvelopeState.END;

        yield break;
    }

    IEnumerator DelayFollowEnvelope(float duration, MonoBehaviour triggerSource)
    {
        yield return new WaitForSeconds(duration);
        CurrentCoroutine = triggerSource.StartCoroutine(FollowEnvelope());
        yield break;
    }
}
