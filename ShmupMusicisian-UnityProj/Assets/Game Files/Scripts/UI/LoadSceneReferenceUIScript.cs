using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneReferenceUIScript : MonoBehaviour
{
    [SerializeField] SceneReferenceParams sceneReference;
    public LoadMode loadMode = LoadMode.AdditiveLoad;

    public void LoadReference()
    {
        if (sceneReference == null)
        {
            Debug.LogWarning("Call to load on LoadSceneReferenceUIScript attached to gameobject '" + gameObject.name + "' at position " + transform.position + " but the scene reference was null, so nothing has been loaded.");
            return;
        }

        int buildIndex = sceneReference.buildIndex;
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
