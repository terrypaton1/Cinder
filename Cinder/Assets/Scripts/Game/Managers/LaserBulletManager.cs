using System.Collections.Generic;
using UnityEngine;

public class LaserBulletManager : MonoBehaviour
{
    [SerializeField]
    protected LaserBullet FallingPointsPrefab;

    private const int maxPooledLasers = 20;

    private List<LaserBullet> laserBulletPool = new List<LaserBullet>();

    public void HideAll()
    {
        foreach (var fallingPoint in laserBulletPool)
        {
            fallingPoint.Disable();
        }
    }

    public void OneTimeSetup()
    {
        while (laserBulletPool.Count < maxPooledLasers)
        {
            var laserBullet = Instantiate(FallingPointsPrefab, transform, true);
            laserBullet.transform.parent = transform;
            laserBulletPool.Add(laserBullet);
            laserBullet.Disable();
        }
    }

    public void LaunchLaserBullet(Vector3 position, Vector3 velocity)
    {
        var laserBullet = GetLaserBulletFromPool();
        laserBullet.transform.position = position;

        laserBullet.Launch(velocity);
    }

    private LaserBullet GetLaserBulletFromPool()
    {
        foreach (var laserBullet in laserBulletPool)
        {
            if (!laserBullet.isUsed)
            {
                return laserBullet;
            }
        }

        Debug.LogError("Out of laserBullet's");
        return null;
    }

    public void LevelComplete()
    {
        HideAll();
    }
}