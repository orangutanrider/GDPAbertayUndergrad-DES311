using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreatheInWhirlwindScript : MonoBehaviour
{
    public void ParseSpawningData(float _windForce, float _breathePower, float _lifetime, int _totalHits, LayerMask _breatheInterfaceMask)
    {
        windForce = _windForce;
        breathePower = _breathePower;
        lifetime = _lifetime;
        totalHits = _totalHits;
        breatheInterfaceMask = _breatheInterfaceMask;
    }

    LayerMask breatheInterfaceMask;
    float lifetime = 0;
    int totalHits = 0;
    float breathePower = 0;

    float hitTimer = 0;
    int currentHit = 0;

    public Vector2 WindForce
    {
        get
        {
            return -transform.up * windForce;
        }
    }
    float windForce = 0;


    void Start()
    {
        lifetime = lifetime / totalHits;
        hitTimer = lifetime;
    }

    void Update()
    {
        hitTimer = hitTimer - Time.deltaTime;
        if(hitTimer > 0) { return; }

        hitTimer = lifetime;
        currentHit++;

        RaycastHit2D[] whirlwindHits = WhirlwindRaycast();
        foreach(RaycastHit2D whirlwindHit in whirlwindHits)
        {
            TryInterfaceHit(whirlwindHit);
        }

        if(currentHit == totalHits)
        {
            Destroy(gameObject);
        }
    }

    void TryInterfaceHit(RaycastHit2D rayHit)
    {
        IBreatheInterface breatheInterface = rayHit.collider.GetComponent<IBreatheInterface>();
        if (breatheInterface != null)
        {
            breatheInterface.HitByBreatheIn(breathePower / totalHits);
        }
    }

    RaycastHit2D[] WhirlwindRaycast()
    {
        return Physics2D.BoxCastAll(transform.position + (-transform.up * Mathf.Abs(transform.localScale.y) / 2), transform.localScale, Vector2.Angle(Vector2.down, -transform.up), -transform.up, 0, breatheInterfaceMask);
    }
}
