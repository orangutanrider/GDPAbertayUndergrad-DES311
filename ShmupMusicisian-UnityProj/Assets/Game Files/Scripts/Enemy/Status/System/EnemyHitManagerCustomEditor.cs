using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(EnemyHitManager))]
public class EnemyHitManagerCustomEditor : Editor
{
    SerializedProperty enemyList;

    private void OnEnable()
    {
        enemyList = serializedObject.FindProperty("enemyList");
    }

    public override void OnInspectorGUI()
    {
        EnemyHitManager hitManager = (EnemyHitManager)target;

        if (GUILayout.Button("Find and Cache Enemies"))
        {
            hitManager.FindAndCacheEnemies();
        }

        if (GUILayout.Button("Print and Highlight Enemy List"))
        {
            hitManager.PrintAndHighlightEnemyList();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Use the 'Find and Cache Enemies' button to fill this list automatically, don't manually add enemies one by one.");
        EditorGUILayout.PropertyField(enemyList);

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(hitManager);
            EditorSceneManager.MarkSceneDirty(hitManager.gameObject.scene);
        }
    }
}
