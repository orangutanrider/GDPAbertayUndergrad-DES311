using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneLoadStack)), CanEditMultipleObjects]
public class SceneLoadStackCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SceneLoadStack sceneLoadStack = (SceneLoadStack)target;

        if(sceneLoadStack.loadStack == null)
        {
            DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();
            return;
        }

        // gives the list entries names
        for (int loop = 0; loop < sceneLoadStack.loadStack.Count; loop++)
        {
            if(sceneLoadStack.loadStack[loop] == null)
            {
                continue;
            }
            if (sceneLoadStack.loadStack[loop].SceneReference == null)
            {
                sceneLoadStack.loadStack[loop].name = "NULL";
                continue;
            }

            sceneLoadStack.loadStack[loop].name = sceneLoadStack.loadStack[loop].SceneReference.name +" (" + sceneLoadStack.loadStack[loop].loadMode.ToString() + ")";
        }

        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}
