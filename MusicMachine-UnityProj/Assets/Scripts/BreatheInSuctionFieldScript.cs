using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("OLD")]
public class BreatheInSuctionFieldScript : MonoBehaviour
{
    /*
    [Header("Required References")]
    public SpriteRenderer suctionFieldSprite;
    public BreathWeightManager breathWeightManager;
    [Space]
    public PlayerBreatheParameters breatheParameters;

    Color suctionFieldBaseColour = Color.magenta;
    bool suctionFieldActive = false;
    float tickTimer = 0;

    float SuctionFieldLength
    {
        get
        {
            AnimationCurve suctionLengthAgainstBreathePower = breatheParameters.suctionLengthAgainstBreathePower;
            return suctionLengthAgainstBreathePower.Evaluate(breathWeightManager.BreatheInPower);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        suctionFieldBaseColour = suctionFieldSprite.color;
        DeActivateSuctionField();
    }

    // Update is called once per frame
    void Update()
    {
        if (suctionFieldActive == false) { return; }

        transform.localScale = new Vector3(transform.localScale.x, SuctionFieldLength);

        // breathe-in tick
        float breatheInTickWindow = breatheParameters.breatheInTickWindow;
        float breatheInTickRate = breatheParameters.breatheInTickRate;
        tickTimer = tickTimer - Time.deltaTime;
        if (tickTimer < -breatheInTickWindow)
        {
            tickTimer = breatheInTickRate;
        }

        if(tickTimer > 0) { return; }
        float breatheInRadius = breatheParameters.breatheInRadius;
        RaycastHit2D[] rayHits = SuctionRaycast(breatheInRadius, SuctionFieldLength, -transform.up);
        foreach(RaycastHit2D rayHit in rayHits)
        {
            BreatheInHit(rayHit);
        }
    }

    RaycastHit2D[] SuctionRaycast(float radius, float length, Vector2 direction)
    {
        LayerMask breatheInterfaceMask = breatheParameters.breatheInterfaceMask;
        return Physics2D.CircleCastAll(transform.position, radius, direction, length, breatheInterfaceMask);
    }

    void BreatheInHit(RaycastHit2D rayHit)
    {
        float adjustedBreathePower = breathWeightManager.BreatheInPower * breatheParameters.breatheInPowerEffector;

        IBreatheInterface breatheInterface = rayHit.collider.GetComponent<IBreatheInterface>();
        if (breatheInterface != null)
        {
            breatheInterface.HitByBreatheIn(adjustedBreathePower);

            float breatheInTickRate = breatheParameters.breatheInTickRate;
            tickTimer = breatheInTickRate;
        }
    }

    #region Activate + De-activate
    public void ActivateSuctionField()
    {
        suctionFieldSprite.color = new Color(suctionFieldBaseColour.r, suctionFieldBaseColour.g, suctionFieldBaseColour.b, 0.5f);
        transform.localScale = new Vector3(transform.localScale.x, SuctionFieldLength);
        suctionFieldActive = true;
    }

    public void DeActivateSuctionField()
    {
        suctionFieldSprite.color = new Color(suctionFieldBaseColour.r, suctionFieldBaseColour.g, suctionFieldBaseColour.b, 0);
        suctionFieldActive = false;
    }
    #endregion
    */
}
