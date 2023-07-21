using System.Collections.Generic;
using UnityEngine;

public class EBullet : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] EBulletGlobalParams eBulletGlobalParams;

    [Header("Bullet Components (nullable)")]
    public Mover mover;
    public EBulletMovement movement;
    public EBulletHitDamage hitDamage; // This is compromised, see definition for further details
    public EBulletSynth synth;

    [Header("Stacks")]
    public List<EBulletComponent> onActivateStack = new List<EBulletComponent>();
    public List<EBulletComponent> onDeactivateStack = new List<EBulletComponent>();
    public List<EBulletComponent> OnDeactivationStack = new List<EBulletComponent>();

    public EBulletPool Source { get; set; }

    public bool Deactivating { get { return deactivating; } }
    private bool deactivating = false;

    public int StayActiveRaisedFlags { get { return stayActiveRaisedFlags; } }
    private int stayActiveRaisedFlags = 0;

    bool firstDisable = true;

    #region Public
    // Handle onActivateStack
    // This isn't on onEnable because certain Emitters want to input custom data and activate bullet components manually
    public void ActivateAll()
    {
        gameObject.SetActive(true);

        for (int loop = 0; loop < onActivateStack.Count; loop++)
        {
            EBulletComponent component = onActivateStack[loop];
            IEBulletOnActivate componentInterface = component.GetOnActivate();

            if (componentInterface == null)
            {
                Debug.Log("BulletComponent attached to gameobject '" + component.gameObject.name + "' at position " + component.transform.position +
                    " Returned a null IEnemyBulletActivatable interface, if this is intentional, then you don't need to add it to the componentActivationStack.");
                continue;
            }
            componentInterface.OnActivate();
        }
    }

    public void RequestDeactivation()
    {
        if (deactivating == true) { return; }

        // if not already deactivating, then set deactivating true and handle onDeactivationStack
        deactivating = true;
        for (int loop = 0; loop < OnDeactivationStack.Count; loop++)
        {
            EBulletComponent component = OnDeactivationStack[loop];
            IEBulletOnDeactivation componentInterface = component.GetOnDeactivation();

            if (componentInterface == null)
            {
                Debug.Log("BulletComponent attached to gameobject '" + component.gameObject.name + "' at position " + component.transform.position +
                    " Returned a null IEnemyBulletOnDeactivating interface, if this is intentional, then you don't need to add it to the onDeactivatingStack.");
                continue;
            }
            componentInterface.OnDeactivation();
        }
    }

    public void RaiseStayActiveFlag()
    {
        stayActiveRaisedFlags++;
    }

    public void LowerStayActiveFlag()
    {
        // lower flag
        stayActiveRaisedFlags--;

        // check if num of flags is negative

        if (stayActiveRaisedFlags >= 0) { return; }

        bool allowNegativeFlags =  eBulletGlobalParams.allowNegativeFlags;
        bool printNegativeFlagWarnings = eBulletGlobalParams.printNegativeFlagWarnings;

        if (allowNegativeFlags == false)
        {
            stayActiveRaisedFlags = 0;
        }

        if (printNegativeFlagWarnings == true && allowNegativeFlags == false)
        {
            Debug.LogWarning("StayActiveFlags went negative on gameobject '" + gameObject.name + "' at position " + gameObject.transform.position + System.Environment.NewLine +
                "This indicates that more flags are being lowered than raised which shouldn't happen. The error has been dealt with automatically by setting raised flags to 0.");
            return;
        }
        if (printNegativeFlagWarnings == true && allowNegativeFlags == true)
        {
            Debug.LogWarning("StayActiveFlags went negative on gameobject '" + gameObject.name + "' at position " + gameObject.transform.position + System.Environment.NewLine +
                "This indicates that more flags are being lowered than raised which shouldn't happen. The error hasn't been dealt with automatically as allowNegativeFlags is set to true.");
            return;
        }
    }
    #endregion

    private void Update()
    {
        if (deactivating == false || stayActiveRaisedFlags > 0) { return; }

        deactivating = false;
        gameObject.SetActive(false);
    }

    // Handle onDisableStack
    private void OnDisable()
    {
        if (firstDisable == true)
        {
            firstDisable = false;
            return;
        }

        for (int loop = 0; loop < onDeactivateStack.Count; loop++)
        {
            EBulletComponent component = onDeactivateStack[loop];
            IEBulletOnDeactivate componentInterface = component.GetOnDeactivate();

            if (componentInterface == null)
            {
                Debug.Log("BulletComponent attached to gameobject '" + component.gameObject.name + "' at position " + component.transform.position +
                    " Returned a null IEnemyBulletOnDeactivating interface, if this is intentional, then you don't need to add it to the onDeactivatingStack.");
                continue;
            }
            componentInterface.OnDeactivate();
        }
    }
}
