using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicBulletManager : MonoBehaviour
{
    [Header("Required References")]
    public BasicPlayerBulletParams bulletParams;

    List<BasicPlayerBulletScript> bulletPool = new List<BasicPlayerBulletScript>();

    int activeBullets = 0;
    int bulletPoolCursor = 0;

    public void ShootBullet()
    {
        BasicPlayerBulletScript bullet = GetPooledBullet();
        if (bullet == null) { return; }

        bullet.ShootBullet();
    }

    public void EndBullet(BasicPlayerBulletScript bullet)
    {

    }

    BasicPlayerBulletScript GetPooledBullet()
    {
        // get params
        int maxConcurrentBullets = bulletParams.maxConcurrentBullets;

        // if all bullets are active, return
        if (activeBullets >= maxConcurrentBullets) { return null; }

        // both loops combined will go through the entire list of bullets (if required)

        // go from cursor position to end of list
        for (int loop = bulletPoolCursor; loop < maxConcurrentBullets; loop++)
        {
            if (bulletPool[loop].Active == true) { continue; }

            IncrementBulletPoolCursor();
            return bulletPool[loop];
        }

        // go from start of list to cursor position
        for (int loop = 0; loop < bulletPoolCursor; loop++)
        {
            if (bulletPool[loop].Active == true) { continue; }

            IncrementBulletPoolCursor();
            return bulletPool[loop];
        }

        return null;
    }

    void IncrementBulletPoolCursor()
    {
        int maxConcurrentBullets = bulletParams.maxConcurrentBullets;
        bulletPoolCursor++;
        if (bulletPoolCursor > maxConcurrentBullets) { bulletPoolCursor = 0; }
    }
}
