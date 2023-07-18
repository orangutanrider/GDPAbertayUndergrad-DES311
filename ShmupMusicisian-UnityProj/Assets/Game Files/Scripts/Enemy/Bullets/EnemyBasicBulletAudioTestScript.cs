using System.Collections;
using UnityEngine;
using AK.Wwise;
using Unity.EditorCoroutines.Editor;

[ExecuteInEditMode]
public class EnemyBasicBulletAudioTestScript : MonoBehaviour
{
    // This thing seems to break whenever script compilation happens.
    // It can be fixed by starting and stopping playmode, not sure what breaks it though.
    // I would imagine that it could be something to do with the soundbank not being loaded.
    // I have called the load() function on the soundbank but I'm not sure how that works.
    // It also could be something to do with the editor coroutines
    // As there is another bug that sometimes causes the tempWwisePoster to not get destroyed

    // i've tried doing proper loading now
    // i've tried registering the gameobject
    // i'vre tried initializing the sound engine
    // and i've tried reseting the default listeners
    // still got this same bug, annoying

    [Header("Required References")]
    [SerializeField] EnemyBulletEnvelopeParams envelopeParams;
    [SerializeField] EnemyBasicBulletParams bulletParams;
    public Bank bank;
    public AkAudioListener akListenerComponent;

    [Header("Params")]
    public float decoyVelocityMagnitude = 1;
    [Range(-180, 180)]
    public float decoyFiringAngle = 0;

    EditorCoroutine activeUpdate = null;
    GameObject tempWwisePoster = null;

    public void ExecuteTest()
    {
        if (activeUpdate != null)
        {
            EditorCoroutineUtility.StopCoroutine(activeUpdate);
        }

        if(tempWwisePoster != null)
        {
            bulletParams.stopSynthEvent.Post(tempWwisePoster);
            DestroyImmediate(tempWwisePoster);
        }

        tempWwisePoster = new GameObject("AudioTestScript - temp");
        AkGameObj tempAkObj = tempWwisePoster.AddComponent<AkGameObj>();
        AKRESULT registerResult = AkSoundEngine.RegisterGameObj(tempWwisePoster);
        Debug.Log("Wwise tempAkObj register results: " + registerResult);

        AKRESULT loadResult = AkSoundEngine.LoadBank(bank.Id);
        Debug.Log("Wwise bank load results: " + loadResult);

        akListenerComponent.StartListeningToEmitter(tempAkObj);

        // Get Params
        RTPC pitch = bulletParams.pitchRTPC;
        RTPC pwm = bulletParams.pwmRTPC;
        RTPC transpose = bulletParams.transposeRTPC;
        RTPC volume = bulletParams.volumeRTPC;
        AK.Wwise.Event playSynth = bulletParams.playSynthEvent;
        EnemyBulletEnvelopeEditorObj envelopeObj = new EnemyBulletEnvelopeEditorObj(envelopeParams.Envelope);

        // for the vector2 range variables on the noteparams object x is min y is max
        // formula is: 01Range = (value - min) / (max - min)
        // the value is the variable being put into it (i.e. currentRotation, currentSpeed, currentXPosition, ect.)
        float xPositionRange01 = (transform.position.x - bulletParams.pitchXPositionRange.x) / (bulletParams.pitchXPositionRange.y - bulletParams.pitchXPositionRange.x);
        float angleRange01 = (decoyFiringAngle - bulletParams.pwmAngleRange.x) / (bulletParams.pwmAngleRange.y - bulletParams.pwmAngleRange.x);
        float speedRange01 = (decoyVelocityMagnitude - bulletParams.transposeSpeedRange.x) / (bulletParams.transposeSpeedRange.y - bulletParams.transposeSpeedRange.x);

        // Set Values
        pitch.SetGlobalValue(Mathf.Lerp(0, 100, xPositionRange01));
        pwm.SetGlobalValue(Mathf.Lerp(0, 100, angleRange01));
        transpose.SetGlobalValue(Mathf.Lerp(0, 100, speedRange01));

        // Begin Envelope Volume Lerp
        envelopeObj.TriggerEnvelopeCoroutineLerp();
        volume.SetGlobalValue(Mathf.Lerp(0, 100, envelopeObj.Current01Value));
        playSynth.Post(tempWwisePoster);

        // Start an Update function
        IEnumerator editorUpdate = EditorUpdate(envelopeObj.TotalDuration, envelopeObj);
        activeUpdate = EditorCoroutineUtility.StartCoroutine(editorUpdate, this);
    }

    IEnumerator EditorUpdate(float duration, EnemyBulletEnvelopeEditorObj envelopeObj)
    {
        EditorWaitForSeconds updateRate = new EditorWaitForSeconds(EnemyBulletEnvelopeEditorObj.coroutineUpdateRate);
        float durationTimer = 0;
        while (durationTimer < duration)
        {
            // Set Volume RTPC to envelope lerp value
            RTPC volume = bulletParams.volumeRTPC;
            volume.SetGlobalValue(Mathf.Lerp(0, 100, envelopeObj.Current01Value));

            // tick
            durationTimer += updateRate.WaitTime;
            yield return updateRate;
        }
        
        bulletParams.stopSynthEvent.Post(tempWwisePoster);
        if (tempWwisePoster != null)
        {
            DestroyImmediate(tempWwisePoster);
        }
        yield break;
    }

    private void OnDestroy()
    {
        bulletParams.stopSynthEvent.Post(tempWwisePoster);
    }
}
