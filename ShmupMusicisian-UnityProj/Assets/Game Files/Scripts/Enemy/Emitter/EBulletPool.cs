using System.Collections.Generic;
using UnityEngine;

public class EBulletPool : MonoBehaviour
{
    // This class is semi-compromised, it is not capable of pooling different prefabs of bullets
    // Adding that feature was considered but I'm not adding due to time and mostly the fact that I probably wouldn't use it anyways

    [SerializeField] EBulletPoolParams poolParams;

    List<EBullet> bulletPool = new List<EBullet>();
    int activeBullets = 0;
    int poolCursor = 0;

    public GameObject HeirarchyObject { get; private set; }

    void Start()
    {
        for (int loop = 0; loop <= poolParams.maxConcurrentBullets; loop++)
        {
            SpawnNewBulletIntoPool();
        }
        CreateHeirarchyObject(poolParams.heirarchyName);
    }

    public void SpawnNewBulletIntoPool()
    {
        GameObject bulletPrefab = poolParams.bulletPrefab;
        Vector3 poolSpawnPoint = poolParams.poolSpawnPoint;

        GameObject newBullet = Instantiate(bulletPrefab, poolSpawnPoint, Quaternion.Euler(Vector3.zero), HeirarchyObject.transform);
        EBullet eBullet = newBullet.GetComponent<EBullet>();

        if (eBullet == null)
        {
            Destroy(newBullet);

            Debug.LogWarning
                (
                "Error occured on EBulletPool attached to gameobject '" + gameObject.name + "' at position " + transform.position + System.Environment.NewLine +
                "Couldn't get an EBullet component from the instantiated object, have automatically destroyed the instantiated object."
                );

            return;
        }

        bulletPool.Add(eBullet);
        newBullet.SetActive(false);
    }

    public EBullet GetPooledBullet()
    {
        // get params
        int maxConcurrentBullets = poolParams.maxConcurrentBullets;
        bool printFails = poolParams.printFails;

        // if all bullets are active, return
        if (activeBullets >= maxConcurrentBullets && printFails == true)
        {
            Debug.Log("The emitter on gameobject '" + gameObject.name + "' reached its maxConcurrentBullets, cannot emit");
            return null;
        }
        if (activeBullets >= maxConcurrentBullets)
        {
            return null;
        }

        // both loops combined will go through the entire list of bullets (if required)

        // go from cursor position to end of list
        for (int loop = poolCursor; loop < maxConcurrentBullets; loop++)
        {
            if (bulletPool[loop].gameObject.activeInHierarchy == true) { continue; }

            IncrementPoolCursor();
            return bulletPool[loop];
        }

        // go from start of list to cursor position
        for (int loop = 0; loop < poolCursor; loop++)
        {
            if (bulletPool[loop].gameObject.activeInHierarchy == true) { continue; }

            IncrementPoolCursor();
            return bulletPool[loop];
        }

        if (printFails == true)
        {
            Debug.LogWarning("The pool on gameobject '" + gameObject.name + "' encountered a miscellaneous error when trying to give a bullet");
            return null;
        }

        return null;
    }

    private void CreateHeirarchyObject(string heirarchyObjectName = "EnemyBullets (unnamed)")
    {
        if (heirarchyObjectName == "EnemyBullets (unnamed)")
        {
            Debug.Log("Unnamed heirarchy object created for enemy bullet emitter attached to gameobject '" + gameObject.name + "' at position " + transform.position + System.Environment.NewLine +
                "You can name the heirarchy object by overriding the Start() method and calling the function CreateHeirarchyObject()");
        }

        HeirarchyObject = new GameObject(heirarchyObjectName);
        HeirarchyObject.transform.position = Vector3.zero;
    }

    private void IncrementPoolCursor()
    {
        poolCursor++;
        if (poolCursor > poolParams.maxConcurrentBullets) { poolCursor = 0; }
    }
}
