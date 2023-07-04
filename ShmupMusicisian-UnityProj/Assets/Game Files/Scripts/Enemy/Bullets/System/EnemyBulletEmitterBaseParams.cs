using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBulletBaseParams", menuName = "Enemy/BulletEmitterBaseParams")]
public class EnemyBulletEmitterBaseParams : ScriptableObject
{
    // Even though there is a emissionTimerMultiply param in here (that can be used to control emission rate)
    // It is preferable to create addutional params (for controlling emission rate) inside the tailored scriptable objects, created for custom emitters
    // As the intention isn't for the dev to create one of these per emitter they create, though you can do this if you want.

    public int maxConcurrentBullets;
    public float emissionTimerMultiply;
}
