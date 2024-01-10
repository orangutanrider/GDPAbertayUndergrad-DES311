using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillKnobScript : MonoBehaviour, IBreatheInterface
{
    [Header("Required References")]
    public Transform millTransform;
    public Transform dialTransform;

    [Header("Parameters")]
    public float dialAngleLimit;
    public float dialWeight;
    public float directionPushBackMultiplier;
    [Space]
    public float negativeMillSpinBias = 2;
    [Space]
    public AnimationCurve millSpeedFallOffCurve;
    public AnimationCurve millSpeedAgainstSpinEnergy;
    public AnimationCurve millSpinEnergyBonusAgainstPowerInput;

    float CurrentPositiveSpin
    {
        get
        { 
            return positiveSpinAtHit * positiveFallOffValue; 
        }
    }
    float CurrentNegativeSpin
    {
        get
        {
            return negativeSpinAtHit * negativeFallOffValue;
        }
    }

    int spinDirection = 0;

    // positive (breathe-out)
    float positiveEnergy = 0;
    float positiveFallOffValue = 1;
    float positiveSpinAtHit = 0;
    float positiveMillSpinEnergyBonus = 0;

    // negative (breathe-in)
    float negativeEnergy = 0;
    float negativeFallOffValue = 0;
    float negativeSpinAtHit = 0;
    float negativeMillSpinEnergyBonus = 0;

    float IBreatheInterface.KnobValue
    {
        get
        {
            return knobValue;
        }
        set
        {
            knobValue = value;
        }
    }
    float knobValue = 0;

    void IBreatheInterface.HitByBreatheIn(float breathePower)
    {
        if(spinDirection == 0) { spinDirection = -1; }

        negativeEnergy = negativeEnergy + breathePower;
        negativeSpinAtHit = CurrentNegativeSpin + (millSpeedAgainstSpinEnergy.Evaluate(breathePower) * negativeMillSpinBias);
        negativeFallOffValue = millSpeedFallOffCurve.Evaluate(1);
        negativeMillSpinEnergyBonus = millSpinEnergyBonusAgainstPowerInput.Evaluate(breathePower);
        positiveMillSpinEnergyBonus = 0;
    }

    void IBreatheInterface.HitByBreatheOut(float breathePower)
    {
        if (spinDirection == 0) { spinDirection = 1; }

        positiveEnergy = positiveEnergy + breathePower;
        positiveSpinAtHit = CurrentPositiveSpin + millSpeedAgainstSpinEnergy.Evaluate(breathePower);
        positiveFallOffValue = millSpeedFallOffCurve.Evaluate(1);
        positiveMillSpinEnergyBonus = millSpinEnergyBonusAgainstPowerInput.Evaluate(breathePower);
        negativeMillSpinEnergyBonus = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(spinDirection == 0) { return; }

        UpdateSpinDirection();
        UpdateNegativeSpin();
        UpdatePositiveSpin();

        ReturnActiveSpinData(out float spin, out float fallOffValue, out float energy);

        Debug.Log(spinDirection);

        // mill spin
        float rotationSpeed = spin * fallOffValue;
        millTransform.Rotate(new Vector3(0, 0, rotationSpeed));

        if(energy <= 0) { return; }

        // dial spin
        knobValue = Mathf.Clamp(knobValue + ((Time.deltaTime / dialWeight) * spinDirection), -1f, 1f);
        float eulerRotation = knobValue * dialAngleLimit;
        dialTransform.transform.eulerAngles = new Vector3(0, 0, eulerRotation);
    }

    void ReturnActiveSpinData(out float spin, out float fallOffValue, out float energy)
    {
        fallOffValue = 0;
        spin = 0;
        energy = 0;

        if (spinDirection == 1)
        {
            spin = positiveSpinAtHit;
            fallOffValue = positiveFallOffValue;
            energy = positiveEnergy;
            return;
        }
        if (spinDirection == -1)
        {
            spin = -negativeSpinAtHit;
            fallOffValue = negativeFallOffValue;
            energy = negativeEnergy;
            return;
        }
    }

    void UpdateSpinDirection()
    {
        // (it locks to the current direction until that direction has no energy left)
        if (negativeEnergy + negativeMillSpinEnergyBonus <= 0 && positiveEnergy > 0)
        {
            spinDirection = 1;
            negativeMillSpinEnergyBonus = 0;
        }
        if (positiveEnergy + positiveMillSpinEnergyBonus <= 0 && negativeEnergy > 0)
        {
            spinDirection = -1;
            positiveMillSpinEnergyBonus = 0;
        }
        if (positiveEnergy + positiveMillSpinEnergyBonus <= 0 && negativeEnergy + negativeMillSpinEnergyBonus <= 0)
        {
            spinDirection = 0;
            positiveMillSpinEnergyBonus = 0;
            negativeMillSpinEnergyBonus = 0;
        }
    }

    void UpdateNegativeSpin()
    {
        if (negativeEnergy + negativeMillSpinEnergyBonus <= 0) 
        {
            negativeFallOffValue = 0;
            negativeMillSpinEnergyBonus = 0;
            return;
        }

        if (spinDirection == -1 && positiveEnergy > 0)
        {
            negativeEnergy = negativeEnergy - (Time.deltaTime * directionPushBackMultiplier);
        }
        else
        {
            negativeEnergy = negativeEnergy - Time.deltaTime;
        }

        float fallOffValue = millSpeedFallOffCurve.Evaluate((negativeEnergy + negativeMillSpinEnergyBonus) / negativeSpinAtHit);
        negativeFallOffValue = fallOffValue;
    }
    
    void UpdatePositiveSpin()
    {
        if (positiveEnergy + positiveMillSpinEnergyBonus <= 0) 
        {
            positiveFallOffValue = 0;
            positiveMillSpinEnergyBonus = 0;
            return; 
        }

        if (spinDirection == 1 && negativeEnergy > 0)
        {
            positiveEnergy = positiveEnergy - (Time.deltaTime * directionPushBackMultiplier);
        }
        else
        {
            positiveEnergy = positiveEnergy - Time.deltaTime;
        }

        float fallOffValue = millSpeedFallOffCurve.Evaluate((positiveEnergy + positiveMillSpinEnergyBonus) / positiveSpinAtHit);
        positiveFallOffValue = fallOffValue;
    }
}
