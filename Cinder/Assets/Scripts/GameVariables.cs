using System;
using UnityEngine;

[Serializable]
public class GameVariables : BaseObject
{
    public const float BallStartHeight = 2.5f;

    //The ball max speed phase1. The lowest speed of the ball, used for early levels
    public const float BallMaxSpeedPhase1 = 6f;

    // The ball max speed phase1. The lowest speed of the ball, used for early levels
    public const float BallMaxSpeedPhase2 = 6.3f;

    //The ball max speed phase1. The lowest speed of the ball, used for early levels
    public const float BallMaxSpeedPhase3 = 6.6f;

    // The ball max speed phase1. The lowest speed of the ball, used for early levels
    public const float BallMaxSpeedPhase4 = 6.9f;
    public const float BrickShakeAmount = 0.1f;
    public const int PlayerStartingLives = 3;
    public const float PlayersBatYPosition = 1.75f;
    public const float BumperPushForce = 3.0f;
    public const float PowerUpShieldTotalTime = 10;
    public const float TntExplosionRange = 1.0f;
    public const float LaserBatLengthOfTime = 4;
    public const float LaserBatFiringFrequency = 0.25f;
    public const float LaserBulletSpeed = 3.0f;
    public const int BonusLife1PointsThreshold = 7500;
    public const int BonusLife2PointsThreshold = 15000;
    public const int BonusLife3PointsThreshold = 25000;
    public const int BonusLife4PointsThreshold = 40000;
    public const float FlameBallLengthOfTime = 6.0f;
    public const float CrazyBallLengthOfTime = 10.0f;
    public const float WanderingObstacleSpeed = 1.0f;

    /// The percent chance to drop BONUS letter. NOTE: This is an int, so 5% = 5 or 55% = 55
    public const int PercentChanceToDropBonusLetter = 2;
    public const int PercentChanceToDropPoints = 45;
    public const int TotalAmountOfLevels = 66;
    public const int BossDropFreezeTriggerCount = 5;
    public const float FreezePlayerLengthOfTime = 1.0f;
    public const int BossesStartDroppingFreezesFromLevel = 20;

    /// The SFX enabled. 0=false,1=true
    [NonSerialized]
    public int sfxEnabled;

    [NonSerialized]
    public int totalBricksBroken;

    protected void Awake()
    {
        totalBricksBroken = PlayerPrefs.GetInt(Constants.TotalBricksDestroyed);
        sfxEnabled = PlayerPrefs.GetInt(Constants.SfxEnabled);
    }

    public void IncreaseBricksBroken()
    {
        totalBricksBroken++;
    }

    public void StoreTotalBricksBroken()
    {
        PlayerPrefs.SetInt(Constants.TotalBricksDestroyed, totalBricksBroken);
    }
}