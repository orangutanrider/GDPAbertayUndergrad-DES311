using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicBulletManager : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] BasicPlayerBulletParams bulletParams;
    [Space]
    public Transform firingPointA;
    public Transform firingPointB;
    public EnemyHitManager hitManager;
    public GameObject playerRootObject;

    List<BasicPlayerBulletScript> bulletPool = new List<BasicPlayerBulletScript>();

    int activeBullets = 0;
    int bulletPoolCursor = 0;

    public const string heirarchyObjectName = "BasicBullets";
    GameObject heirarchyObject = null;

    readonly Vector3 bulletSpawnPoint = new Vector3(100, 100);

    // Initialize
    private void Start()
    {
        // get params
        int maxConcurrentBullets = bulletParams.maxConcurrentBullets;

        // Spawn heirarchy object
        heirarchyObject = new GameObject(heirarchyObjectName);
        heirarchyObject.transform.position = Vector3.zero;

        // Spawn bullets
        for (int loop = 0; loop <= maxConcurrentBullets; loop++)
        {
            SpawnNewBulletIntoPool();
        }

        // Initialize BasicBullet class
        BasicPlayerBulletScript.InitializeBullets(firingPointA, firingPointB, hitManager, playerRootObject);
    }

    public void ShootBullet()
    {
        BasicPlayerBulletScript bullet = GetPooledBullet();
        if (bullet == null) { return; }

        bullet.ShootBullet();
    }

    BasicPlayerBulletScript GetPooledBullet()
    {
        // get params
        int maxConcurrentBullets = bulletParams.maxConcurrentBullets;

        // if all bullets are active, return
        if (activeBullets >= maxConcurrentBullets) { return null; }

        // both loops combined will go through the entire list of bullets (if required)

        // go from cursor position to end of list
        for (int loop = bulletPoolCursor; loop < maxConcurrentBullets; loop++)
        {
            if (bulletPool[loop].Active == true) { continue; }

            IncrementBulletPoolCursor();
            return bulletPool[loop];
        }

        // go from start of list to cursor position
        for (int loop = 0; loop < bulletPoolCursor; loop++)
        {
            if (bulletPool[loop].Active == true) { continue; }

            IncrementBulletPoolCursor();
            return bulletPool[loop];
        }

        return null;
    }

    void IncrementBulletPoolCursor()
    {
        int maxConcurrentBullets = bulletParams.maxConcurrentBullets;
        bulletPoolCursor++;
        if (bulletPoolCursor > maxConcurrentBullets) { bulletPoolCursor = 0; }
    }

    void SpawnNewBulletIntoPool()
    {
        // get params
        GameObject bulletPrefab = bulletParams.BulletPrefab;

        // Instantiate new bullet
        GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint, Quaternion.Euler(Vector3.zero), heirarchyObject.transform);
        BasicPlayerBulletScript bulletScript = newBullet.GetComponent<BasicPlayerBulletScript>();

        // Add bullet to pool
        bulletPool.Add(bulletScript);

        // (the bullets set themselves to inactive on start)
    }
}
