using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(EnemyHitManager))]
public class EnemyHitManagerCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyHitManager hitManager = (EnemyHitManager)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Cache and Initialize Enemies"))
        {
            hitManager.CacheAndInitializeEnemies();
        }

        if (GUILayout.Button("Print and Highlight Enemy List"))
        {
            hitManager.PrintAndHighlightEnemyList();
        }

        if (GUILayout.Button("Try Cache Pre-Initialized Enemies"))
        {
            hitManager.TryCachePreInitializedEnemies();
        }

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(hitManager);
            EditorSceneManager.MarkSceneDirty(hitManager.gameObject.scene);
        }
    }
}
