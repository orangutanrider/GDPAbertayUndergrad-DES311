using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBreatheController : MonoBehaviour
{
    [Header("Required References")]
    public BreathWeightManager breathWeightManager;

    [Header("Parameters")]
    public PlayerBreatheParameters breatheParameters;
    public BreatheOutBoomParameters breatheOutBoomParameters;
    public BreatheInWhirlwindParameters breatheInWhirlwindParameters;

    void Update()
    {
        // breathe input
        if (Input.GetMouseButtonDown(1) == true)
        {
            BreatheIn(GetMouseWorldPosition(), breathWeightManager.BreatheInPower);
            return;
        }

        if (Input.GetMouseButtonDown(0) == true)
        {
            BreatheOut(GetMouseWorldPosition(), breathWeightManager.BreatheOutPower);
            return;
        }
    }

    void WheezeCough()
    {

    }

    void WheezeDeflate()
    {

    }

    void BreatheOut(Vector3 targetPostion, float breathePower)
    {
        // can't breathe out if weight is so lop-sided that there's no breathe power 
        if (breathePower <= 0)
        {
            WheezeCough();
            return;
        }

        // load player breathe parameters
        float breatheOutProjectileSpawnOffset = breatheParameters.breatheOutProjectileSpawnOffset;
        GameObject breatheOutProjectilePrefab = breatheParameters.breatheOutProjectilePrefab;

        // load boom parameters (projectile)
        LayerMask projectileHitMask = breatheOutBoomParameters.projectileHitMask;
        AnimationCurve projectileRangeAgainstPower = breatheOutBoomParameters.projectileRangeAgainstPower;
        AnimationCurve projectileSizeAgainstPower = breatheOutBoomParameters.projectileSizeAgainstPower;
        float projectileSpeed = breatheOutBoomParameters.projectileSpeed;

        // load boom parameters (explosion)
        GameObject explosionPrefab = breatheOutBoomParameters.explosionPrefab;
        AnimationCurve explosionForceAgainstPower = breatheOutBoomParameters.explosionForceAgainstPower;
        LayerMask explosionHitMask = breatheOutBoomParameters.explosionHitMask;
        float explosionSizeEffector = breatheOutBoomParameters.explosionSizeEffector;

        // update breathe weight
        breathWeightManager.BreatheOutWeightUpdate();

        // instantiate breathe object at postion + (targetdirection * offset) and rotation
        Vector3 targetDirection = (targetPostion - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection, new Vector3(0, 0, 1));
        Quaternion breatheRotation = new Quaternion(0, 0, lookRotation.z, lookRotation.w);
        GameObject breatheProjectile = Instantiate(breatheOutProjectilePrefab, transform.position + (targetDirection * breatheOutProjectileSpawnOffset), breatheRotation);

        // set-up breathe projectile
        BreatheOutProjectileScript projectileScript = breatheProjectile.GetComponent<BreatheOutProjectileScript>();
        float explosionForce = explosionForceAgainstPower.Evaluate(breathePower);
        float maxRange = projectileRangeAgainstPower.Evaluate(breathePower);
        float breatheProjectileSize = projectileSizeAgainstPower.Evaluate(breathePower);
        projectileScript.ParseProjectileSpawningData(maxRange, targetDirection, projectileSpeed, breathePower, projectileHitMask, breatheProjectileSize);
        projectileScript.ParseExplosionSpawningData(explosionPrefab, explosionSizeEffector, explosionForce, explosionHitMask);
    }

    void BreatheIn(Vector3 targetPostion, float breathePower)
    {
        // can't breathe in if weight is lop-sided against breathe in
        if (breathePower <= 0)
        {
            WheezeDeflate();
            return;
        }

        // load player breathe parameters
        GameObject breatheInWhirlwindPrefab = breatheParameters.breatheInWhirlwindPrefab;
        float breatheInWhirlwindSpawnOffset = breatheParameters.breatheInWhirlwindSpawnOffset;

        // load whirlwind parameters
        AnimationCurve forceStrengthAgainstBreathePower = breatheInWhirlwindParameters.forceStrengthAgainstBreathePower;
        AnimationCurve lengthAgainstBreathePower = breatheInWhirlwindParameters.lengthAgainstBreathePower;
        AnimationCurve lifetimeAgainstPower = breatheInWhirlwindParameters.lifetimeAgainstPower;
        AnimationCurve totalHitsAgainstPower = breatheInWhirlwindParameters.totalHitsAgainstPower;
        LayerMask breatheInterfaceMask = breatheInWhirlwindParameters.breatheInterfaceMask;

        // update breathe weight
        breathWeightManager.BreatheInWeightUpdate();

        // instantiate breathe object at postion + (targetdirection * offset) and rotation
        Vector3 targetDirection = (targetPostion - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection, new Vector3(0, 0, 1));
        Quaternion breatheRotation = new Quaternion(0, 0, lookRotation.z, lookRotation.w);
        GameObject whirlwind = Instantiate(breatheInWhirlwindPrefab, transform.position + (targetDirection * breatheInWhirlwindSpawnOffset), breatheRotation);

        // set-up whirlwind
        BreatheInWhirlwindScript whirlwindScript = whirlwind.GetComponent<BreatheInWhirlwindScript>();
        float windForce = forceStrengthAgainstBreathePower.Evaluate(breathePower);
        float whirlwindLength = lengthAgainstBreathePower.Evaluate(breathePower);
        float lifetime = lifetimeAgainstPower.Evaluate(breathePower);
        int totalHits = Mathf.FloorToInt(totalHitsAgainstPower.Evaluate(breathePower));
        whirlwind.transform.localScale = new Vector3(whirlwind.transform.localScale.x, whirlwindLength);
        whirlwindScript.ParseSpawningData(windForce, breathePower, lifetime, totalHits, breatheInterfaceMask);
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        return mousePosition;
    }
}
