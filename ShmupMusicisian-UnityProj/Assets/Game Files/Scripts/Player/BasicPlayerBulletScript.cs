using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerBulletScript : MonoBehaviour
{
    [Header("Required References")]
    public Rigidbody2D rb2D;
    public BasicPlayerBulletParams bulletParams;

    static Transform firingPointA = null;
    static Transform firingPointB = null;
    static bool firingPointABFlipFlop = false; // false is A, true is B

    public bool Active
    {
        get
        {
            return active;
        }
    }
    bool active = true;

    void Start()
    {
        DeActivateBullet();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // get params
        LayerMask enemyMask = bulletParams.enemyMask;
        LayerMask bulletBoundaryMask = bulletParams.bulletBoundaryMask;

        // check if collision is in mask
        int layer = collision.gameObject.layer;
        if ((enemyMask & (1 << layer)) != 0)
        {
            return;
        }
        if ((bulletBoundaryMask & (1 << layer)) != 0)
        {
            DeActivateBullet();
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // get params
        LayerMask enemyMask = bulletParams.enemyMask;
        LayerMask bulletBoundaryMask = bulletParams.bulletBoundaryMask;

        // check if collision is in mask
        int layer = collision.gameObject.layer;
        if ((enemyMask & (1 << layer)) != 0)
        {
            return;
        }
        if ((bulletBoundaryMask & (1 << layer)) != 0)
        {
            DeActivateBullet();
            return;
        }
    }

    public static void InitializeBullets(Transform _firingPointA, Transform _firingPointB)
    {
        firingPointA = _firingPointA;
        firingPointA = _firingPointB;
    }

    public void ShootBullet()
    {
        if(active == true) 
        {
            Debug.LogWarning("Bullet is currently active and fired");
            return; 
        }
        ActivateBullet();
    }

    void BullletEnemyHit()
    {

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
            rb2D.MovePosition(firingPointA.position);
            //transform.position = firingPointA.position;
        }
        else
        {
            rb2D.MovePosition(firingPointB.position);
            //transform.position = firingPointB.position;
        }

        // give it velocity
        rb2D.velocity = Vector2.up * speed;
    }

    void DeActivateBullet()
    {
        active = false;
        gameObject.SetActive(false);
    }
}
