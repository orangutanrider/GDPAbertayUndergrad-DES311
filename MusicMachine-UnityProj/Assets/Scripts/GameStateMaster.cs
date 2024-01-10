using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMaster : MonoBehaviour
{
    public static GameStateMaster instance = null;

    public float gameTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Debug.LogWarning("Two instances of this?");
        }

        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer = gameTimer + Time.deltaTime;
    }
}
