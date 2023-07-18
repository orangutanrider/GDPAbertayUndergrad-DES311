using UnityEngine;

public struct EnemyBulletEnvelope 
{
    //https://musictechstudent.co.uk/music-production-glossary/adsr/#:~:text=ADSR%20%E2%80%93%20Attack%2C%20Decay%2C%20Sustain,off%20to%20the%20sustain%20level.

    // attack is default volume 
    // it's value corresponds to how long the lerp takes to get to that volume 

    // decay is how long the lerp takes to go from attack to sustain

    // sustain is a multiply of the default volume, it's duration is determined by the holdTime

    // release is how long the lerp takes to go from sustain to no volume

    public float magnitude;

    public float holdTime;

    public float attack;
    public float decay;
    public float sustain;
    public float release;

    public EnemyBulletEnvelope(float _magnitude, float _holdTime, float _attack, float _decay, float _sustain, float _release)
    {
        magnitude = _magnitude;
        holdTime = _holdTime;
        attack = _attack;
        sustain = _sustain;
        decay = _decay;
        release = _release;
    }

    // returns value between 0 and 1, t should be given as real values (e.g. this will not return the end value at t = 1)
    public float LerpEnvelope(float t, out EnemyBulletEnvelopeState status)
    {
        // negative t values
        if(t < 0)
        {
            status = EnemyBulletEnvelopeState.NEGATIVE;
            return 0 * magnitude;
        }

        float totalDuration = attack;

        // attack
        if(t < totalDuration)
        {
            float volAttack = Mathf.Lerp(0, 1, t / totalDuration); // t / attack
            status = EnemyBulletEnvelopeState.Attack;
            return volAttack * magnitude;
        }

        totalDuration += decay;

        // decay
        if (t < totalDuration)
        {
            float volDecay = Mathf.Lerp(1, sustain, t / totalDuration); // t / (attack + decay)
            status = EnemyBulletEnvelopeState.Decay;
            return volDecay * magnitude;
        }

        totalDuration += holdTime;

        // sustain
        if(t < totalDuration)
        {
            float volSustain = Mathf.Lerp(1, sustain, t / totalDuration); // t / (attack + decay + holdTime)
            status = EnemyBulletEnvelopeState.Sustain;
            return volSustain * magnitude;
        }

        totalDuration += release;

        // release
        if(t < totalDuration)
        {
            float volRelease = Mathf.Lerp(sustain, 0, t / totalDuration); // t / (attack + decay + holdTime + release)
            status = EnemyBulletEnvelopeState.Release;
            return volRelease * magnitude;
        }

        // End
        status = EnemyBulletEnvelopeState.END;
        return 0 * magnitude;
    }
}
