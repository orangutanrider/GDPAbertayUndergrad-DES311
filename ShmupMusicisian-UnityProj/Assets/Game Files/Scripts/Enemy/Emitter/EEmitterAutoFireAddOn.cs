using System.Collections.Generic;
using UnityEngine;

public class EEmitterAutoFireAddOn : MonoBehaviour
{
    public bool firing = true;
    public float emissionInterval = 1;
    public List<EEmitter> emitters = new List<EEmitter>();

    float emissionTimer = 0;

    private void Update()
    {
        emissionTimer = emissionTimer + Time.deltaTime;

        if (firing == false || emissionTimer < emissionInterval) { return; }

        emissionTimer = 0;

        foreach (EEmitter emitter in emitters)
        {
            emitter.Emit(this);
        }
    }
}
