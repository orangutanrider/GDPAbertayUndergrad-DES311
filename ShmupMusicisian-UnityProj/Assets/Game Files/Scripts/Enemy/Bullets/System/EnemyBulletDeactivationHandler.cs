using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletDeactivationHandler : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] EnemyBulletDeactivationHandlerParams handlerParams;
    public GameObject bulletRoot; // as in the root of the prefab instance, not the actual root in the scene

    [Header("Stacks")]
    public List<EnemyBulletComponent> onDeactivationStack = new List<EnemyBulletComponent>();
    public List<EnemyBulletComponent> onDeactivatingStack = new List<EnemyBulletComponent>();

    public int StayActiveRaisedFlags
    {
        get { return stayActiveRaisedFlags; }
    }
    int stayActiveRaisedFlags = 0;

    public bool Deactivating
    {
        get { return deactivating; }
    }
    bool deactivating = false;

    private void Update()
    {
        if (deactivating == false) { return; }

        if (stayActiveRaisedFlags > 0) { return; }

        Deactivate();
    }

    void Deactivate()
    {
        // Handle onDeactivationStack
        for (int loop = 0; loop < onDeactivationStack.Count; loop++)
        {
            EnemyBulletComponent component = onDeactivationStack[loop];
            IEnemyBulletOnDeactivate componentInterface = component.GetOnDeactivationInterface();

            if (componentInterface == null)
            {
                Debug.Log("BulletComponent attached to gameobject '" + component.gameObject.name + "' at position " + component.transform.position +
                    " Returned a null IEnemyBulletOnDeactivating interface, if this is intentional, then you don't need to add it to the onDeactivatingStack.");
                continue;
            }
            componentInterface.OnDeactivate();
        }

        deactivating = false;

        // Deactivate
        bulletRoot.SetActive(false);
    }

    public void RequestDeactivation()
    {
        if(deactivating == true) { return; }

        deactivating = true;

        // Handle onDeactivatingStack
        for (int loop = 0; loop < onDeactivatingStack.Count; loop++)
        {
            EnemyBulletComponent component = onDeactivatingStack[loop];
            IEnemyBulletOnDeactivating componentInterface = component.GetOnDeactivatingInterface();

            if (componentInterface == null)
            {
                Debug.Log("BulletComponent attached to gameobject '" + component.gameObject.name + "' at position " + component.transform.position +
                    " Returned a null IEnemyBulletOnDeactivating interface, if this is intentional, then you don't need to add it to the onDeactivatingStack.");
                continue;
            }
            componentInterface.OnDeactivating();
        }
    }

    public void RaiseStayActiveFlag()
    {
        stayActiveRaisedFlags++;
    }

    public void LowerStayActiveFlag()
    {
        stayActiveRaisedFlags--;

        if (stayActiveRaisedFlags >= 0) { return; }

        if (handlerParams.allowNegativeFlags == false)
        {
            stayActiveRaisedFlags = 0;
        }

        if (handlerParams.printNegativeFlagWarnings == true && handlerParams.allowNegativeFlags == false) 
        {
            Debug.LogWarning("StayActiveFlags went negative on gameobject '" + gameObject.name + "' at position " + gameObject.transform.position + System.Environment.NewLine +
                "This indicates that more flags are being lowered than raised which shouldn't happen. The error has been dealt with automatically by setting raised flags to 0.");
            return;
        }
        if (handlerParams.printNegativeFlagWarnings == true && handlerParams.allowNegativeFlags == true)
        {
            Debug.LogWarning("StayActiveFlags went negative on gameobject '" + gameObject.name + "' at position " + gameObject.transform.position + System.Environment.NewLine +
                "This indicates that more flags are being lowered than raised which shouldn't happen. The error hasn't been dealt with automatically as allowNegativeFlags is set to true.");
            return;
        }
    }
}
