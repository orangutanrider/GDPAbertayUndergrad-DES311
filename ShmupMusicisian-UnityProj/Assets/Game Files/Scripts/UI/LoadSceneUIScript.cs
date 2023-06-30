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
        SceneManager.LoadScene(loadParams.name, loadParams.loadMode);
    }

    public void LoadSceneByIndex()
    {
        SceneManager.LoadScene(loadParams.sceneIndex, loadParams.loadMode);
    }
}
