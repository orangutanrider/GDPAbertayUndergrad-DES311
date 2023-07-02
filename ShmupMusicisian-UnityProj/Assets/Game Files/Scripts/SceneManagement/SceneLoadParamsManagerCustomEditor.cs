using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneLoadParamsManager)), CanEditMultipleObjects]
public class SceneLoadParamsManagerCustomEditor : Editor
{
    SerializedProperty targetLoadParams;
    SerializedProperty targetGroup;
    SerializedProperty managedGroups;

    const string setBuildIndexesMessage = "Doing this will go through each group given and set their buildIndex values. " +
        "The system gets their buildIndex values by using the path values to find each scene and its buildIndex, in the build settings." +
        "This will overwrite any previously set buildIndex values";
    const string setNamesAndPathsMessage = "Doing this will go through each group given and set their sceneName and path values." +
        "The system gets their names and paths by using their buildIndex values to find each scene and its path, in the build settings." +
        "This will overwrite any previously set names and paths.";

    private void OnEnable()
    {
        targetLoadParams = serializedObject.FindProperty("targetLoadParams");
        targetGroup = serializedObject.FindProperty("targetGroup");
        managedGroups = serializedObject.FindProperty("managedGroups");
    }

    public override void OnInspectorGUI()
    {
        SceneLoadParamsManager paramsManager = (SceneLoadParamsManager)target;

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // MULTI EDIT
        EditorGUILayout.LabelField("Multi Edit", EditorStyles.boldLabel);
        if (GUILayout.Button("Set Names and Paths by using BuildIndexes"))
        {
            if (EditorUtility.DisplayDialog("Set Names", setNamesAndPathsMessage, "Ok", "Cancel") == true)
            {
                paramsManager.SetNamesAndPathsByUsingBuildIndexes();
            }
        }
        if (GUILayout.Button("Set BuildIndexes by using Paths"))
        {
            if (EditorUtility.DisplayDialog("Set BuildIndexes", setBuildIndexesMessage, "Ok", "Cancel") == true)
            {
                paramsManager.SetBuildIndexesByUsingPath();
            }
        }
        EditorGUILayout.PropertyField(managedGroups);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // TARGET EDIT GROUP
        EditorGUILayout.LabelField("Target Edit Group", EditorStyles.boldLabel);
        if (GUILayout.Button("Set Target's Names and Paths by using BuildIndexes"))
        {
            paramsManager.TargetGroupSetNameAndPath();
        }
        if (GUILayout.Button("Set Target's BuildIndexes by using Paths"))
        {
            paramsManager.TargetGroupSetBuildIndex();
        }
        EditorGUILayout.PropertyField(targetGroup);

        EditorGUILayout.Space();

        // TARGET EDIT PARAMS
        EditorGUILayout.LabelField("Target Edit Params", EditorStyles.boldLabel);
        if (GUILayout.Button("Set Target's Name and Path by using its BuildIndex"))
        {
            paramsManager.TargetLoadParamsSetNameAndPath();
        }
        if (GUILayout.Button("Set Target's BuildIndex by using its Path"))
        {
            paramsManager.TargetLoadParamsSetBuildIndex();
        }
        EditorGUILayout.PropertyField(targetLoadParams);

        serializedObject.ApplyModifiedProperties();
    }
}
