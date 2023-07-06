using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerBulletScript : MonoBehaviour
{
    [Header("Required References")]
    public GameObject bulletRoot;
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
    static PlayerBasicBulletManager bulletManager = null;
    static GameObject playerRootObject;

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
        if (initialized == false)
        {
            Debug.LogWarning("BasicBullets haven't been initialized, a BasicBulletManager is usually what does this.");
            DeActivateBullet();
            return;
        }

        // get params
        LayerMask enemyMask = bulletParams.enemyMask;
        LayerMask bulletBoundaryMask = bulletParams.bulletBoundaryMask;

        int layer = collision.gameObject.layer;

        // if enemy hit
        if ((enemyMask & (1 << layer)) != 0)
        {
            bulletManager.RegisterBulletEnemyHit(collision, this);
            return;
        }
        // if boundary hit
        if ((bulletBoundaryMask & (1 << layer)) != 0)
        {
            DeActivateBullet();
            return;
        }
    }

    #region Object Pooling Stuff
    public static void InitializeBullets(Transform _firingPointA, Transform _firingPointB,PlayerBasicBulletManager _bulletManager, GameObject _playerRootObject)
    {
        initialized = true;
  
        firingPointA = _firingPointA;
        firingPointB = _firingPointB;
        bulletManager = _bulletManager;
        playerRootObject = _playerRootObject;
    }

    void ActivateBullet()
    {
        // get params
        float speed = bulletParams.speed;

        // activate it
        active = true;
        bulletRoot.SetActive(true);

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
        bulletRoot.SetActive(false);
    }
    #endregion
}
