using UnityEngine;

[System.Serializable]
public class GameVariables : BaseObject
{
    public const bool displayDebugOverlay = false;

    public const float ballStartHeight = 2.5f;

    public const float ballMaxSpeedPhase1 = 6f;

    public const float ballMaxSpeedPhase2 = 6.3f;

    public const float ballMaxSpeedPhase3 = 6.6f;

    public const float ballMaxSpeedPhase4 = 6.9f;

    public const float brickShakeAmount = .1f;

    public const float maximumFallingItemSpeed = .055f;

    public const float maximumFallingLetterItemSpeed = .08f;

    public const int fallingLetterCollectedPointValue = 100;

    public const int playerStartingLives = 3;

    public const float playersBatYPosition = 2;
    public const int multihit1Points = 500;
    public const int multihit2Points = 1000;
    public const int multihit3Points = 1500;
    public const int multihit4Points = 2000;
    public const int multihit5Points = 4000;
    public const int multihit6Points = 7500;
    public const int multihit7Points = 10000;
    public const int multihit8Points = 20000;

    public const float bumperPushForce = 3;

    public const float powerupShieldTotalTime = 10;

    public const float tntExplosionRange = .75f;
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
    public const int wanderingObstaclePointsValue = 500;
    public const int brickPointsValue = 10;
    public const int fallingPointValues1 = 10;
    public const int fallingPointValues2 = 50;
    public const int fallingPointValues3 = 100;
    public const int fallingPointValues4 = 500;

    public const int AllLettersOfBONUSCollectedPoints = 5000;

    public const int percentChanceToDropBONUSLetter = 2;

    public const int percentChanceToDropPoints = 45;

    public const int bossPointsValue = 250;

    public const int totalAmountofLevels = 66;

    public const int bossDropFreezeTriggerCount = 5;

    public const float freezePlayerLengthOfTime = .5f;

    public const int bossesStartDroppingFreezesFromLevel = 20;

    [System.NonSerialized]
    public int totalBricksBroken ;

    [System.NonSerialized]
    public int SFXEnabled ;

    [System.NonSerialized]
    public bool playerIsLoggedIn = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        totalBricksBroken = PlayerPrefs.GetInt(DataVariables.totalBricksDestroyed);
        SFXEnabled = PlayerPrefs.GetInt(DataVariables.SFXEnabled);
    }

    public void IncreaseBricksBroken()
    {
        totalBricksBroken++;
//		Debug.Log("totalBricksBroken:"+totalBricksBroken);
        if (totalBricksBroken == 100)
        {
            Messenger<string, float>.Broadcast(SocialEvents.ReportAchievementProgress, AchievementIDs.Break100Bricks,
                100);
        }

        if (totalBricksBroken == 1000)
        {
            Messenger<string, float>.Broadcast(SocialEvents.ReportAchievementProgress, AchievementIDs.Break1000Bricks,
                100);
        }

        if (totalBricksBroken == 10000)
        {
            Messenger<string, float>.Broadcast(SocialEvents.ReportAchievementProgress, AchievementIDs.Break10000Bricks,
                100);
        }
    }

    /// <summary>
    /// Stores the total bricks broken.
    /// </summary>
    public void StoreTotalBricksBroken()
    {
        PlayerPrefs.SetInt(DataVariables.totalBricksDestroyed, GameVariables.instance.totalBricksBroken);
    }

    protected GameVariables()
    {
    }

    #region instance

    private static GameVariables s_Instance;

    public static GameVariables instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GameVariables)) as GameVariables;
            }

            if (s_Instance == null)
            {
                Debug.LogError("Could not locate an GameVariables object!");
                Debug.Break();
            }

            return s_Instance;
        }
    }

    void OnApplicationQuit()
    {
        s_Instance = null;
    }

    #endregion
}