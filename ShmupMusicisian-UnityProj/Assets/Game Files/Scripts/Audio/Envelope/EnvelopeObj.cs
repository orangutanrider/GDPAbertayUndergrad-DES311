using System.Collections;
using UnityEngine;

public class EnvelopeObj 
{
    public EnvelopeObj() { }

    public EnvelopeObj(Envelope envelope)
    {
        Envelope = envelope;
    }

    [SerializeField] float magnitude = 1;
    [SerializeField] float holdTime = 0;
    [Space] 
    [Range(0f, 1f)] [SerializeField] float attack = 0.1f;
    [Range(0f, 10f)] [SerializeField] float decay = 1f;
    [Range(0f, 1f)] [SerializeField] float sustain = 0.66f;
    [Range(0f, 10f)] [SerializeField] float release = 1f;

    #region Variables
    public EnvelopeEditorObj GetEditorEnvelope()
    {
        return new EnvelopeEditorObj(Envelope);
    }
 
    public Envelope Envelope
    {
        get
        {
            return new Envelope(magnitude, holdTime, attack, decay, sustain, release);
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

    const float cancelTime = 0.1f;

    public Coroutine CurrentCoroutine { get; private set; }
    public MonoBehaviour CurrentTriggerSource { get; private set; }
    public float Current01Value { get; private set; }
    public float CurrentTime { get; private set; }

    EnvelopeState currentState = EnvelopeState.END;
    public EnvelopeState CurrentState() { return currentState; }
    #endregion

    public void TriggerEnvelopeCoroutine(MonoBehaviour triggerSource)
    {
        // if there is an active one cancel it 
        if (CurrentCoroutine != null)
        {
            // cancel the current one (it's a lerp to avoid audio popping)
            triggerSource.StartCoroutine(CancelLerp());

            // start a new one on a delay so that it starts as the cancel lerp ends
            IEnumerator delayedFollowEnvelope = DelayFollowEnvelope(cancelTime, triggerSource);
            CurrentCoroutine = triggerSource.StartCoroutine(delayedFollowEnvelope);

            CurrentTriggerSource = triggerSource;
            return;
        }

        Current01Value = 0;
        CurrentTime = 0;
        currentState = EnvelopeState.Attack;

        CurrentTriggerSource = triggerSource;
        CurrentCoroutine = triggerSource.StartCoroutine(FollowEnvelope());
    }

    public void InterruptEnvelope(MonoBehaviour triggerSource)
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
            Current01Value = Envelope.LerpEnvelope(CurrentTime, out EnvelopeState state);
            currentState = state;
            yield return null;
        }

        Current01Value = 0;
        CurrentTime = TotalDuration;
        currentState = EnvelopeState.END;

        CurrentCoroutine = null;
        CurrentTriggerSource = null;

        yield break;
    }

    IEnumerator CancelLerp()
    {
        float volumeAtCall = Current01Value;

        CurrentTime = 0;
        while (CurrentTime < cancelTime)
        {
            CurrentTime += Time.deltaTime;
            Current01Value = Mathf.Lerp(volumeAtCall, 0, CurrentTime / TotalDuration);
            currentState = EnvelopeState.CANCELLING;
            yield return null;
        }

        Current01Value = 0;
        CurrentTime = TotalDuration;
        currentState = EnvelopeState.END;

        yield break;
    }

    IEnumerator DelayFollowEnvelope(float duration, MonoBehaviour triggerSource)
    {
        yield return new WaitForSeconds(duration);
        CurrentCoroutine = triggerSource.StartCoroutine(FollowEnvelope());
        yield break;
    }
}
