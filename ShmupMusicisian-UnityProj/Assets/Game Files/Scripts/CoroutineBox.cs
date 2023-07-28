using UnityEngine;

//https://forum.unity.com/threads/how-to-target-a-coroutine-from-inside-itself.892342/

public class CoroutineBox
{
    public CoroutineBox(Coroutine coroutine)
    {
        Coroutine = coroutine;
    }

    public Coroutine Coroutine { get; set; }
}
