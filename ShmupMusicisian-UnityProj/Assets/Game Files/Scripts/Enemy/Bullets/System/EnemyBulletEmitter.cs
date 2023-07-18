using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBulletEmitter : MonoBehaviour
{
    public bool autoEmit = false;

    [Header("(Base) Required References")]
    [SerializeField] EnemyBulletEmitterBaseParams emitterBaseParams;

    #region Variables
    public EnemyBulletEmitterBaseParams EmitterBaseParams
    {
        get { return emitterBaseParams; }
    }

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

    public float EmissionTimer { get; set; }

    public static readonly Vector3 bulletSpawnPoint = new Vector3(-100, -100);

    public int ActiveBullets { get; set; }
    public int BulletPoolCursor { get; set; }
    public GameObject HeirarchyObject { get; set; } // the heirarchy object is used to group all bullets under the same heirarchy (to keep things neat)
    #endregion

    // Important Functions:
    // GetPooledBullet() 
    // SpawnNewBulletIntoPool(GameObject bulletPrefab)
    // Emit() - shoots the bullet(s)

    // Important Variables:
    // EmitterBaseParams (look to class to see what it contains)
    // activeAndFiring (the on/off switch)

    private void Start()
    {
        CreateHeirarchyObject(HeirarchyObjectName());
        SpawnBulletPool();
    }

    private void Update()
    {
        EmissionTimer = EmissionTimer + (Time.deltaTime * emitterBaseParams.emissionTimerMultiply);

        if (autoEmit == false || EmissionTimer < EmissionRate()) { return; }

        Emit();
        EmissionTimer = 0;
    }

    public abstract string HeirarchyObjectName();

    protected abstract float EmissionRate();

    protected abstract void SpawnBulletPool();
    // Example mono bullet pool (mono as in they're all the same bullet type)
    /*
        for (int loop = 0; loop <= emitterBaseParams.maxConcurrentBullets; loop++)
        {
            SpawnNewBulletIntoPool(bulletPrefab);
        }
    */

    public abstract void Emit();
    // Example of basic emission (it simply emits a single bullet when called)
    /*
        GameObject bulletBeingEmitted = GetPooledBullet();

        if (bulletBeingEmitted == null && emitterBaseParams.printEmissionFails == true) 
        {
            Debug.Log("The emitter on gameobject '" + gameObject.name + "' couldn't get a bullet from its pool");
            return; 
        }
        if (bulletBeingEmitted == null)
        {
            return;
        }

        bulletBeingEmitted.SetActive(true);
        bulletBeingEmitted.transform.position = transform.position;
    */

    #region Object Pooling Stuff
    protected void CreateHeirarchyObject(string heirarchyObjectName = "EnemyBullets (unnamed)")
    {
        if (heirarchyObjectName == "EnemyBullets (unnamed)")
        {
            Debug.Log("Unnamed heirarchy object created for enemy bullet emitter attached to gameobject '" + gameObject.name + "' at position " + transform.position + System.Environment.NewLine +
                "You can name the heirarchy object by overriding the Start() method and calling the function CreateHeirarchyObject()");
        }

        HeirarchyObject = new GameObject(heirarchyObjectName);
        HeirarchyObject.transform.position = Vector3.zero;
    }

    protected GameObject GetPooledBullet()
    {
        // get params
        int maxConcurrentBullets = emitterBaseParams.maxConcurrentBullets;

        // if all bullets are active, return
        if (ActiveBullets >= maxConcurrentBullets && emitterBaseParams.printEmissionFails == true) 
        {
            Debug.Log("The emitter on gameobject '" + gameObject.name + "' reached its maxConcurrentBullets, cannot emit");
            return null; 
        }
        if (ActiveBullets >= maxConcurrentBullets)
        {
            return null;
        }

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

        if(emitterBaseParams.printEmissionFails == true)
        {
            Debug.LogWarning("The emitter on gameobject '" + gameObject.name + "' encountered an miscellaneous emission error");
            return null;
        }

        return null;
    }

    protected void IncrementBulletPoolCursor()
    {
        int maxConcurrentBullets = emitterBaseParams.maxConcurrentBullets;
        BulletPoolCursor++;
        if (BulletPoolCursor > maxConcurrentBullets) { BulletPoolCursor = 0; }
    }

    protected void SpawnNewBulletIntoPool(GameObject bulletPrefab)
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint, Quaternion.Euler(Vector3.zero), HeirarchyObject.transform);

        bulletPool.Add(newBullet);
        newBullet.SetActive(false);
    }
    #endregion
}
