using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyHitManager : MonoBehaviour
{
    // store a list of enemies
    // set the names of these enemies, so they include their index in the list, in the name
    // include a function to return the status changer of an enemy via index

    [ReadOnly]
    [SerializeField] int cachedEnemies = 0;

    // (it has to be serialized for it to save the cache, otherwise it gets reset when you go in and out of play mode) (hence: [HideInInspector] [SerializeField])
    [HideInInspector] [SerializeField] List<EnemyStatusChanger> enemyList = null;

    const int maximumListLength = 100;

    #region Initialization
    [ContextMenu("Cache and Initialize Enemies")]
    public void CacheAndInitializeEnemies()
    {
        // find and cache enemy status changers (including ones on inactive gameobjects)
        EnemyStatusChanger[] enemyArray = (EnemyStatusChanger[])FindObjectsOfType(typeof(EnemyStatusChanger), true);
        if (enemyArray.Length == 0)
        {
            Debug.Log("No enemies were found");
            return;
        }

        // update display
        cachedEnemies = enemyArray.Length;

        // construct new list
        enemyList = new List<EnemyStatusChanger>();
        for(int loop = 0; loop < enemyArray.Length; loop++)
        {
            enemyList.Add(enemyArray[loop]);

            // update enemy names
            RenameEnemyStatusChanger(enemyArray[loop], loop);
        }

        Debug.Log("Cached and initialized " + enemyArray.Length + " enemies.");
    }

    [ContextMenu("Try Cache Pre-Initialized Enemies")]
    public void TryCachePreInitializedEnemies()
    {
        // find and cache enemy status changers (including ones on inactive gameobjects)
        EnemyStatusChanger[] enemyArray = (EnemyStatusChanger[])FindObjectsOfType(typeof(EnemyStatusChanger), true);
        if (enemyArray.Length == 0)
        {
            Debug.Log("No enemies were found");
            return;
        }

        // construct a new list of the enemies in order of their pre-initialized indexes in their names
        List<EnemyStatusChanger> newEnemyList = new List<EnemyStatusChanger>();
        List<int> parsedAndAddedIndexes = new List<int>();
        int discardedEnemies = 0;
        for (int loop = 0; loop < enemyArray.Length; loop++)
        {
            // fill list with null entries 
            newEnemyList.Add(null);
        }
        for (int loop = 0; loop < enemyArray.Length; loop++)
        {
            // change those null entries to the real data (if a matching index is found)

            EnemyStatusChanger enemy = enemyArray[loop];

            #region Error Checking
            // is name formatted correctly?
            bool nameFormatted = CheckIfNameIsFormattedCorrectly(out int index, enemy, true); // when debug mode is true, this function handles the error message itself 
            if(nameFormatted == false) 
            {
                Debug.LogWarning("The enemy " + enemy.gameObject.name + " at position " + enemy.transform.position + " will not be cached due to the incorrectly formatted name.");
                discardedEnemies++;
                continue; 
            }

            // does the enemy share an index with any of the other pre-initialized enemies?
            if(parsedAndAddedIndexes.Contains(index) == true)
            {
                Debug.LogWarning("The enemy " + enemy.gameObject.name + " at position " + enemy.transform.position + " shares an index with another of the pre-initialized enemies, this enemy hasn't been cached due to that.");
                EditorGUIUtility.PingObject(enemy.gameObject);
                discardedEnemies++;
                continue;
            }

            // is the enemy's index too large for the list?
            if(index > maximumListLength)
            {
                Debug.LogWarning("The enemy " + enemy.gameObject.name + " at position " + enemy.transform.position + " has an index too large for the list, it has not been cached due to that.");
                EditorGUIUtility.PingObject(enemy.gameObject);
                discardedEnemies++;
                continue;
            }
            #endregion

            // if an index given is greater than the number of list entries, then add null values until the number of list entries reaches the index
            if (index >= newEnemyList.Count)
            {
                AddNullValuesToListUntilCountX(index, ref newEnemyList);
                Debug.Log("List has been extended beyond the actual number of enemies in the scene to match the index of enemy " + enemy.name + " at position " + enemy.transform.position);
                EditorGUIUtility.PingObject(enemy.gameObject);
            }

            // overwrite a null to add the enemy to the list
            newEnemyList[index] = enemy;

            // add enemy to added indexes list so that other entries can check if they share this index
            parsedAndAddedIndexes.Add(index);
        }

        if(parsedAndAddedIndexes.Count == 0)
        {
            Debug.Log("No enemies have been cached due to errors, the enemy list has not been updated.");
            return;
        }

        Debug.Log("A total of " + parsedAndAddedIndexes.Count + " enemies have been cached.");
        Debug.Log(discardedEnemies + " enemies have been discarded due to errors.");
        enemyList = newEnemyList;
    }

    void AddNullValuesToListUntilCountX(int x, ref List<EnemyStatusChanger> list)
    {
        for (int loop = 0; loop < x; loop++)
        {
            if(loop > list.Count)
            {
                list.Add(null);
                continue;
            }
        }
    }

    void RenameEnemyStatusChanger(EnemyStatusChanger enemy, int index)
    {
        enemy.gameObject.name = index.ToString() + "-EnemyStatus";

        foreach(EnemyHittable hittable in enemy.hittables)
        {
            hittable.gameObject.name = index.ToString() + "-EnemyHittable";
        }
    }

    // Debug
    [ContextMenu("Print and Highlight Enemy List")]
    public void PrintAndHighlightEnemyList()
    {
        if(enemyList == null)
        {
            Debug.Log("Enemy list is null");
            return;
        }

        if(enemyList.Count == 0)
        {
            Debug.Log("Enemy list is empty");
            return;
        }

        foreach(EnemyStatusChanger enemy in enemyList)
        {
            Debug.Log(enemy.gameObject.name + " at " + enemy.transform.position);
            EditorGUIUtility.PingObject(enemy);
        }
    }
    #endregion

    private void Awake()
    {
        foreach(EnemyStatusChanger enemy in enemyList)
        {
            enemy.HitManager = this;
        }
    }

    public void ManualEnemyInitializeAndCache(EnemyStatusChanger enemy)
    {
        if (enemyList == null)
        {
            enemyList = new List<EnemyStatusChanger>();
        }

        RenameEnemyStatusChanger(enemy, enemyList.Count);
        enemyList.Add(enemy);
        enemy.HitManager = this;
        cachedEnemies = enemyList.Count;
    }

    public EnemyStatusChanger GetEnemyStatusViaFormattedName(string enemyName)
    {
        if(enemyList == null)
        {
            Debug.LogWarning("EnemyList is null, there are no cached enemies to get");
            return null;
        }

        // read the index portion of the name
        string indexPortionOfName = "";
        for (int loop = 0; loop < enemyName.Length; loop++)
        {
            if (enemyName[loop] == '-')
            {
                // end loop
                loop = enemyName.Length;
                continue;
            }

            indexPortionOfName = indexPortionOfName + enemyName[loop];
        }

        bool parseSuccess = int.TryParse(indexPortionOfName, out int index);
        if(parseSuccess == false) 
        {
            Debug.LogWarning("Parse unsucessful, name is likely formatted incorrectly (it must start with a number, and a '-' must be used to denote the end of the number)");
            return null; 
        }

        // (index out of bounds isn't handled, it can happen but it's not up to this script to deal with it)

        return enemyList[index];
    }

    // returns -1 out index if the name is formatted incorrectly
    public static bool CheckIfNameIsFormattedCorrectly(out int outIndex, EnemyStatusChanger enemy, bool debugMode = false)
    {
        outIndex = -1;
        string enemyName = enemy.gameObject.name;

        // step through the enemy's name and look for a '-'
        string indexPortionOfName = "";
        bool dashFound = false;
        for (int loop = 0; loop < enemyName.Length; loop++)
        {
            if (enemyName[loop] == '-')
            {
                // end loop
                loop = enemyName.Length;
                dashFound = true;
                continue;
            }

            indexPortionOfName = indexPortionOfName + enemyName[loop];
        }

        // does '-' exist?
        if (dashFound == false && debugMode == true)
        {
            Debug.Log(enemy.gameObject.name + " at " + enemy.transform.position + " has an incorrectly formatted name (no '-' character was found).");
            EditorGUIUtility.PingObject(enemy.gameObject);
        }
        if (dashFound == false) 
        { 
            return false; 
        }

        // can an index be parsed from the name?
        bool parseSuccess = int.TryParse(indexPortionOfName, out int index);
        if (parseSuccess == false && debugMode == true)
        {
            Debug.Log(enemy.gameObject.name + " at " + enemy.transform.position + " has an incorrectly formatted name (an index couldn't be parsed from it).");
            EditorGUIUtility.PingObject(enemy.gameObject);
        }
        if (parseSuccess == false)
        {
            return false;
        }

        // is index a large number? Extremely large indexes aren't accepted.
        if (index > maximumListLength && debugMode == true)
        {
            Debug.Log(enemy.gameObject.name + " at " + enemy.transform.position + " has an incorrectly formatted name (their index value was too large, it cannot be over " + maximumListLength +").");
            EditorGUIUtility.PingObject(enemy.gameObject);
        }
        if (index > maximumListLength)
        {
            return false;
        }

        // if the above comments are correct, then it is a correctly formatted name.
        outIndex = index;
        return true;
    }
}
