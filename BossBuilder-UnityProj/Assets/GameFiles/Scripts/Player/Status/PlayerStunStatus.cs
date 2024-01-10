using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunStatus : MonoBehaviour
{
    public bool active = true;

    [Header("Status")]
    [ReadOnly]
    [SerializeField] bool isStunned = false;
    [ReadOnly]
    [SerializeField] float stunTimer = 0;
    [ReadOnly]
    [SerializeField] bool stunDuringAirborne = false;

    [Header("Required References")]
    [SerializeField] PlayerStatusParameters statusParameters;
    public Rigidbody2D rb2D;
    public PlayerMovement playerMovement;

    #region Variables
    public PlayerStatusParameters StatusParameters
    {
        get { return statusParameters; }
    }

    public bool IsStunned
    {
        get
        {
            return isStunned;
        }
    }

    public float StunTimer
    {
        get
        {
            return stunTimer;
        }
        set
        {
            stunTimer = value;

            if(value > 0)
            {
                ApplyStun();
            }
        }
    }

    public bool StunDuringAirborne
    {
        get
        {
            return stunDuringAirborne;
        }
        set
        {
            stunDuringAirborne = value;

            if (value == false || airborneStunFrameBuffer == true) { return; }
            // if they're being stunned and the frame buffer isn't already active, then:
            // stun them, and activate the frame buffer so that the system doesn't check for the grounded state to unstun them for the first few frames of them being stunned
            airborneStunFrameBuffer = true;
            IEnumerator frameBuffer = SetFrameBufferInXFrames(false, frameBufferFrames);
            StartCoroutine(frameBuffer);
            ApplyStun();
        }
    }

    bool airborneStunFrameBuffer = false;
    const int frameBufferFrames = 2;
    #endregion

    private void Update()
    {
        ProcessStunTimer();
        ProcessStunDuringAirborne();
    }

    void ApplyStun()
    {
        if (active == false) { return; }
        isStunned = true;
        playerMovement.Stunned = true;
    }

    void RemoveStun()
    {
        isStunned = false;
        playerMovement.Stunned = false; 
    }

    void ProcessStunTimer()
    {
        if(stunTimer == float.MinValue) { return; }

        stunTimer = stunTimer - Time.deltaTime;

        if(stunTimer <= 0)
        {
            stunTimer = float.MinValue;
            RemoveStun();
        }
    }

    void ProcessStunDuringAirborne()
    {
        if (airborneStunFrameBuffer == true || stunDuringAirborne == false) { return; }

        if (playerMovement.Grounded == true && playerMovement.LeavingGrounded == false)
        {
            stunDuringAirborne = false;
            RemoveStun();
        }
    }

    IEnumerator SetFrameBufferInXFrames(bool boolValue, int xFrames)
    {
        while (xFrames > 0)
        {
            xFrames--;
            yield return null;
        }
        airborneStunFrameBuffer = boolValue;
        yield break;
    }
}
