using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyHitManager))]
public class EnemyHitManagerCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemyHitManager hitManager = (EnemyHitManager)target;

        // cache button
        if (GUILayout.Button("CacheAndInitializeEnemies"))
        {
            hitManager.CacheAndInitializeEnemies();
        }

        if (GUILayout.Button("PrintAndHighlightEnemyList"))
        {
            hitManager.PrintAndHighlightEnemyList();
        }
    }
}
