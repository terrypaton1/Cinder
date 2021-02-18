using UnityEngine;

[System.Serializable]
public class GameVariables : BaseObject
{
    public const float ballStartHeight = 2.5f;

    //The ball max speed phase1. The lowest speed of the ball, used for early levels
    public const float ballMaxSpeedPhase1 = 6f;

    // The ball max speed phase1. The lowest speed of the ball, used for early levels
    public const float ballMaxSpeedPhase2 = 6.3f;

    //The ball max speed phase1. The lowest speed of the ball, used for early levels
    public const float ballMaxSpeedPhase3 = 6.6f;

    // The ball max speed phase1. The lowest speed of the ball, used for early levels
    public const float ballMaxSpeedPhase4 = 6.9f;

    public const float brickShakeAmount = .1f;

    public const int playerStartingLives = 3;

    public const float playersBatYPosition = 2;

    public const float bumperPushForce = 3;

    public const float powerUpShieldTotalTime = 10;

    public const float tntExplosionRange = 1.0f;

    public const float laserBatLengthOfTime = 4;

    public const float laserBatFiringFrequency = .45f;

    public const float laserBulletSpeed = 3f;

    public const int bonusLife1PointsThreshold = 7500;

    public const int bonusLife2PointsThreshold = 15000;

    public const int bonusLife3PointsThreshold = 25000;

    public const int bonusLife4PointsThreshold = 40000;

    public const float flameBallLengthOfTime = 6;

    public const float crazyBallLengthOfTime = 10;

    public const float wanderingObstacleSpeed = 1f;

    /// The percent chance to drop BONUS letter. NOTE: This is an int, so 5% = 5 or 55% = 55
    public const int percentChanceToDropBONUSLetter = 2;

    public const int percentChanceToDropPoints = 45;
    public const int totalAmountOfLevels = 66;
    public const int bossDropFreezeTriggerCount = 5;

    public const float freezePlayerLengthOfTime = .5f;
    public const int bossesStartDroppingFreezesFromLevel = 20;

    [System.NonSerialized]
    public int totalBricksBroken;

    /// The SFX enabled. 0=false,1=true 
    [System.NonSerialized]
    public int SFXEnabled;

    protected void Awake()
    {
        totalBricksBroken = PlayerPrefs.GetInt(DataVariables.totalBricksDestroyed);
        SFXEnabled = PlayerPrefs.GetInt(DataVariables.SFXEnabled);
    }

    public void IncreaseBricksBroken()
    {
        totalBricksBroken++;
    }

    public void StoreTotalBricksBroken()
    {
        PlayerPrefs.SetInt(DataVariables.totalBricksDestroyed, totalBricksBroken);
    }
}