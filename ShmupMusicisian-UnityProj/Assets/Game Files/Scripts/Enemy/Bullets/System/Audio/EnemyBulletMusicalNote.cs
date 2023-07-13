using UnityEngine;

public abstract class EnemyBulletMusicalNote : EnemyBulletComponent
{
    [Header("(Base) Required References")]
    [SerializeField] EnemyBulletEnvelopeParams bulletEnvelope;
    public GameObject bulletRoot; // root of the prefab instance, not the absolute root in the scene.

    [Header("(Base) Envelope Settings")]
    public bool overrideHoldTime = false;
    public float holdTimeOverride = 0;

    public EnemyBulletEnvelopeParams BulletEnvelope
    {
        get { return bulletEnvelope; }
    }
}
