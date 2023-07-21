using UnityEngine;

[CreateAssetMenu(fileName = "EnvelopeParams", menuName = "Audio/EnvelopeParams")]
public class EnvelopeParams : ScriptableObject
{
    public float magnitude = 1;
    public float holdTime = 0;
    [Space]
    [Range(0f, 1f)] public float attack = 0.1f;
    [Range(0f, 10f)] public float decay = 1f;
    [Range(0f, 1f)] public float sustain = 0.66f;
    [Range(0f, 10f)] public float release = 1f;

    public Envelope GetDataCopy()
    {
        return new Envelope(magnitude, holdTime, attack, decay, sustain, release);
    }
}
