using UnityEngine;

[System.Serializable]
public struct Envelope 
{
    //https://musictechstudent.co.uk/music-production-glossary/adsr/#:~:text=ADSR%20%E2%80%93%20Attack%2C%20Decay%2C%20Sustain,off%20to%20the%20sustain%20level.

    public Envelope(float _magnitude, float _holdTime, float _attack, float _decay, float _sustain, float _release)
    {
        magnitude = _magnitude;
        holdTime = _holdTime;

        attack = _attack;
        sustain = _sustain;
        decay = _decay;
        release = _release;
    }

    public Envelope(Envelope envelope)
    {
        magnitude = envelope.magnitude;
        holdTime = envelope.holdTime;
        attack = envelope.attack;
        decay = envelope.decay;
        sustain = envelope.sustain;
        release = envelope.release;
    }

    public float magnitude;
    public float holdTime;

    public float attack;
    public float decay;
    public float sustain;
    public float release;

    public float TotalDuration
    {
        get
        {
            return attack + decay + holdTime + release;
        }
    }

    // Returns value between 0 and 1 that gets multiplied by the magnitude
    // t should be given as real values (e.g. this will not return the end value at t = 1)
    public float LerpEnvelope(float t, out EnvelopeState state)
    {
        // negative t values
        if (t < 0)
        {
            state = EnvelopeState.NEGATIVE;
            return 0 * magnitude;
        }

        float duration = attack;

        // attack
        if (t < duration)
        {
            float attackLevel = Mathf.Lerp(0, 1, t / duration); // t / attack
            state = EnvelopeState.Attack;
            return attackLevel * magnitude;
        }

        duration += decay;

        // decay
        if (t < duration)
        {
            float decayLevel = Mathf.Lerp(1, sustain, t / duration); // t / (attack + decay)
            state = EnvelopeState.Decay;
            return decayLevel * magnitude;
        }

        duration += holdTime;

        // sustain
        if (t < duration)
        {
            float sustainLevel = Mathf.Lerp(1, sustain, t / duration); // t / (attack + decay + holdTime)
            state = EnvelopeState.Sustain;
            return sustainLevel * magnitude;
        }

        duration += release;

        // release
        if (t < duration)
        {
            float releaseLevel = Mathf.Lerp(sustain, 0, t / duration); // t / (attack + decay + holdTime + release)
            state = EnvelopeState.Release;
            return releaseLevel * magnitude;
        }

        // End
        state = EnvelopeState.END;
        return 0 * magnitude;
    }
}
