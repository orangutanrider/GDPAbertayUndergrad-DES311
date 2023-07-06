using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneReferenceManager)), CanEditMultipleObjects]
public class SceneReferenceManagerCustomEditor : Editor
{
    SerializedProperty targetSingle;
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
        targetSingle = serializedObject.FindProperty("targetSingle");
        targetGroup = serializedObject.FindProperty("targetGroup");
        managedGroups = serializedObject.FindProperty("managedGroups");
    }

    public override void OnInspectorGUI()
    {
        SceneReferenceManager refManager = (SceneReferenceManager)target;

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // MULTI EDIT
        EditorGUILayout.LabelField("Multi Edit", EditorStyles.boldLabel);
        if (GUILayout.Button("Set Names and Paths by using BuildIndexes"))
        {
            if (EditorUtility.DisplayDialog("Set Names", setNamesAndPathsMessage, "Ok", "Cancel") == true)
            {
                refManager.SetNamesAndPathsByUsingBuildIndexes();
            }
        }
        if (GUILayout.Button("Set BuildIndexes by using Paths"))
        {
            if (EditorUtility.DisplayDialog("Set BuildIndexes", setBuildIndexesMessage, "Ok", "Cancel") == true)
            {
                refManager.SetBuildIndexesByUsingPath();
            }
        }
        EditorGUILayout.PropertyField(managedGroups);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // TARGET EDIT GROUP
        EditorGUILayout.LabelField("Target Edit Group", EditorStyles.boldLabel);
        if (GUILayout.Button("Set Target's Names and Paths by using BuildIndexes"))
        {
            refManager.TargetGroupSetNameAndPath();
        }
        if (GUILayout.Button("Set Target's BuildIndexes by using Paths"))
        {
            refManager.TargetGroupSetBuildIndex();
        }
        EditorGUILayout.PropertyField(targetGroup);

        EditorGUILayout.Space();

        // TARGET EDIT SINGLE
        EditorGUILayout.LabelField("Target Edit Params", EditorStyles.boldLabel);
        if (GUILayout.Button("Set Target's Name and Path by using its BuildIndex"))
        {
            refManager.TargetLoadParamsSetNameAndPath();
        }
        if (GUILayout.Button("Set Target's BuildIndex by using its Path"))
        {
            refManager.TargetLoadParamsSetBuildIndex();
        }
        EditorGUILayout.PropertyField(targetSingle);

        serializedObject.ApplyModifiedProperties();
    }
}
