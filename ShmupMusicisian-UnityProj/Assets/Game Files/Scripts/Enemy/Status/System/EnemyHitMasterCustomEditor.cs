using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(EnemyHitMaster))]
public class EnemyHitMasterCustomEditor : Editor
{
    SerializedProperty managers;
    SerializedProperty levelGadgetsScene;

    private void OnEnable()
    {
        managers = serializedObject.FindProperty("managers");
        levelGadgetsScene = serializedObject.FindProperty("levelGadgetsScene");
    }

    public override void OnInspectorGUI()
    {
        EnemyHitMaster hitManager = (EnemyHitMaster)target;

        EditorGUILayout.LabelField("Required References", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(levelGadgetsScene);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("EnemyHitManagers", EditorStyles.boldLabel);

        if (GUILayout.Button("Find and Cache HitManagers"))
        {
            hitManager.FindAndCacheHitManagers();
        }

        if (GUILayout.Button("Print and Highlight HitManagers"))
        {
            hitManager.PrintAndHighlightHitManagers();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Use the 'Find and Cache HitManagers' button to fill this list automatically, don't manually add managers one by one.");
        EditorGUILayout.PropertyField(managers);

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(hitManager);
            EditorSceneManager.MarkSceneDirty(hitManager.gameObject.scene);
        }
    }
}
