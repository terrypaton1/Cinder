using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : BaseObject
{
    [SerializeField]
    protected CrazyBallPowerUp crazyBallPowerUpRef;

    [SerializeField]
    protected Shield shield;

    [SerializeField]
    protected LaserBatPowerUp laserBat;

    [SerializeField]
    protected FlameBall flameBall;

    [SerializeField]
    protected FreezePlayer freezePlayer;

    [SerializeField]
    protected SplitBatPowerUp splitBat;

    [SerializeField]
    protected MultiBallPowerUp multiBallPowerUp;

    [SerializeField]
    protected WideBatPowerUp wideBatPowerUp;

    [SerializeField]
    protected SmallBatPowerUp smallBatPowerUp;

    [Space(10), SerializeField]
    protected LaserBulletManager laserBulletManager;

    private Dictionary<PowerupType, PowerUpBase> allPowerups = new Dictionary<PowerupType, PowerUpBase>();

    public void OneTimeSetup()
    {
        laserBulletManager.OneTimeSetup();
    }

    public override void LevelComplete()
    {
        CoreConnector.GameUIManager.HidePowerUpBar();
        DisableAllPowerUps();
        laserBulletManager.LevelComplete();
    }

    private void DisableAllPowerUps()
    {
        foreach (var powerup in allPowerups)
        {
            powerup.Value.DisablePowerUp();
        }
    }

    protected void OnEnable()
    {
        allPowerups = new Dictionary<PowerupType, PowerUpBase>
        {
            {PowerupType.Shield, shield},
            {PowerupType.CrazyBall, crazyBallPowerUpRef},
            {PowerupType.LaserBat, laserBat},
            {PowerupType.FlameBall, flameBall},
            {PowerupType.FreezePlayer, freezePlayer},
            {PowerupType.SplitBat, splitBat},
            {PowerupType.WideBat, wideBatPowerUp},
            {PowerupType.MultiBall, multiBallPowerUp},
            {PowerupType.SmallBat, smallBatPowerUp}
        };

        foreach (var powerUpKeyPair in allPowerups)
        {
            powerUpKeyPair.Value.DisableInstantly();
        }
    }

    public void FireLaser(Vector3 position, Vector3 velocity)
    {
        laserBulletManager.LaunchLaserBullet(position, velocity);
    }

    public override void LifeLost()
    {
        DisableAllPowerUps();
    }

    public void DisableLaserBat()
    {
        laserBat.DisableInstantly();
    }

    public void ActivatePowerUp(PowerupType powerUpType)
    {
        // if the game is not in play mode, then don't activate power up - this is to catch some cases during level complete or game over where a power up may be collected at the same time as the event occurs
        if (!CoreConnector.GameManager.IsGamePlaying())
        {
            // Game is not playing, cannot activate a power up
            return;
        }

        foreach (var powerUpPair in allPowerups)
        {
            if (powerUpPair.Key == powerUpType)
            {
                powerUpPair.Value.Activate();
                return;
            }
        }
    }

    protected void Update()
    {
        if (!CoreConnector.GameManager.IsGamePlaying())
        {
            return;
        }

        ManagePowerUps();
    }

    private void ManagePowerUps()
    {
        foreach (var powerUpPair in allPowerups)
        {
            powerUpPair.Value.ManagePowerUpTime();
        }
    }

    public void TestDisablePowerUpBar()
    {
        if (laserBat.IsPowerUpActive())
        {
            return;
        }

        if (shield.IsPowerUpActive())
        {
            return;
        }

        if (crazyBallPowerUpRef.IsPowerUpActive())
        {
            return;
        }

        CoreConnector.GameUIManager.HidePowerUpBar();
    }

    public void RestartLevel()
    {
        DisableAllPowerUps();
        laserBulletManager.HideAll();
        CoreConnector.GameUIManager.HidePowerUpBarInstantly();
    }
}