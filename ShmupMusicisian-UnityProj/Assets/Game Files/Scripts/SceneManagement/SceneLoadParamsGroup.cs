using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneLoadParamsGroup", menuName = "SceneManagement/ParamsGroup")]
public class SceneLoadParamsGroup : ScriptableObject
{
    public List<SceneLoadParams> paramsGroup = new List<SceneLoadParams>();
}
