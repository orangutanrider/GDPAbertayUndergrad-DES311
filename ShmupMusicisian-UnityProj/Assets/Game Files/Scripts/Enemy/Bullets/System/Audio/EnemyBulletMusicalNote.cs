using UnityEngine;

public abstract class EnemyBulletMusicalNote : EnemyBulletComponent
{
    [Header("(Base) Required References")]
    [SerializeField] private EnemyBulletEnvelopeParams bulletEnvelope;
    public GameObject bulletRoot; // root of the prefab instance, not the absolute root in the scene.

    [Header("(Base) Envelope Settings")]
    public bool overrideHoldTime = false;
    public float holdTimeOverride = 0;

    private void Awake()
    {
        envelope = new EnemyBulletEnvelopeObj(bulletEnvelope.Envelope);
    }
    protected EnemyBulletEnvelopeObj envelope = null;
}
