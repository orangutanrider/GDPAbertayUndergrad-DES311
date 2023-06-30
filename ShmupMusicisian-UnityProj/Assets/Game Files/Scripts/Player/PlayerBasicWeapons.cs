using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBasicWeapons : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] BasicPlayerBulletParams weaponParams;
    public PlayerBasicBulletManager bulletManager;

    bool firing = false;
    float shotTimer = 0f;

    private void Start()
    {
        shotTimer = weaponParams.shotInterval;
    }

    // Execution
    private void FixedUpdate()
    {
        FiringUpdate();
    }

    private void OnDisable()
    {
        ResetFlags();
    }

    // Input event
    public void FireBasicWeaponsInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                firing = true;
                break;
            case InputActionPhase.Canceled:
                firing = false;
                break;
        }
    }

    // Fixed Update function
    void FiringUpdate()
    {
        if (firing == false)
        {
            shotTimer = weaponParams.shotInterval;
            return;
        }

        shotTimer = shotTimer + Time.fixedDeltaTime;

        if (shotTimer < weaponParams.shotInterval) { return; }

        // Shoot Bullet
        bulletManager.ShootBullet();
        shotTimer = 0f;
    }

    public void ResetFlags()
    {
        firing = false;
        shotTimer = weaponParams.shotInterval;
    }
}
