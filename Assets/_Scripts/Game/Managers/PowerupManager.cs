#region

using System.Collections;
using UnityEngine;

#endregion

public class PowerupManager : BaseObject
{
    [SerializeField]
    protected GameObject shieldObject;

    [SerializeField]
    protected GameObject shieldCollider;

    [SerializeField]
    protected Animator shieldAnimator;

    [SerializeField]
    protected LaserBullet laserBulletPrefabReference;

    private bool crazyBallEnabled;

    private float crazyBallTimer;

    private bool flameBallEnabled;

    private float flameBallTimer;

    private bool freezePlayerEnabled;

    private float freezePlayerTimer;

    private bool laserBatEnabled;

    private float laserBatTimer;

    private bool shieldEnabled;

    private bool shieldPulseShown;

    private float shieldTimer;

    void Awake()
    {
        InstantlyHideShield();
    }

    void Update()
    {
        ManagePowerups();
    }

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    protected    void OnEnable()
    {
        Messenger.AddListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.AddListener(MenuEvents.RestartGame, RestartLevel);
        Messenger<PowerupType>.AddListener(GlobalEvents.ActivatePowerup, ActivatePowerup);
        Messenger<Vector3, Vector3>.AddListener(GlobalEvents.FireLaser, FireLaser);
        Messenger.AddListener(GlobalEvents.LifeLost, LifeLost);
        DisableAllPowerups();
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    protected void OnDisable()
    {
        Messenger.RemoveListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.RemoveListener(MenuEvents.RestartGame, RestartLevel);
        Messenger<PowerupType>.RemoveListener(GlobalEvents.ActivatePowerup, ActivatePowerup);
        Messenger.RemoveListener(GlobalEvents.LifeLost, LifeLost);
        Messenger<Vector3, Vector3>.RemoveListener(GlobalEvents.FireLaser, FireLaser);
    }

    void LevelComplete()
    {
        Messenger.Broadcast(GlobalEvents.HidePowerupBar);
        DisableAllPowerups();
    }

    /// <summary>
    /// Disables all powerups.
    /// </summary>
    void DisableAllPowerups()
    {
        laserBatEnabled = false;
        laserBatTimer = 0;
        freezePlayerEnabled = false;
        freezePlayerTimer = 0;
        shieldEnabled = false;
        shieldTimer = 0;
        crazyBallEnabled = false;
        crazyBallTimer = 0;
        flameBallEnabled = false;
        flameBallTimer = 0;
        InstantlyHideShield();
    }

    void FireLaser(Vector3 position, Vector3 velocity)
    {
        // fire a laser from position, in direction and speed of velocity
//		Debug.Log("fire a bullet");
        var _laserBullet = Instantiate(laserBulletPrefabReference, position, Quaternion.identity);
        _laserBullet.transform.parent = transform;
        _laserBullet.Launch(velocity);
    }

    /// <summary>
    /// Player loses a life
    /// </summary>
    void LifeLost()
    {
        if (shieldEnabled)
            InstantlyHideShield();
        DisableAllPowerups();
    }

    /// <summary>
    /// Activates the powerup.
    /// </summary>
    void ActivatePowerup(PowerupType _powerupType)
    {
        // if the game is not in play mode, then don't activate power up - this is to catch some cases during level complete or game over where a power up may be collcted at the same time as the event occurs
        if (GameManager.instance.gameState != GameState.Playing)
        {
            // Game is not playing, cannot activate a powerup
            return;
        }

        // check that level isn't complete as its possible that a power up is collected just as you complete a level		
//		Debug.Log("ActivatePowerup:" + _powerupType.ToString());
        switch (_powerupType)
        {
            case PowerupType.Multiball:
                PlaySound(SoundList.multiball);
                // add two new balls to the scene, side by side at the starting height
                BallManager.instance.AddMultiballs(-.3f);
                ShowInGameMessage("Multiball!");
                break;
            case PowerupType.WideBat:
                laserBatEnabled = false;
                PlaySound(SoundList.PowerupWideBat);
                PlayersBatManager.instance.ChangeToNewBat(PlayerBatTypes.Wide);
                ShowInGameMessage("Wide!");
                break;
            case PowerupType.SmallBat:
                laserBatEnabled = false;
                PlaySound(SoundList.PowerupSmallBat);
                PlayersBatManager.instance.ChangeToNewBat(PlayerBatTypes.Small);
                ShowInGameMessage("Small!");
                break;
            case PowerupType.LaserBat:
                ActivateLaserBat();
                break;
            case PowerupType.SplitBat:
                PlaySound(SoundList.PowerupSplitBat);
                PlayersBatManager.instance.ChangeToNewBat(PlayerBatTypes.Split);
                ShowInGameMessage("Split!");
                break;
            case PowerupType.Shield:
                PlaySound(SoundList.PowerupShield);
                ShowInGameMessage("Shield!");
                ActivateShield();
                break;
            case PowerupType.Flameball:
                ActivateFlameBall();
                break;
            case PowerupType.CrazyBall:
                ActivateCrazyBall();
                break;
            case PowerupType.FreezePlayer:
                ActivateFreezePlayer();
                break;
        }
    }

    /// <summary>
    /// Manages the powerups.
    /// </summary>
    void ManagePowerups()
    {
        if (shieldEnabled)
        {
            // count down the shield time
            // when time is up, put shield away
            shieldTimer -= Time.deltaTime;
            var percentLeft = shieldTimer / GameVariables.powerupShieldTotalTime;
            PowerupRemainingDisplay.instance.DisplayPercent(percentLeft);
            if (!shieldPulseShown)
            {
                if (shieldTimer < 1.5f)
                {
                    shieldPulseShown = true;
                    shieldAnimator.Play("PulseShield");
                }
            }

            if (shieldTimer <= 0)
            {
                DisableShield();
            }
        }

        if (laserBatEnabled)
        {
            laserBatTimer -= Time.deltaTime;
            // display percent left
            var percentLeft = laserBatTimer / GameVariables.laserBatLengthOfTime;
            PowerupRemainingDisplay.instance.DisplayPercent(percentLeft);
            if (laserBatTimer <= 0)
            {
                DisableLaserBat();
            }
        }

        if (flameBallEnabled)
        {
            flameBallTimer -= Time.deltaTime;
            if (flameBallTimer <= 0)
            {
                Messenger.Broadcast(GlobalEvents.DisableFlameBall, MessengerMode.DONT_REQUIRE_LISTENER);
            }
        }

        if (crazyBallEnabled)
        {
            crazyBallTimer -= Time.deltaTime;
            // display percent left
            var percentLeft = crazyBallTimer / GameVariables.crazyBallLengthOfTime;
            PowerupRemainingDisplay.instance.DisplayPercent(percentLeft);
            if (crazyBallTimer <= 0)
            {
                Messenger.Broadcast(GlobalEvents.DisableCrazyBall, MessengerMode.DONT_REQUIRE_LISTENER);
                DisableCrazyBall();
            }
        }

        if (freezePlayerEnabled)
        {
            freezePlayerTimer -= Time.deltaTime;
            // display percent left
//			float percentLeft = crazyBallTimer / GameVariables.crazyBallLengthOfTime;
//			PowerupRemainingDisplay.instance.DisplayPercent(percentLeft);
// create freeze particle effect where player is
//			Debug.Log("create freeze particle effect where player is");
//			Debug.Log("freezePlayerTimer:"+freezePlayerTimer);
            if (freezePlayerTimer <= 0)
            {
                Messenger.Broadcast(GlobalEvents.DisableFreezePlayer, MessengerMode.DONT_REQUIRE_LISTENER);
                DisableFreezePlayer();
            }
        }
    }

    /// <summary>
    /// Activates the laser bat.
    /// </summary>
    void ActivateLaserBat()
    {
//		Debug.Log("ActivateLaserBat");
        Messenger.Broadcast(GlobalEvents.DisplayPowerupBar);
        laserBatEnabled = true;
        laserBatTimer = GameVariables.laserBatLengthOfTime;
        PlaySound(SoundList.PowerupLaser);
        PlayersBatManager.instance.ChangeToNewBat(PlayerBatTypes.Laser);
        ShowInGameMessage("Laser!");
    }

    /// <summary>
    /// Disables the laser bat.
    /// </summary>
    void DisableLaserBat()
    {
        laserBatEnabled = false;
//		Debug.Log("DisableLaserBat");
        PlayersBatManager.instance.ChangeToNewBat(PlayerBatTypes.Normal);
        TestDisablePowerupBar();
    }

    /// <summary>
    /// Tests the disable powerup bar. Only disable it if no other powerups are active
    /// </summary>
    void TestDisablePowerupBar()
    {
        if (laserBatEnabled) return;
        if (shieldEnabled) return;
        if (crazyBallEnabled) return;
        Messenger.Broadcast(GlobalEvents.HidePowerupBar);
    }

    void ActivateFreezePlayer()
    {
        Debug.Log("ActivateFreezePlayer");
//		Debug.Break();
        ShowInGameMessage("FREEZE!");
        freezePlayerEnabled = true;
        PlayersBatManager.instance.ActivateFreezePlayer();
        freezePlayerTimer = GameVariables.freezePlayerLengthOfTime;
        PlaySound(SoundList.PowerupFreeze);
    }

    /// <summary>
    /// Disables the crazy ball.
    /// </summary>
    void DisableFreezePlayer()
    {
        freezePlayerEnabled = false;
        PlayersBatManager.instance.DisableFreezePlayer();
    }

    /// <summary>
    /// Activates the crazy ball.
    /// </summary>
    void ActivateCrazyBall()
    {
        Messenger.Broadcast(GlobalEvents.DisplayPowerupBar);
//		Debug.Log("ActivateCrazyBall");
        PlaySound(SoundList.PowerupFireball);
        ShowInGameMessage("Crazy Ball!");
        // tell all current balls to go into flame mode
        Messenger.Broadcast(GlobalEvents.ActivateCrazyBall, MessengerMode.DONT_REQUIRE_LISTENER);
        crazyBallEnabled = true;
        crazyBallTimer = GameVariables.crazyBallLengthOfTime;
        PlaySound(SoundList.PowerupCrazyBall);
    }

    /// <summary>
    /// Disables the crazy ball.
    /// </summary>
    void DisableCrazyBall()
    {
        crazyBallEnabled = false;
//		Messenger.Broadcast(GlobalEvents.HidePowerupBar);
        TestDisablePowerupBar();
    }

    /// <summary>
    /// Restarts the level.
    /// </summary>
    void RestartLevel()
    {
        DisableAllPowerups();
        Messenger.Broadcast(GlobalEvents.HidePowerupBarInstantly);
        InstantlyHideShield();
    }

    /// <summary>
    /// Disables the shield.
    /// </summary>
    void DisableShield()
    {
//		Debug.Log("DisableShield");
        StartCoroutine(DisableShieldSequence());
        shieldAnimator.Play("DisableShield");
        //Messenger.Broadcast(GlobalEvents.HidePowerupBar);
        TestDisablePowerupBar();
    }

    /// <summary>
    /// Disables the shield sequence.
    /// </summary>
    IEnumerator DisableShieldSequence()
    {
        shieldEnabled = false;
        shieldCollider.SetActive(false);
        yield return new WaitForSeconds(1);
        InstantlyHideShield();
    }

    /// <summary>
    /// Enables the shield.
    /// </summary>
    void ActivateShield()
    {
        Messenger.Broadcast(GlobalEvents.DisplayPowerupBar);
        shieldPulseShown = false;
        Debug.Log("ActivateShield");
        shieldTimer = GameVariables.powerupShieldTotalTime;
        shieldCollider.SetActive(true);
        shieldObject.SetActive(true);
        shieldEnabled = true;
        shieldAnimator.Play("ActivateShield");
    }

    /// <summary>
    /// Instantlies the hide shield.
    /// </summary>
    void InstantlyHideShield()
    {
        shieldEnabled = false;
        shieldTimer = 0;
        shieldPulseShown = false;
        shieldCollider.SetActive(false);
        shieldObject.SetActive(false);
    }

    /// <summary>
    /// Activates the flame ball.
    /// </summary>
    void ActivateFlameBall()
    {
        PlaySound(SoundList.PowerupFireball);
        ShowInGameMessage("Fireball!");
        // tell all current balls to go into flame mode
        Messenger.Broadcast(GlobalEvents.ActivateFlameBall, MessengerMode.DONT_REQUIRE_LISTENER);
        flameBallEnabled = true;
        flameBallTimer = GameVariables.flameBallLengthOfTime;
        PlaySound(SoundList.PowerupLaser);
    }
}