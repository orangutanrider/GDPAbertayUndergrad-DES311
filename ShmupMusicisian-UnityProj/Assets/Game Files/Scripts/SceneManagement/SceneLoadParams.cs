using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneLoadParams", menuName = "SceneManagement/LoadParams")]
public class SceneLoadParams : ScriptableObject
{
    public string sceneName = "";
    public string scenePath = "";
    public int buildIndex = 0;

    [Header("Additional Data")]
    public LoadSceneMode loadMode = LoadSceneMode.Single;
}
