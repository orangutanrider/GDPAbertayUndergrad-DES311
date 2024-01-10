using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "sequencerWindowParameters", menuName = "Misc/SequencerWindowParameters")]
public class SequencerWindowParameters : ScriptableObject
{
    public float flickerScaleEffector = 0;
    [Space]
    public float baseOnValue = 0;
    public Color onColor;
    public Color offColor;
}
