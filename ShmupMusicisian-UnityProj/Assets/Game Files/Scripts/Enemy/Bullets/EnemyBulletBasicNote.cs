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
        // Set Volume RTPC to envelope lerp value
        RTPC volume = bulletParams.volumeRTPC;
        volume.SetValue(gameObject, Mathf.Lerp(0, 100, envelope.Current01Value));

        // Read envelope status and stop synth if it is over
        if (envelope.CurrentStatus() == EnemyBulletEnvelopeState.END && isPlaying == true)
        {
            bulletParams.stopSynthEvent.Post(gameObject);
            isPlaying = false;
        }
    }

    void OnDestroy()
    {
        bulletParams.stopSynthEvent.Post(gameObject);
    }

    void IEnemyBulletActivatable.Activate()
    {
        // it probably would've been better to have somekind of synth class for handling the parameter linking part of this
        // then this could just call that and feed in it's own data, rather than this
        // which essentially limits this synth to always being used via EnemyBasicBulletParams
        // if it wasn't this way then there'd theoeretically only need to be one test script, while with this there'll need to be a custom one per musical audio component

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
        envelope.TriggerEnvelopeCoroutineLerp(this);
        volume.SetValue(gameObject, Mathf.Lerp(0, 100, envelope.Current01Value));
        playSynth.Post(gameObject);
        isPlaying = true;
    }
}
