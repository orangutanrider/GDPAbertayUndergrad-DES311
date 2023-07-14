using UnityEngine;
using AK.Wwise;

public class EnemyBulletBasicNote : EnemyBulletMusicalNote, IEnemyBulletActivatable
{
    // xPosition - Pitch
    // Angle - PWM
    // Speed - Transpose

    [Header("Required References")]
    [SerializeField] EnemyBasicBulletParams bulletParams;
    public EnemyBulletMovement movement;

    bool isPlaying = false;

    public override IEnemyBulletActivatable GetActivationInterface() { return this; }
    public override IEnemyBulletOnDeactivating GetOnDeactivatingInterface() { return null; }
    public override IEnemyBulletOnDeactivate GetOnDeactivationInterface() { return null; }

    private void Update()
    {
        // Get Params
        AK.Wwise.Event stopSynth = bulletParams.stopSynthEvent;
        RTPC volume = bulletParams.volumeRTPC;

        // Set Volume RTPC to envelope lerp value
        volume.SetValue(gameObject, Mathf.Lerp(0, 100, BulletEnvelope.envelopeObj.Current01Value));

        // Read envelope status and stop synth if it is over
        if (BulletEnvelope.envelopeObj.CurrentStatus() == EnemyBulletEnvelopeState.END && isPlaying == true)
        {
            stopSynth.Post(gameObject);
            isPlaying = false;
        }
    }

    void OnDestroy()
    {
        bulletParams.stopSynthEvent.Post(gameObject);
    }

    void IEnemyBulletActivatable.Activate()
    {
        // Get Params
        RTPC pitch = bulletParams.pitchRTPC;
        RTPC pwm = bulletParams.pwmRTPC;
        RTPC transpose = bulletParams.transposeRTPC;
        RTPC volume = bulletParams.volumeRTPC;
        AK.Wwise.Event playSynth = bulletParams.playSynthEvent;

        // for the vector2 range variables on the noteparams object x is min y is max
        // formula is: 01Range = (value - min) / (max - min)
        // the value is the variable being put into it (i.e. currentRotation, currentSpeed, currentXPosition, ect.)
        float xPositionRange01 = (transform.position.x - bulletParams.pitchXPositionRange.x) / (bulletParams.pitchXPositionRange.y - bulletParams.pitchXPositionRange.x);
        float angleRange01 = (bulletRoot.transform.rotation.eulerAngles.z - bulletParams.pwmAngleRange.x) / (bulletParams.pwmAngleRange.y - bulletParams.pwmAngleRange.x);
        float speedRange01 = (movement.Velocity.magnitude - bulletParams.transposeSpeedRange.x) / (bulletParams.transposeSpeedRange.y - bulletParams.transposeSpeedRange.x);

        // Set Values 
        pitch.SetValue(gameObject, Mathf.Lerp(0, 100, xPositionRange01));
        pwm.SetValue(gameObject, Mathf.Lerp(0, 100, angleRange01));
        transpose.SetValue(gameObject, Mathf.Lerp(0, 100, speedRange01));

        // Begin Envelope Volume Lerp
        BulletEnvelope.envelopeObj.TriggerEnvelopeCoroutineLerp(this);
        volume.SetValue(gameObject, Mathf.Lerp(0, 100, BulletEnvelope.envelopeObj.Current01Value));
        playSynth.Post(gameObject);
        isPlaying = true;
    }
}
