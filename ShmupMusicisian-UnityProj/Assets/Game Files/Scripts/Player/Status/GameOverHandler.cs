using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] SceneReferenceParams gameOverScene;
    public GameObject playerRoot; // temp for destroying the player
    public GameObject playerInputObject;

    public void TriggerGameOver()
    {
        int buildIndex = gameOverScene.buildIndex;
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);

        playerInputObject.SetActive(false);
        Destroy(playerRoot);
    }
}
