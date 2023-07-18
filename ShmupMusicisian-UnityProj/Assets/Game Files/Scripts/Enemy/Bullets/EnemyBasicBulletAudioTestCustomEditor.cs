using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyBasicBulletAudioTestScript)), CanEditMultipleObjects]
public class EnemyBasicBulletAudioTestCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyBasicBulletAudioTestScript testScript = (EnemyBasicBulletAudioTestScript)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.wordWrap = true;

        EditorGUILayout.LabelField("If for some reason it doesn't work (i.e. no audio is played when the button is clicked) then try entering and exiting playmode.", labelStyle);
        if (GUILayout.Button("ExecuteTest"))
        {
            testScript.ExecuteTest();
        }
    }
}
