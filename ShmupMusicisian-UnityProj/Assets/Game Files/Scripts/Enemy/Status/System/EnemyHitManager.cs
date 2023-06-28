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

    List<EnemyStatusChanger> enemyList = null;

    #region Initialization
    [ContextMenu("Cache and Initialize Enemies")]
    public void CacheAndInitializeEnemies()
    {
        // find and cache enemy status changers (including ones on inactive gameobjects)
        EnemyStatusChanger[] enemyArray = (EnemyStatusChanger[])FindObjectsOfType(typeof(EnemyStatusChanger), true);

        // update display
        cachedEnemies = enemyArray.Length;

        // construct new list
        enemyList = new List<EnemyStatusChanger>();
        for(int loop = 0; loop < enemyArray.Length; loop++)
        {
            enemyList.Add(enemyArray[loop]);

            // update enemy names
            RenameEnemy(enemyArray[loop], loop); 
        }

        Debug.Log("Cached and initialized " + enemyArray.Length + " enemies.");
    }

    public void ManualAddEnemyToList(EnemyStatusChanger enemy)
    {
        if(enemyList == null)
        {
            enemyList = new List<EnemyStatusChanger>();
        }

        RenameEnemy(enemy, enemyList.Count);
        enemyList.Add(enemy);

        cachedEnemies = enemyList.Count;
    }

    void RenameEnemy(EnemyStatusChanger enemy, int index)
    {
        enemy.gameObject.name = index.ToString() + "-EnemyStatus";
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
            indexPortionOfName = indexPortionOfName + enemyName[loop];
            if (enemyName[loop] == '-')
            {
                // end loop
                loop = enemyName.Length;
                continue;
            }
        }

        bool parseSuccess = int.TryParse(indexPortionOfName, out int index);
        if(parseSuccess == false) 
        {
            Debug.LogWarning("Parse unsucessful, name is likely formatted incorrectly (it must start with a number, and a '-' must be used to denote the end of the number)");
            return null; 
        }

        return enemyList[index];
    }
}
