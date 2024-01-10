using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerGoombaStompParameters", menuName = "Player/GoombaStompParameters")]
public class PlayerGoombaStompParameters : ScriptableObject
{
    public float stompDamage = 0;
    [Space]
    public float stompJumpPower = 0;
    public float stompJumpXPower = 0;
    [Space]
    [Tooltip("Below the player's yPostion plus this offset is what gets treated as the player's feet. What that means, is that goombaStomp rayhits above that location, don't count.")]
    public float stompPointOffset = 0;
}
