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
    protected LaserBullet laserBulletPrefabReference;

    public override void LevelComplete()
    {
        CoreConnector.GameUIManager.HidePowerUpBar();
        DisableAllPowerups();
    }

    private void DisableAllPowerups()
    {
        shield.DisablePowerUp();
        crazyBallRef.DisablePowerUp();
        laserBat.DisablePowerUp();
        flameBall.DisablePowerUp();
    }

    protected void OnEnable()
    {
        DisableAllPowerups();
    }

    public void FireLaser(Vector3 position, Vector3 velocity)
    {
        // fire a laser from position, in direction and speed of velocity
//		Debug.Log("fire a bullet");
// todo change this to using a pool
        var _laserBullet = Instantiate(laserBulletPrefabReference, position, Quaternion.identity);
        _laserBullet.transform.parent = transform;
        _laserBullet.Launch(velocity);
    }

    public override void LifeLost()
    {
        shield.DisableInstantly();
        DisableAllPowerups();
    }

    public void ActivatePowerup(PowerupType _powerupType)
    {
        // if the game is not in play mode, then don't activate power up - this is to catch some cases during level complete or game over where a power up may be collcted at the same time as the event occurs
        if (!CoreConnector.GameManager.IsGamePlaying())
        {
            // Game is not playing, cannot activate a powerup
            return;
        }

        // check that level isn't complete as its possible that a power up is collected just as you complete a level		
//		Debug.Log("ActivatePowerup:" + _powerupType.ToString());
        switch (_powerupType)
        {
            case PowerupType.MultiBall:
                PlaySound(SoundList.multiBall);
                // add two new balls to the scene, side by side at the starting height
                CoreConnector.GameManager.ballManager.AddMultiballs(-.3f);
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
                PlaySound(SoundList.PowerupSmallBat);
                CoreConnector.GameManager.playersBatManager.ChangeToNewBat(PlayerBatTypes.Small);
                ShowInGameMessage(Message.SmallBat);
                break;
            case PowerupType.LaserBat:
                laserBat.Activate();
                ShowInGameMessage(Message.LaserBat);
                break;
            case PowerupType.SplitBat:
                PlaySound(SoundList.PowerupSplitBat);
                CoreConnector.GameManager.playersBatManager.ChangeToNewBat(PlayerBatTypes.Split);
                ShowInGameMessage("Split!");
                break;
            case PowerupType.Shield:
                ShowInGameMessage("Shield!");
                shield.Activate();
                break;
            case PowerupType.FlameBall:
                flameBall.Activate();
                ShowInGameMessage("Fireball!");
                break;
            case PowerupType.CrazyBall:
                crazyBallRef.Activate();
                ShowInGameMessage("Crazy Ball!");
                break;
            case PowerupType.FreezePlayer:

                ShowInGameMessage(Message.Freeze);
                freezePlayer.Activate();
                break;
        }

        CoreConnector.GameUIManager.DisplayPowerUpBar();
    }

    protected void Update()
    {
        if (!CoreConnector.GameManager.IsGamePlaying())
        {
            return;
        }

        ManagePowerups();
    }

    private void ManagePowerups()
    {
        var deltaTime = Time.deltaTime;

        crazyBallRef.ManagePowerUpLoop(deltaTime);
        shield.ManagePowerUpLoop(deltaTime);
        laserBat.ManagePowerUpLoop(deltaTime);
        flameBall.ManagePowerUpLoop(deltaTime);
        freezePlayer.ManagePowerUpLoop(deltaTime);

        TestDisablePowerupBar();
    }

    private void TestDisablePowerupBar()
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
        DisableAllPowerups();
        CoreConnector.GameUIManager.HidePowerUpBarInstantly();
    }
}