using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBulletEnvelope", menuName = "Enemy/BulletEnvelope")]
public class EnemyBulletEnvelopeParams : ScriptableObject
{
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
}
