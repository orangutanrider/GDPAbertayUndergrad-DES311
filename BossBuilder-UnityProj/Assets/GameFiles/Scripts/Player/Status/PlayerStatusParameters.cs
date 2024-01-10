using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatusParameters", menuName = "Player/StatusParameters")]
public class PlayerStatusParameters : ScriptableObject
{
    public float maxHealth = 3f;
    [Space]
    public bool knockbackStunImmunity = false;
}
