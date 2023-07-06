using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class EnemyHitManager : MonoBehaviour
{
    // store a list of enemies
    // set the names of these enemies, so they include their index in the list, in the name
    // include a function to return the status changer of an enemy via index

    public List<EnemyStatusChanger> enemyList = new List<EnemyStatusChanger>();

    public const char managerNotationChar = 'H';
    public const char enemyNotationChar = '-';

    public EnemyHitMaster HitMaster { get; set; }
    public int ManagerIndex { get; set; } // if this is -1, it means it's a runtime manager

    private void Awake()
    {
        // give enemies a reference to this hitManager
        foreach (EnemyStatusChanger enemy in enemyList)
        {
            enemy.HitManager = this;
        }
    }

    #region Tools
    [ContextMenu("FindAndCacheEnemies")]
    public void FindAndCacheEnemies()
    {
        // find and cache enemy status changers in the home scene (including ones on inactive gameobjects)

        // clear list 
        enemyList = new List<EnemyStatusChanger>();

        // get all EnemyStatusChanger in current scene
        GameObject[] gameObjectsInHomeScene = gameObject.scene.GetRootGameObjects();
        foreach (GameObject gameObjectInHomeScene in gameObjectsInHomeScene)
        {
            EnemyStatusChanger[] enemyArray = gameObjectInHomeScene.transform.GetComponentsInChildren<EnemyStatusChanger>(true);
            if (enemyArray == null) { continue; }
            if (enemyArray.Length == 0) { continue; }

            // add successful finds into the list
            foreach (EnemyStatusChanger enemy in enemyArray)
            {
                enemyList.Add(enemy);
            }
        }

        Debug.Log("Cached " + enemyList.Count + " enemies.");
    }

    // Debug
    [ContextMenu("Print and Highlight EnemyList")]
    public void PrintAndHighlightEnemyList()
    {
        if (enemyList == null)
        {
            Debug.Log("Enemy list is null");
            return;
        }

        if (enemyList.Count == 0)
        {
            Debug.Log("Enemy list is empty");
            return;
        }

        foreach (EnemyStatusChanger enemy in enemyList)
        {
            Debug.Log(enemy.gameObject.name + " at " + enemy.transform.position);
            EditorGUIUtility.PingObject(enemy);
        }
    }
    #endregion

    public void RenameEnemyStatusList(int managerIndex)
    {
        for(int loop = 0; loop < enemyList.Count; loop++)
        {
            RenameEnemyStatus(enemyList[loop], loop, managerIndex);
        }
    }

    static void RenameEnemyStatus(EnemyStatusChanger enemy, int enemyIndex, int managerIndex)
    {
        string statusChangerName = managerIndex.ToString() + managerNotationChar + enemyIndex.ToString() + enemyNotationChar + "EnemyStatus";
        string hittablesName = managerIndex.ToString() + managerNotationChar + enemyIndex.ToString() + enemyNotationChar + "EnemyHittable";

        enemy.gameObject.name = statusChangerName;

        foreach (EnemyHittable hittable in enemy.hittables)
        {
            hittable.gameObject.name = hittablesName;
        }
    }

    public int AddEnemyStatusToManager(EnemyStatusChanger enemyStatus)
    {
        // step through enemy list and look for null values, if find a null, then use that list slot instead of adding a new one
        for (int loop = 0; loop < enemyList.Count; loop++)
        {
            if (enemyList[loop] == null)
            {
                enemyList[loop] = enemyStatus;
                RenameEnemyStatus(enemyStatus, loop, ManagerIndex);
                return loop;
            }
        }

        enemyList.Add(enemyStatus);
        RenameEnemyStatus(enemyStatus, enemyList.Count - 1, ManagerIndex);
        return enemyList.Count - 1;
    }
}

