using UnityEngine;

public class LoadSceneStackUIScript : MonoBehaviour
{
    [SerializeField] SceneLoadStack loadStack;

    public void LoadStack()
    {
        if(loadStack == null) 
        {
            Debug.LogWarning("Call to load on LoadSceneStackUIScript attached to gameobject '" + gameObject.name + "' at position " + transform.position + " but the load stack reference was null, so nothing has been loaded.");
            return;
        }

        loadStack.LoadStack();
    }
}
