using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreatheOutExplosionScript : MonoBehaviour
{
    public void ParseSpawningData(float _rayCastRadius, float _breathePower, float _explosionForce, LayerMask _raycastMask)
    {
        rayCastRadius = _rayCastRadius;
        breathePower = _breathePower;
        explosionForce = _explosionForce;
        raycastMask = _raycastMask;
    }

    float rayCastRadius = 1;
    float breathePower = 1;
    float explosionForce = 1;
    LayerMask raycastMask;

    const float destroyInX = 0.75f;

    void Start()
    {
        StartCoroutine(DestroyInX(destroyInX));

        RaycastHit2D[] explosionCast = ExplosionRaycast(rayCastRadius);
        foreach (RaycastHit2D explosionHit in explosionCast)
        {
            ExplosionInterfaceHit(explosionHit);

            if(explosionHit.collider.tag == TagReferences.playerTag)
            {
                ExplosionPlayerHit(explosionHit);
            }
        }
    }

    void ExplosionInterfaceHit(RaycastHit2D rayHit)
    {
        IBreatheInterface breatheInterface = rayHit.collider.GetComponent<IBreatheInterface>();
        if (breatheInterface != null)
        {
            breatheInterface.HitByBreatheOut(breathePower);
        }
    }

    void ExplosionPlayerHit(RaycastHit2D rayHit)
    {
        rayHit.collider.GetComponent<PlayerExternalMovement>().BreatheOutExplosion(transform.position, explosionForce);
    }

    RaycastHit2D[] ExplosionRaycast(float radius)
    {
        return Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, 0, raycastMask);
    }

    IEnumerator DestroyInX(float x)
    {
        yield return new WaitForSeconds(x);
        Destroy(gameObject);
    }
}
