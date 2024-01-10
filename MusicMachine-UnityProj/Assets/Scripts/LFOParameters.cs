using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLFOParameters", menuName = "Misc/LFOParameters")]
public class LFOParameters : ScriptableObject
{
    public WaveShape waveShape = WaveShape.Sin;
    public Texture2D texture;
    [Space]
    public float frequency = 1f;
    public float amplitude = 1f;
}
