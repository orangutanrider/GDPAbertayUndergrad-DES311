using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerBulletScript : MonoBehaviour
{
    [Header("Required References")]
    public Rigidbody2D rb2D;
    [SerializeField] BasicPlayerBulletParams bulletParams;

    public bool Active
    {
        get
        {
            return active;
        }
    }
    bool active = true;

    // initialization data
    static bool initialized = false;
    static bool firingPointABFlipFlop = false; // false is A, true is B
    static Transform firingPointA = null;
    static Transform firingPointB = null;
    static GameObject playerObject;
    static EnemyHitManager hitManager;

    void Start()
    {
        DeActivateBullet();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleBulletCollision(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleBulletCollision(collision);
    }

    public void ShootBullet()
    {
        if (initialized == false)
        {
            Debug.LogWarning("BasicBullets haven't been initialized, a BasicBulletManager is usually what does this.");
            DeActivateBullet();
            return;
        }

        if (active == true)
        {
            Debug.LogWarning("BasicBullet is currently active and fired");
            return;
        }
        ActivateBullet();
    }

    void HandleBulletCollision(Collider2D collision)
    {
        // get params
        LayerMask enemyMask = bulletParams.enemyMask;
        LayerMask bulletBoundaryMask = bulletParams.bulletBoundaryMask;

        int layer = collision.gameObject.layer;

        // if enemy hit
        if ((enemyMask & (1 << layer)) != 0)
        {
            BullletEnemyHit(collision);
            return;
        }
        // if boundary hit
        if ((bulletBoundaryMask & (1 << layer)) != 0)
        {
            DeActivateBullet();
            return;
        }
    }

    void BullletEnemyHit(Collider2D collision)
    {
        if(initialized == false)
        {
            Debug.LogWarning("BasicBullets haven't been initialized, a BasicBulletManager is usually what does this.");
            DeActivateBullet();
            return;
        }

        // get enemy status
        EnemyStatusChanger enemyStatus = hitManager.GetEnemyStatusViaFormattedName(collision.gameObject.name);
        if(enemyStatus == null) { return; }

        // get params
        float damage = bulletParams.damage;

        // construct damage data
        AtEnemyDamageData damageData = new AtEnemyDamageData(damage, Vector2.up, transform.position, playerObject);

        // apply damage
        enemyStatus.ApplyDamage(damageData);
    }

    #region Object Pooling Stuff
    public static void InitializeBullets(Transform _firingPointA, Transform _firingPointB, EnemyHitManager _hitManager, GameObject _playerObject)
    {
        initialized = true;

        firingPointA = _firingPointA;
        firingPointB = _firingPointB;
        hitManager = _hitManager;
        playerObject = _playerObject;
    }

    void ActivateBullet()
    {
        // get params
        float speed = bulletParams.speed;

        // activate it
        active = true;
        gameObject.SetActive(true);

        // position it at a firing point
        firingPointABFlipFlop = !firingPointABFlipFlop;
        if (firingPointABFlipFlop == false)
        {
            //rb2D.MovePosition(firingPointA.position);
            transform.position = firingPointA.position;
        }
        else
        {
            //rb2D.MovePosition(firingPointB.position);
            transform.position = firingPointB.position;
        }

        // give it velocity
        rb2D.velocity = Vector2.up * speed;
    }

    public void DeActivateBullet()
    {
        active = false;
        gameObject.SetActive(false);
    }
    #endregion
}
