using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletLinearMovement : EnemyBulletMovement
{
    [Header("Required References")]
    [SerializeField] EnemyBasicBulletParams bulletParams;
    public Rigidbody2D rb2D;

    private void OnDrawGizmos()
    {
        if(bulletParams.debugMode == false) { return; }

        Gizmos.DrawRay(transform.position, bulletParams.movementVector);
    }

    private void OnEnable()
    {
        rb2D.velocity = bulletParams.movementVector * BulletBaseParams.speedMultiply;
        UpdateBaseData(bulletParams.movementVector, rb2D.velocity);
    }
}
