using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletActivator : MonoBehaviour
{
    // The purpose of this script is to handle the execution order of OnEnable() on bullet components

    public List<EnemyBulletComponent> activationStack = new List<EnemyBulletComponent>();

    private void OnEnable()
    {
        for (int loop = 0; loop < activationStack.Count; loop++)
        {
            EnemyBulletComponent component = activationStack[loop];
            IEnemyBulletActivatable componentInterface = component.GetActivationInterface();

            if(componentInterface == null)
            {
                Debug.Log("BulletComponent attached to gameobject '" + component.gameObject.name + "' at position " + component.transform.position +
                    " Returned a null IEnemyBulletActivatable interface, if this is intentional, then you don't need to add it to the componentActivationStack.");
                continue;
            }
            componentInterface.Activate();
        }
    }
}
