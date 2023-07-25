using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EEmitterGroup)), CanEditMultipleObjects]
public class EEmitterGroupCustomEditor : Editor
{
    float multiEdit_RandomDelay = 0;
    float multiEdit_RandomRotation = 0;

    public override void OnInspectorGUI()
    {
        EEmitterGroup emitterGroup = (EEmitterGroup)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Multi-Edit RandomRotation", EditorStyles.boldLabel);
        multiEdit_RandomRotation = EditorGUILayout.FloatField("Multi-Edit Value", multiEdit_RandomRotation);
        if (GUILayout.Button("Multi-Edit RandomRotation"))
        {
            emitterGroup.MultiEditRandomRotation(multiEdit_RandomRotation);
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Multi-Edit RandomDelay", EditorStyles.boldLabel);
        multiEdit_RandomDelay = EditorGUILayout.FloatField("Multi-Edit Value", multiEdit_RandomDelay);
        if (GUILayout.Button("Multi-Edit RandomDelay"))
        {
            emitterGroup.MultiEditRandomDelay(multiEdit_RandomDelay);
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Get EEmitters from Children", EditorStyles.boldLabel);
        if (GUILayout.Button("Get EEmitters from Children"))
        {
            emitterGroup.GetEEmittersFromChildren();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
