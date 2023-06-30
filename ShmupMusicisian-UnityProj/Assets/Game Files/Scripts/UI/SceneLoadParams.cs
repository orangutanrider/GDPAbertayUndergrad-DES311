using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneLoadParams", menuName = "Misc/SceneLoadParams")]
public class SceneLoadParams : ScriptableObject
{
    public int sceneIndex = 0;
    public string sceneName = "";
    public LoadSceneMode loadMode = LoadSceneMode.Single;
}
