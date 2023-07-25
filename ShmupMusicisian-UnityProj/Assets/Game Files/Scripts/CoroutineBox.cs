using UnityEngine;

//https://forum.unity.com/threads/how-to-target-a-coroutine-from-inside-itself.892342/

// I ended up not using this, but I'm keeping it here cause i find it interesting

public class CoroutineBox
{
    public CoroutineBox(Coroutine coroutine)
    {
        Coroutine = coroutine;
    }

    public Coroutine Coroutine { get; }
}
