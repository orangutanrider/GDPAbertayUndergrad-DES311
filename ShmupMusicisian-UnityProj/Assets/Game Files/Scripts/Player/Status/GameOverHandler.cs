using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] SceneLoadParams gameOverScene;
    public GameObject playerRoot; // temp for destroying the player
    public GameObject playerInputObject;

    public void TriggerGameOver()
    {
        SceneManager.LoadScene(gameOverScene.buildIndex, LoadSceneMode.Additive);
        playerInputObject.SetActive(false);
        Destroy(playerRoot);
    }
}
