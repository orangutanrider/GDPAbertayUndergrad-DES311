using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreatheOutProjectileScript : MonoBehaviour
{
    public void ParseProjectileSpawningData(float _maxRange, Vector3 _targetDirection, float _projectileSpeed, float _breathePower, LayerMask _projectileHitMask, float _projectileSize)
    {
        maxRange = _maxRange;
        targetDirection = _targetDirection;
        projectileSpeed = _projectileSpeed;
        breathePower = _breathePower;
        projectileHitMask = _projectileHitMask;
        projectileSize = _projectileSize;
    }

    public void ParseExplosionSpawningData(GameObject _projectileExplosionPrefab, float _explosionSizeEffector, float _explosionForce, LayerMask _explosionHitMask)
    {
        projectileExplosionPrefab = _projectileExplosionPrefab;
        explosionSizeEffector = _explosionSizeEffector;
        explosionForce = _explosionForce;
        explosionHitMask = _explosionHitMask;
    }

    [Header("Required References")]
    public Transform spriteHolder;

    LayerMask projectileHitMask;
    Vector3 targetDirection = Vector3.zero;
    float projectileSpeed = 0;
    float breathePower = 0;
    float maxRange = 0;
    float projectileSize = 0;

    LayerMask explosionHitMask;
    GameObject projectileExplosionPrefab;
    float explosionSizeEffector = 0;
    float explosionForce = 0;

    float timeTillMaxRange = 0;
    float impactTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(projectileSize, projectileSize, 1);
        timeTillMaxRange = Vector3.Distance(transform.position, transform.position + new Vector3(maxRange, 0)) / projectileSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (targetDirection * projectileSpeed * Time.deltaTime);

        impactTimer = impactTimer + Time.deltaTime;
        if(impactTimer >= timeTillMaxRange)
        {
            Explode();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
        if (projectileHitMask == (projectileHitMask | (1 << collision.gameObject.layer)))
        {
            Explode();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
        if (projectileHitMask == (projectileHitMask | (1 << collision.gameObject.layer)))
        {
            Explode();
        }
    }

    void Explode()
    {
        GameObject explosionObject = Instantiate(projectileExplosionPrefab, transform.position, transform.rotation);
        float explosionSize = transform.localScale.x * explosionSizeEffector;
        explosionObject.transform.localScale = new Vector3(explosionSize, explosionSize, 1);
        explosionObject.GetComponent<BreatheOutExplosionScript>().ParseSpawningData(explosionSize, breathePower, explosionForce, explosionHitMask);
        Destroy(gameObject);
    }
}
