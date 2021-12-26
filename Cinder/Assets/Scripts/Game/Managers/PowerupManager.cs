using UnityEngine;

public class PowerupManager : BaseObject
{
    [SerializeField]
    protected CrazyBall crazyBallRef;

    [SerializeField]
    protected Shield shield;

    [SerializeField]
    protected LaserBatPowerUp laserBat;

    [SerializeField]
    protected FlameBall flameBall;

    [SerializeField]
    protected FreezePlayer freezePlayer;

    [SerializeField]
    protected LaserBulletManager laserBulletManager;

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
        shield.DisablePowerUp();
        crazyBallRef.DisablePowerUp();
        laserBat.DisablePowerUp();
        flameBall.DisablePowerUp();
    }

    protected void OnEnable()
    {
        shield.DisableInstantly();
        crazyBallRef.DisableInstantly();
        laserBat.DisableInstantly();
        flameBall.DisableInstantly();
    }

    public void FireLaser(Vector3 position, Vector3 velocity)
    {
        // fire a laser from position, in direction and speed of velocity
//		Debug.Log("fire a bullet");
// todo change this to using a pool
        //     var _laserBullet = Instantiate(laserBulletPrefabReference, position, Quaternion.identity);
        //    _laserBullet.transform.parent = transform;
        //   _laserBullet.Launch(velocity);

        laserBulletManager.LaunchLaserBullet(position, velocity);
    }

    public override void LifeLost()
    {
        DisableAllPowerUps();
    }

    public void ActivatePowerUp(PowerupType powerUpType)
    {
        // if the game is not in play mode, then don't activate power up - this is to catch some cases during level complete or game over where a power up may be collected at the same time as the event occurs
        if (!CoreConnector.GameManager.IsGamePlaying())
        {
            // Game is not playing, cannot activate a power up
            return;
        }

        // check that level isn't complete as its possible that a power up is collected just as you complete a level		
        switch (powerUpType)
        {
            case PowerupType.MultiBall:
                PlaySound(SoundList.multiBall);
                // add two new balls to the scene, side by side at the starting height
                CoreConnector.GameManager.ballManager.AddMultiBalls(-.3f);
                ShowInGameMessage(Message.MultiBall);
                break;
            case PowerupType.WideBat:
                laserBat.DisableInstantly();
                PlaySound(SoundList.PowerUpWideBat);
                CoreConnector.GameManager.playersBatManager.ChangeToNewBat(PlayerBatTypes.Wide);
                ShowInGameMessage(Message.WideBat);
                break;
            case PowerupType.SmallBat:
                laserBat.DisableInstantly();
                PlaySound(SoundList.PowerUpSmallBat);
                CoreConnector.GameManager.playersBatManager.ChangeToNewBat(PlayerBatTypes.Small);
                ShowInGameMessage(Message.SmallBat);
                break;
            case PowerupType.LaserBat:
                CoreConnector.GameUIManager.DisplayPowerUpBar();
                laserBat.Activate();
                ShowInGameMessage(Message.LaserBat);
                break;
            case PowerupType.SplitBat:
                laserBat.DisableInstantly();
                PlaySound(SoundList.PowerUpSplitBat);
                CoreConnector.GameManager.playersBatManager.ChangeToNewBat(PlayerBatTypes.Split);
                ShowInGameMessage(Message.SplitBat);
                break;
            case PowerupType.Shield:
                CoreConnector.GameUIManager.DisplayPowerUpBar();
                ShowInGameMessage(Message.Shield);
                shield.Activate();
                break;
            case PowerupType.FlameBall:
                flameBall.Activate();
                ShowInGameMessage(Message.Fireball);
                break;
            case PowerupType.CrazyBall:
                crazyBallRef.Activate();
                ShowInGameMessage(Message.CrazyBall);
                break;
            case PowerupType.FreezePlayer:
                ShowInGameMessage(Message.Freeze);
                freezePlayer.Activate();
                break;
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
        crazyBallRef.ManagePowerUpTime();
        shield.ManagePowerUpTime();
        laserBat.ManagePowerUpTime();
        flameBall.ManagePowerUpTime();
        freezePlayer.ManagePowerUpTime();
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

        if (crazyBallRef.IsPowerUpActive())
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