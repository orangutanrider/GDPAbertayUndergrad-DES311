using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class EnemyHitMaster : MonoBehaviour
{
    [SerializeField] SceneReferenceParams levelGadgetsScene;
    public List<EnemyHitManager> managers = new List<EnemyHitManager>();

    static EnemyHitMaster currentMaster = null;

    const string runTimeManagerName = "Runtime EnemyHitManager";
    static int runTimeHitManagerIndex = -1;

    #region Tools
    [ContextMenu("Find and Cache HitManagers")]
    public void FindAndCacheHitManagers()
    {
        // find and cache enemy status changers in the home scene (including ones on inactive gameobjects)

        // clear list 
        managers = new List<EnemyHitManager>();

        // get all EnemyStatusChanger in current scene
        GameObject[] gameObjectsInHomeScene = gameObject.scene.GetRootGameObjects();
        foreach (GameObject gameObjectInHomeScene in gameObjectsInHomeScene)
        {
            EnemyHitManager[] managerArray = gameObjectInHomeScene.transform.GetComponentsInChildren<EnemyHitManager>(true);
            if (managerArray == null) { continue; }
            if (managerArray.Length == 0) { continue; }

            // add successful finds into the list
            foreach (EnemyHitManager enemy in managerArray)
            {
                managers.Add(enemy);
            }
        }

        Debug.Log("Cached " + managers.Count + " managers.");
    }

    // Debug
    [ContextMenu("Print and Highlight HitManagers")]
    public void PrintAndHighlightHitManagers()
    {
        if (managers == null)
        {
            Debug.Log("HitManager list is null");
            return;
        }

        if (managers.Count == 0)
        {
            Debug.Log("HitManager list is empty");
            return;
        }

        foreach (EnemyHitManager manager in managers)
        {
            Debug.Log(manager.gameObject.name + " at " + manager.transform.position);
            EditorGUIUtility.PingObject(manager);
        }
    }
    #endregion

    void Start()
    {
        // if this is there isn't a current master, become the current master
        if (currentMaster == null)
        {
            Scene gadgetsScene = SceneManager.GetSceneByBuildIndex(levelGadgetsScene.buildIndex);
            SceneManager.MoveGameObjectToScene(gameObject, gadgetsScene);

            currentMaster = this;

            CreateRuntimeManager();

            // initialize managers
            for (int loop = 0; loop < managers.Count; loop++)
            {
                managers[loop].RenameEnemyStatusList(loop);
                managers[loop].HitMaster = this;
                managers[loop].ManagerIndex = loop;
            }

            return;
        }

        // if there's already an active master
        // add this master's managers to it, and self destruct
        for (int loop = 0; loop < managers.Count; loop++)
        {
            int managerIndex = currentMaster.RegisterManager(managers[loop]);
            managers[loop].RenameEnemyStatusList(managerIndex);
            managers[loop].HitMaster = currentMaster;
            managers[loop].ManagerIndex = managerIndex;
        }

        Destroy(this);
    }

    int RegisterManager(EnemyHitManager hitManager) // adds a manager to the list and returns the index that the new manager gets put in
    {
        // step through manager list and look for null values, if find a null, then use that list slot instead of adding a new one
        for (int loop = 0; loop < managers.Count; loop++)
        {
            if (managers[loop] == null)
            {
                managers[loop] = hitManager;
                return loop;
            }
        }

        managers.Add(hitManager);
        return managers.Count - 1;
    }

    public EnemyStatusChanger GetEnemyStatusViaFormattedName(string enemyName)
    {
        #region Get Index Portions of the Name
        int endPosition = 0;
        string firstIndexString = "";
        bool firstIndexEnemyOrManager = false; // if false first index was enemy index, if true first index was manager index
        for (int loop = 0; loop < enemyName.Length; loop++)
        {
            if(enemyName[loop] == EnemyHitManager.enemyNotationChar)
            {
                firstIndexEnemyOrManager = false;
                endPosition = loop + 1;
                loop = enemyName.Length;
                continue;
            }
            if (enemyName[loop] == EnemyHitManager.managerNotationChar)
            {
                firstIndexEnemyOrManager = true;
                endPosition = loop + 1;
                loop = enemyName.Length;
                continue;
            }

            firstIndexString = firstIndexString + enemyName[loop];
        }

        string secondIndexString = "";
        for (int loop = endPosition; loop < enemyName.Length; loop++)
        {
            if (enemyName[loop] == EnemyHitManager.enemyNotationChar || enemyName[loop] == EnemyHitManager.managerNotationChar)
            {
                endPosition = loop + 1;
                loop = enemyName.Length;
                continue;
            }

            secondIndexString = secondIndexString + enemyName[loop];
        }
        #endregion

        #region Parse Index Strings
        int enemyIndex = -1;
        int managerIndex = -1;
        if(firstIndexEnemyOrManager == false) // if first index was the enemy's index
        {
            // Parse enemy index from firstIndexString
            bool enemyParse = int.TryParse(firstIndexString, out int outEnemyIndex);
            if (enemyParse == false)
            {
                Debug.LogWarning("Index parse was unsuccesfful, the string given has probably been formatted incorrectly. Use the function RenameEnemyStatus() in the EnemyHitManager class to format the name correctly.");
                return null;
            }
            enemyIndex = outEnemyIndex;

            // Parse manager index from secondIndexString
            bool managerParse = int.TryParse(secondIndexString, out int outManagerIndex);
            if (managerParse == false)
            {
                Debug.LogWarning("Index parse was unsuccesfful, the string given has probably been formatted incorrectly. Use the function RenameEnemyStatus() in the EnemyHitManager class to format the names correctly.");
                return null;
            }
            managerIndex = outManagerIndex;
        }
        else // if first index was the manager's index
        {
            // Parse manager index from firstIndexString
            bool managerParse = int.TryParse(firstIndexString, out int outManagerIndex);
            if (managerParse == false)
            {
                Debug.LogWarning("Index parse was unsuccesfful, the string given has probably been formatted incorrectly. Use the function RenameEnemyStatus() in the EnemyHitManager class to format the name correctly.");
                return null;
            }
            managerIndex = outManagerIndex;

            // Parse enemy index from secondIndexString
            bool enemyParse = int.TryParse(secondIndexString, out int outEnemyIndex);
            if (enemyParse == false)
            {
                Debug.LogWarning("Index parse was unsuccesfful, the string given has probably been formatted incorrectly. Use the function RenameEnemyStatus() in the EnemyHitManager class to format the names correctly.");
                return null;
            }
            enemyIndex = outEnemyIndex;
        }
        #endregion

        return managers[managerIndex].enemyList[enemyIndex];
    }

    void CreateRuntimeManager()
    {
        // Create Runtime Manager
        GameObject runtimeManagerGameObject = new GameObject(runTimeManagerName);
        EnemyHitManager runTimeHitManager = runtimeManagerGameObject.AddComponent<EnemyHitManager>();

        // Initialize manager
        runTimeHitManagerIndex = RegisterManager(runTimeHitManager);
        runTimeHitManager.HitMaster = this;
        runTimeHitManager.RenameEnemyStatusList(runTimeHitManagerIndex);
        runTimeHitManager.ManagerIndex = runTimeHitManagerIndex;

        // Move it to gadgets scene, so it doesn't get un-loaded
        Scene gadgetsScene = SceneManager.GetSceneByBuildIndex(levelGadgetsScene.buildIndex);
        SceneManager.MoveGameObjectToScene(runtimeManagerGameObject, gadgetsScene);
    }

    public void RuntimeAddStatusEnemyToManager(EnemyStatusChanger enemyStatus)
    {
        if(managers[runTimeHitManagerIndex] == null)
        {
            Debug.Log("There was no runtime HitManager, creating a new one. This isn't supposed to happen though. They're supposed to get created onStart and then preserved in the levelGadgetsScene.");
            CreateRuntimeManager();
            return;
        }
        managers[runTimeHitManagerIndex].AddEnemyStatusToManager(enemyStatus);
    }
}
