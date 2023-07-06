using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneReferenceManager", menuName = "SceneManagement/Manager")]
public class SceneReferenceManager : ScriptableObject
{
    [SerializeField] SceneReferenceParams targetSingle;
    [SerializeField] SceneReferenceGroup targetGroup;
    [SerializeField] List<SceneReferenceGroup> managedGroups = new List<SceneReferenceGroup>();

    enum GroupResults
    {
        All,
        Some,
        None
    }

    #region TargetEdit
    public void TargetLoadParamsSetBuildIndex()
    {
        if (targetSingle == null)
        {
            Debug.LogWarning("Target is null");
            return;
        }
        TrySetBuildIndexViaPath(targetSingle);
    }
    public void TargetLoadParamsSetNameAndPath()
    {
        if (targetSingle == null)
        {
            Debug.LogWarning("Target is null");
            return;
        }
        TrySetNameAndPathViaIndex(targetSingle);
    }

    public void TargetGroupSetBuildIndex()
    {
        if (targetGroup == null)
        {
            Debug.LogWarning("Target is null");
            return;
        }
        SetBuildIndexesForGroup(targetGroup);
    }

    public void TargetGroupSetNameAndPath()
    {
        if (targetGroup == null)
        {
            Debug.LogWarning("Target is null");
            return;
        }
        SetNamesAndPathsForGroup(targetGroup);
    }
    #endregion

    #region SetBuildIndex
    public void SetBuildIndexesByUsingPath()
    {
        int numFullSuccess = 0;
        int numPartialSuccess = 0;
        int numFailed = 0;

        foreach (SceneReferenceGroup group in managedGroups)
        {
            GroupResults results = SetBuildIndexesForGroup(group);

            switch (results)
            {
                case GroupResults.All:
                    numFullSuccess++;
                    break;
                case GroupResults.Some:
                    numPartialSuccess++;
                    break;
                case GroupResults.None:
                    numFailed++;
                    break;
            }
        }

        if (numFailed == 0 && numPartialSuccess == 0)
        {
            Debug.Log("Succesffuly set all (" + numFullSuccess + ") groups with no errors.");
            return;
        }
        if (numFullSuccess == 0 && numPartialSuccess == 0)
        {
            Debug.LogWarning("Failed to set all (" + numFailed + ") groups.");
            return;
        }

        if (numFullSuccess > 0)
        {
            Debug.Log(numFullSuccess + " groups have been set with full success.");
        }
        if (numPartialSuccess > 0)
        {
            Debug.LogWarning(numFullSuccess + " groups have been set with partial success.");
        }
        if (numFailed > 0)
        {
            Debug.LogWarning(numFullSuccess + " groups have failed to set anything.");
        }
    }

    GroupResults SetBuildIndexesForGroup(SceneReferenceGroup group)
    {
        int numSucceeded = 0;
        int numFailed = 0;
        foreach (SceneReferenceParams sceneParams in group.paramsGroup)
        {
            bool successful = TrySetBuildIndexViaPath(sceneParams);
            if (successful == true)
            {
                numSucceeded++;
            }
            else
            {
                numFailed++;
            }
        }

        if (numFailed == 0)
        {
            Debug.Log("Set the buildIndexes for all (" + numSucceeded + ") sceneLoadParam objects in sceneGroup '" + group.name + "'");
            return GroupResults.All;
        }

        if (numSucceeded == 0)
        {
            Debug.LogWarning("Failed to set the buildIndexes for all (" + numSucceeded + ") sceneLoadParam objects in sceneGroup '" + group.name + "'");
            return GroupResults.None;
        }

        Debug.LogWarning("Set the buildIndexes for " + numSucceeded + " sceneLoadParam objects in sceneGroup '" + group.name + "'");
        Debug.LogWarning("Failed to set the buildIndexes for " + numFailed + " sceneLoadParam objects in sceneGroup '" + group.name + "'");
        return GroupResults.Some;
    }

