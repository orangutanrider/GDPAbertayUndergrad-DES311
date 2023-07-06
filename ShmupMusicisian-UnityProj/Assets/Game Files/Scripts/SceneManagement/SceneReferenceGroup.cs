using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneReferenceGroup", menuName = "SceneManagement/ReferenceGroup")]
public class SceneReferenceGroup : ScriptableObject
{
    public List<SceneReferenceParams> paramsGroup = new List<SceneReferenceParams>();
}
