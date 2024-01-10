using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseTestScript : MonoBehaviour
{
    public AK.Wwise.Event testEvent = new AK.Wwise.Event();

    // Start is called before the first frame update
    void Start()
    {
        testEvent.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J) == true)
        {
            testEvent.Post(gameObject);
        }
    }
}