    bool TrySetBuildIndexViaPath(SceneReferenceParams sceneParams)
    {
        int buildIndex = SceneUtility.GetBuildIndexByScenePath(sceneParams.scenePath);

        if (buildIndex == -1)
        {
            Debug.LogWarning("Couldn't find the buildIndex from path '" + sceneParams.scenePath + "'" + System.Environment.NewLine +
                "Its scriptable object is '" + sceneParams.name + "'");
            return false;
        }

        Debug.Log("Succesfully set sceneParams Object '" + sceneParams.name + "' buildIndex to " + buildIndex);
        sceneParams.buildIndex = buildIndex;
        return true;
    }
    #endregion

    #region SetNameAndPath
    public void SetNamesAndPathsByUsingBuildIndexes()
    {
        int numFullSuccess = 0;
        int numPartialSuccess = 0;
        int numFailed = 0;

        foreach (SceneReferenceGroup group in managedGroups)
        {
            GroupResults results = SetNamesAndPathsForGroup(group);
            switch (results)
            {
                case GroupResults.All:
                    numFullSuccess++;
                    break;
                case GroupResults.Some:
                    numPartialSuccess++;
                    break;
                case GroupResults.None:
                    numFailed++;
                    break;
            }
        }

        if (numFailed == 0 && numPartialSuccess == 0)
        {
            Debug.Log("Succesffuly set all (" + numFullSuccess + ") groups with no errors.");
            return;
        }
        if (numFullSuccess == 0 && numPartialSuccess == 0)
        {
            Debug.LogWarning("Failed to set all (" + numFailed + ") groups.");
            return;
        }

        if (numFullSuccess > 0)
        {
            Debug.Log(numFullSuccess + " groups have been set with full success.");
        }
        if (numPartialSuccess > 0)
        {
            Debug.LogWarning(numPartialSuccess + " groups have been set with partial success.");
        }
        if (numFailed > 0)
        {
            Debug.LogWarning(numFailed + " groups have failed to set anything.");
        }
    }

    GroupResults SetNamesAndPathsForGroup(SceneReferenceGroup group)
    {
        int numSucceeded = 0;
        int numFailed = 0;
        foreach (SceneReferenceParams sceneParams in group.paramsGroup)
        {
            bool successful = TrySetNameAndPathViaIndex(sceneParams);
            if (successful == true)
            {
                numSucceeded++;
            }
            else
            {
                numFailed++;
            }
        }

        if (numFailed == 0)
        {
            Debug.Log("Set the names and paths for all (" + numSucceeded + ") sceneLoadParam objects in sceneGroup '" + group.name + "'");
            return GroupResults.All;
        }

        if (numSucceeded == 0)
        {
            Debug.LogWarning("Failed to set the names for all (" + numSucceeded + ") sceneLoadParam objects in sceneGroup '" + group.name + "'");
            return GroupResults.None;
        }

        Debug.LogWarning("Set the names for " + numSucceeded + " sceneLoadParam objects in sceneGroup '" + group.name + "'");
        Debug.LogWarning("Failed to set the names for " + numFailed + " sceneLoadParam objects in sceneGroup '" + group.name + "'");
        return GroupResults.Some;
    }

    bool TrySetNameAndPathViaIndex(SceneReferenceParams sceneParams)
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(sceneParams.buildIndex);

        if (string.IsNullOrWhiteSpace(scenePath) == true)
        {
            Debug.LogWarning("Couldn't find the name for scene with buildIndex" + sceneParams.buildIndex + ", Its scriptable object is '" + sceneParams.name + "'");
            return false;
        }

        // set scene path

        // process and set name
        string sceneName = "";
        for (int loop = scenePath.Length - 1; loop >= 0; loop--)
        {
            if (scenePath[loop] == '/')
            {
                // end loop
                loop = -1;
                continue;
            }

            // add character to name
            sceneName = sceneName + scenePath[loop];
        }
        // (flip string)
        char[] charArray = sceneName.ToCharArray();
        System.Array.Reverse(charArray);
        sceneName = new string(charArray);

        // set data
        Debug.Log
            (
            "Succesfully set sceneParams Object '" + sceneParams + "'" + System.Environment.NewLine +
            "Name to '" + sceneName + "'" + System.Environment.NewLine +
            "And path to '" + scenePath + "'"
            );
        sceneParams.sceneName = sceneName;
        sceneParams.scenePath = scenePath;
        return true;
    }
    #endregion
}
