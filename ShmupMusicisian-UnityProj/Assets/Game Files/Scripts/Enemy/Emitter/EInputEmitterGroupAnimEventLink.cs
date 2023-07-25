using System.Collections.Generic;
using UnityEngine;

public class EInputEmitterGroupAnimEventLink : MonoBehaviour
{
    public string eventLinkName = "";
    [SerializeField] List<EInputEmitterGroup> emitterGroups = new List<EInputEmitterGroup>();

    public void EmitEInputEmitterGroups()
    {
        foreach(EInputEmitterGroup emitterGroup in emitterGroups)
        {
            emitterGroup.EmitAll();
        }
    }
}
