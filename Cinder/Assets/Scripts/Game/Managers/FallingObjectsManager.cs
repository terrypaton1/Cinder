using System.Collections.Generic;
using UnityEngine;

public class FallingObjectsManager : MonoBehaviour
{
    [SerializeField]
    private FallingPoints FallingPointsPrefab;

    [SerializeField]
    private FallingPowerup FallingPowerUpPrefab;

    private const int maxPooled = 20;
    private List<FallingPoints> fallingPointsPool;
    private List<FallingPowerup> fallingPowerUpPool;

    public void HideAll()
    {
        foreach (var fallingPoint in fallingPointsPool)
        {
            fallingPoint.Disable();
        }

        foreach (var fallingPowerup in fallingPowerUpPool)
        {
            fallingPowerup.Disable();
        }
    }

    public void BuildFallingPointsPool()
    {
        fallingPointsPool = new List<FallingPoints>();
        for (var i = 0; i < maxPooled; ++i)
        {
            var instance = Instantiate(FallingPointsPrefab, transform, true);
            fallingPointsPool.Add(instance);
            instance.Disable();
        }
    }

    public void AddFallingPoints(Vector3 position, int value, int category)
    {
        var falling = GetFallingPointsFromPool();
        falling.Setup(value, category);
        falling.StartFalling(position);
    }

    private FallingPoints GetFallingPointsFromPool()
    {
        foreach (var fallingPoint in fallingPointsPool)
        {
            if (!fallingPoint.isFalling)
            {
                return fallingPoint;
            }
        }

        Debug.LogError("Out of falling points");
        return null;
    }

    public void BuildFallingPowerUpsPool()
    {
        fallingPowerUpPool = new List<FallingPowerup>();
        for (var i = 0; i < maxPooled; ++i)
        {
            var instance = Instantiate(FallingPowerUpPrefab, transform, true);
            fallingPowerUpPool.Add(instance);
            instance.Disable();
        }
    }

    public void AddFallingPowerUp(Vector3 position, PowerupType newPowerupType)
    {
        var falling = GetPowerUpFromPool();
        falling.Setup(newPowerupType);
        falling.StartFalling(position);
    }

    private FallingPowerup GetPowerUpFromPool()
    {
        foreach (var fallingPowerUp in fallingPowerUpPool)
        {
            if (!fallingPowerUp.isFalling)
            {
                return fallingPowerUp;
            }
        }

        Debug.LogError("Out of falling powerups");
        return null;
    }

    public void LevelComplete()
    {
        foreach (var fallingPowerUp in fallingPowerUpPool)
        {
            if (fallingPowerUp.isFalling)
            {
                fallingPowerUp.LevelComplete();
            }
        }
    }
}