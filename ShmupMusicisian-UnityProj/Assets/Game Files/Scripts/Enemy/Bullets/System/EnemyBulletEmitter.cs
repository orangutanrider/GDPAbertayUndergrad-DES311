using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBulletEmitter : MonoBehaviour
{
    public bool activeAndFiring = false;

    [Header("(Base) Required References")]
    [SerializeField] EnemyBulletEmitterBaseParams emitterBaseParams;

    #region Variables
    public static readonly Vector3 bulletSpawnPoint = new Vector3(-100, -100);

    public List<GameObject> BulletPool
    {
        get
        {
            return bulletPool;
        }
        set
        {
            bulletPool = value;
        }
    }
    List<GameObject> bulletPool = new List<GameObject>();

    public int ActiveBullets { get; set; }
    public int BulletPoolCursor { get; set; }
    public GameObject HeirarchyObject { get; set; } // the heirarchy object is used to group all bullets under the same heirarchy (to keep things neat)

    public virtual float EmissionTimer
    {
        get
        {
            return emissionTimer;
        }
        set
        {
            emissionTimer = value;
        }
    }
    float emissionTimer = 0;
    public const float fallbackEmissionRate = 0.5f;
    #endregion

    public virtual void Start()
    {
        CreateHeirarchyObject();

        // Spawn bullets
        for (int loop = 0; loop <= emitterBaseParams.maxConcurrentBullets; loop++)
        {
            SpawnNewBulletIntoPool(new GameObject("Empty EnemyBullet"));
        }
    }

    public virtual void Update()
    {
        EmissionTimerUpdate();

        if (activeAndFiring == false || EmissionTimer < fallbackEmissionRate) { return; }

        Debug.Log("Firing at the, default, fallback emission rate of " + fallbackEmissionRate + ", you can edit the emission rate by overriding the Update() function, and by overriding the EmissionTimerUpdate() function too.");
        Emit();
        EmissionTimer = 0;
    }

    public virtual void Emit()
    {
        EmissionTimer = 0;

        GameObject bulletBeingEmitted = GetPooledBullet();
        if (bulletBeingEmitted == null) { return; }
        bulletBeingEmitted.SetActive(true);
    }

    public virtual float EmissionTimerUpdate()
    {
        return EmissionTimer = EmissionTimer + (Time.deltaTime * emitterBaseParams.emissionTimerMultiply);
    }

    #region Object Pooling Stuff
    public void CreateHeirarchyObject(string heirarchyObjectName = "EnemyBullets (unnamed)")
    {
        if (heirarchyObjectName == "EnemyBullets (unnamed)")
        {
            Debug.Log("Unnamed heirarchy object created for enemy bullet emitter attached to gameobject '" + gameObject.name + "' at position " + transform.position + System.Environment.NewLine +
                "You can name the heirarchy object by overriding the Start() method and calling the function CreateHeirarchyObject()");
        }

        HeirarchyObject = new GameObject(heirarchyObjectName);
        HeirarchyObject.transform.position = Vector3.zero;
    }

    public GameObject GetPooledBullet()
    {
        // get params
        int maxConcurrentBullets = emitterBaseParams.maxConcurrentBullets;

        // if all bullets are active, return
        if (ActiveBullets >= maxConcurrentBullets) { return null; }

        // both loops combined will go through the entire list of bullets (if required)

        // go from cursor position to end of list
        for (int loop = BulletPoolCursor; loop < maxConcurrentBullets; loop++)
        {
            if (bulletPool[loop].activeInHierarchy == true) { continue; }

            IncrementBulletPoolCursor();
            return bulletPool[loop];
        }

        // go from start of list to cursor position
        for (int loop = 0; loop < BulletPoolCursor; loop++)
        {
            if (bulletPool[loop].activeInHierarchy == true) { continue; }

            IncrementBulletPoolCursor();
            return bulletPool[loop];
        }

        return null;
    }

    public void IncrementBulletPoolCursor()
    {
        int maxConcurrentBullets = emitterBaseParams.maxConcurrentBullets;
        BulletPoolCursor++;
        if (BulletPoolCursor > maxConcurrentBullets) { BulletPoolCursor = 0; }
    }

    public void SpawnNewBulletIntoPool(GameObject bulletPrefab)
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint, Quaternion.Euler(Vector3.zero), HeirarchyObject.transform);
        bulletPool.Add(newBullet);
        newBullet.SetActive(false);
    }
    #endregion
}
