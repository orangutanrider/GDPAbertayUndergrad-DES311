using System.Collections;
using UnityEngine;
using Unity.EditorCoroutines.Editor;

public class EnemyBulletEnvelopeEditorObj
{
    public EnemyBulletEnvelopeEditorObj(EnemyBulletEnvelope envelope)
    {
        Envelope = envelope;
    }

    [Range(0f, 1f)]
    [SerializeField] float magnitude = 1;
    [SerializeField] float holdTime = 0;
    [Space]
    [Range(0f, 1f)]
    [SerializeField] float attack;
    [Range(0f, 10f)]
    [SerializeField] float decay;
    [Range(0f, 1f)]
    [SerializeField] float sustain;
    [Range(0f, 10f)]
    [SerializeField] float release;

    #region Variables
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

    public const float coroutineUpdateRate = 0.01667f; //60Hz
    const float cancelLerpDuration = 0.1f;

    public EditorCoroutine CurrentCoroutine { get; private set; }
    public float Current01Value { get; private set; }
    public float CurrentTime { get; private set; }

    EnemyBulletEnvelopeState currentStatus = EnemyBulletEnvelopeState.END;
    public EnemyBulletEnvelopeState CurrentStatus()
    {
        return currentStatus;
    }
    #endregion

    public void TriggerEnvelopeCoroutineLerp()
    {
        // automatically handle the cancelling of that one
        if (CurrentCoroutine != null)
        {
            // cancel the current one (it's a lerp to avoid audio popping)
            EditorCoroutineUtility.StartCoroutine(CancelLerp(), this);

            // start a new one on a delay so that it starts as the cancel lerp ends
            IEnumerator delayedFollowEnvelope = DelayFollowEnvelope(cancelLerpDuration);
            CurrentCoroutine = EditorCoroutineUtility.StartCoroutine(delayedFollowEnvelope, this);

            return;
        }

        CurrentCoroutine = EditorCoroutineUtility.StartCoroutine(FollowEnvelope(), this);
    }

    public void InterruptEnvelopeLerp()
    {
        if(CurrentCoroutine == null) { return; }

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
            Current01Value = Envelope.LerpEnvelope(CurrentTime, out EnemyBulletEnvelopeState status);
            currentStatus = status;
            yield return updateRate;
        }

        Current01Value = 0;
        CurrentTime = TotalDuration;
        currentStatus = EnemyBulletEnvelopeState.END;

        CurrentCoroutine = null;

        yield break;
    }

    IEnumerator CancelLerp()
    {
        EditorWaitForSeconds updateRate = new EditorWaitForSeconds(coroutineUpdateRate);

        float volumeAtCall = Current01Value;

        CurrentTime = 0;
        while (CurrentTime < cancelLerpDuration)
        {
            CurrentTime += updateRate.WaitTime;
            Current01Value = Mathf.Lerp(volumeAtCall, 0, CurrentTime / TotalDuration);
            currentStatus = EnemyBulletEnvelopeState.CANCELLING;
            yield return updateRate;
        }

        Current01Value = 0;
        CurrentTime = TotalDuration;
        currentStatus = EnemyBulletEnvelopeState.END;

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
