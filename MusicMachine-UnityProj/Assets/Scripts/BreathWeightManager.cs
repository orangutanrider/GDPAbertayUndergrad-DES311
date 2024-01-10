using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathWeightManager : MonoBehaviour
{
    public bool debugMode = false;

    [Header("Parameters")]
    public PlayerBreatheParameters breatheParameters;

    float breathWeight = 0; // positive is too much breathing out, negative is too much breathing in

    public float BreatheOutPower
    {
        get
        {
            AnimationCurve powerAgainstWeight = breatheParameters.powerAgainstWeight;

            float breatheInPower = powerAgainstWeight.Evaluate(Mathf.Abs(breathWeight));
            if (breathWeight < 0)
            {
                breatheInPower = powerAgainstWeight.Evaluate(-Mathf.Abs(breathWeight));
            }

            return breatheInPower;
        }
    }

    public float BreatheInPower
    {
        get
        {
            AnimationCurve powerAgainstWeight = breatheParameters.powerAgainstWeight;

            float breatheInPower = powerAgainstWeight.Evaluate(Mathf.Abs(breathWeight));
            if (breathWeight > 0)
            {
                breatheInPower = powerAgainstWeight.Evaluate(-Mathf.Abs(breathWeight));
            }

            return breatheInPower;
        }
    }

    // Update breathe weight
    public void BreatheOutWeightUpdate()
    {
        AnimationCurve weightCostCurve = breatheParameters.weightCostCurve;

        breathWeight = breathWeight + weightCostCurve.Evaluate(breathWeight);
        breathWeight = Mathf.Clamp(breathWeight, -1, 1);
    }

    public void BreatheInWeightUpdate()
    {
        AnimationCurve weightCostCurve = breatheParameters.weightCostCurve;

        breathWeight = breathWeight - weightCostCurve.Evaluate(breathWeight);
        breathWeight = Mathf.Clamp(breathWeight, -1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(debugMode == false) { return; }
    }
}
