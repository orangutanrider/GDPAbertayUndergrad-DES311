using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneLoadStack", menuName = "SceneManagement/LoadStack")]
public class SceneLoadStack : ScriptableObject
{
    public List<SceneLoad> loadStack = new List<SceneLoad>();

    [System.Serializable]
    public class SceneLoad
    {
        [HideInInspector] public string name = "";
        [SerializeField] SceneReferenceParams sceneReference;
        public LoadMode loadMode = LoadMode.AdditiveLoad;

        public SceneReferenceParams SceneReference
        {
            get
            {
                return sceneReference;
            }
        }
    }

    public void LoadStack()
    {
        Debug.Log("Call to load SceneLoadStack: '" + name + "'");

        if(loadStack == null)
        {
            Debug.LogWarning("Call to load, but loadStack was null on SceneLoadStack '" + name);
            return;
        }
        if (loadStack.Count == 0)
        {
            Debug.LogWarning("Call to load, but loadStack was empty on SceneLoadStack '" + name);
            return;
        }

        for (int loop = 0; loop < loadStack.Count; loop++)
        {
            int buildIndex = loadStack[loop].SceneReference.buildIndex;
            LoadMode loadMode = loadStack[loop].loadMode;

            Debug.Log("Scene reference '" + loadStack[loop].SceneReference.name + "' with path '" + loadStack[loop].SceneReference.scenePath + " and index " + loadStack[loop].SceneReference.buildIndex + System.Environment.NewLine +
                "is being loaded in LoadMode: " + loadMode);

            switch (loadMode)
            {
                case LoadMode.SingleLoad:
                    SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
                    break;
                case LoadMode.AdditiveLoad:
                    SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
                    break;
                case LoadMode.AsyncSingleLoad:
                    SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
                    break;
                case LoadMode.AsyncAdditiveLoad:
                    SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
                    break;
                case LoadMode.AsyncUnload:
                    SceneManager.UnloadSceneAsync(buildIndex);
                    break;
                case LoadMode.AsyncUnloadAllEmbedded:
                    SceneManager.UnloadSceneAsync(buildIndex, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                    break;
            }
        }
    }

    private void OnValidate()
    {
        
    }
}
