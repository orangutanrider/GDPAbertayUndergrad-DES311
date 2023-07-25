using System.Collections.Generic;
using UnityEngine;

public class EEmitterGroupAnimEventLink : MonoBehaviour
{
    public string eventLinkName = "";
    [SerializeField] List<EEmitterGroup> emitterGroups = new List<EEmitterGroup>();

    public void EmitEEmitterGroups()
    {
        foreach (EEmitterGroup emitterGroup in emitterGroups)
        {
            emitterGroup.EmitAll();
        }
    }
}
