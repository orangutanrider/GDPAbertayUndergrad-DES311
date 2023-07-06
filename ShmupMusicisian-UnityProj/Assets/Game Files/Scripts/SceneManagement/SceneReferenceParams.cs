using UnityEngine;

[CreateAssetMenu(fileName = "SceneReference", menuName = "SceneManagement/SceneReference")]
public class SceneReferenceParams : ScriptableObject
{
    public string sceneName = "";
    public string scenePath = "";
    public int buildIndex = 0;
}
