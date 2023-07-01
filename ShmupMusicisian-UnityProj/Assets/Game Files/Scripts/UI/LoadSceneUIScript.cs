using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneUIScript : MonoBehaviour
{
    [Header("Required References")]
    public SceneLoadParams loadParams;

    public void LoadSceneByName()
    {
        SceneManager.LoadScene(loadParams.sceneName, loadParams.loadMode);
    }

    public void LoadSceneByIndex()
    {
        SceneManager.LoadScene(loadParams.buildIndex, loadParams.loadMode);
    }

    public void UnLoadSceneByName()
    {
        SceneManager.UnloadSceneAsync(loadParams.sceneName);
    }

    public void UnLoadSceneByIndex()
    {
        SceneManager.UnloadSceneAsync(loadParams.buildIndex);
    }
}
