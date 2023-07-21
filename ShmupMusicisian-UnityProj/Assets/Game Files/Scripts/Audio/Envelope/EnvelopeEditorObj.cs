using System.Collections;
using UnityEngine;
using Unity.EditorCoroutines.Editor;

[ExecuteInEditMode]
public class EnvelopeEditorObj 
{
    public EnvelopeEditorObj(Envelope envelope)
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

    public const float coroutineUpdateRate = 0.01667f; //60Hz
    const float cancelTime = 0.1f;

    public EditorCoroutine CurrentCoroutine { get; private set; }
    public float Current01Value { get; private set; }
    public float CurrentTime { get; private set; }

    EnvelopeState currentState = EnvelopeState.END;
    public EnvelopeState CurrentState() { return currentState; }
    #endregion

    public void TriggerEnvelopeCoroutine()
    {
        // if there is an active one cancel it 
        if (CurrentCoroutine != null)
        {
            // cancel the current one (it's a lerp to avoid audio popping)
            EditorCoroutineUtility.StartCoroutine(CancelLerp(), this);

            // start a new one on a delay so that it starts as the cancel lerp ends
            IEnumerator delayedFollowEnvelope = DelayFollowEnvelope(cancelTime);
            CurrentCoroutine = EditorCoroutineUtility.StartCoroutine(delayedFollowEnvelope, this);

            return;
        }

        CurrentCoroutine = EditorCoroutineUtility.StartCoroutine(FollowEnvelope(), this);
    }

    public void InterruptEnvelope()
    {
        if (CurrentCoroutine == null) { return; }

        EditorCoroutineUtility.StopCoroutine(CurrentCoroutine);
        CurrentCoroutine = null;

        EditorCoroutineUtility.StartCoroutine(CancelLerp(), this);
    }

    IEnumerator FollowEnvelope()
    {
        EditorWaitForSeconds updateRate = new EditorWaitForSeconds(coroutineUpdateRate);

        Current01Value = 0;
        CurrentTime = 0;

        while (CurrentTime < TotalDuration)
        {
            CurrentTime += updateRate.WaitTime;
            Current01Value = Envelope.LerpEnvelope(CurrentTime, out EnvelopeState state);
            currentState = state;
            yield return updateRate;
        }

        Current01Value = 0;
        CurrentTime = TotalDuration;
        currentState = EnvelopeState.END;

        CurrentCoroutine = null;

        yield break;
    }

    IEnumerator CancelLerp()
    {
        EditorWaitForSeconds updateRate = new EditorWaitForSeconds(coroutineUpdateRate);

        float volumeAtCall = Current01Value;

        CurrentTime = 0;
        while (CurrentTime < cancelTime)
        {
            CurrentTime += updateRate.WaitTime;
            Current01Value = Mathf.Lerp(volumeAtCall, 0, CurrentTime / TotalDuration);
            currentState = EnvelopeState.CANCELLING;
            yield return updateRate;
        }

        Current01Value = 0;
        CurrentTime = TotalDuration;
        currentState = EnvelopeState.END;

        yield break;
    }

    IEnumerator DelayFollowEnvelope(float duration)
    {
        EditorWaitForSeconds waitForDuration = new EditorWaitForSeconds(duration);
        yield return waitForDuration;

        CurrentCoroutine = EditorCoroutineUtility.StartCoroutine(FollowEnvelope(), this);
        yield break;
    }
}
