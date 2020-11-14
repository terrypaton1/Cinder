using UnityEngine;

[System.Serializable]
public class GameVariables : BaseObject
{
    /// <summary> The display debug overlay. </summary>
    public const bool displayDebugOverlay = false;

    /// <summary> The height of the ball start. </summary>
    public const float ballStartHeight = 2.5f;

    /// <summary> The ball max speed phase1. The lowest speed of the ball, used for early levels </summary>
    public const float ballMaxSpeedPhase1 = 6f;

    /// <summary> The ball max speed phase1. The lowest speed of the ball, used for early levels </summary>
    public const float ballMaxSpeedPhase2 = 6.3f;

    /// <summary> The ball max speed phase1. The lowest speed of the ball, used for early levels </summary>
    public const float ballMaxSpeedPhase3 = 6.6f;

    /// <summary> The ball max speed phase1. The lowest speed of the ball, used for early levels </summary>
    public const float ballMaxSpeedPhase4 = 6.9f;

    /// <summary> The brick shake amount. </summary>
    public const float brickShakeAmount = .1f;

    /// <summary> The maximum falling item speed. - used for powerups and falling points </summary>
    public const float maximumFallingItemSpeed = .055f;

    /// <summary> The maximum falling letter item speed. </summary>
    public const float maximumFallingLetterItemSpeed = .08f;

    /// <summary> The falling letter collected point value. </summary>
    public const int fallingLetterCollectedPointValue = 100;

    /// <summary> The player starting lives. </summary>
    public const int playerStartingLives = 3;

    /// <summary> The players bat Y position. </summary>
    public const float playersBatYPosition = 2;

    /// <summary>  The multihit1 points. </summary>
    public const int multihit1Points = 500;

    /// <summary> The multihit2 points. </summary>
    public const int multihit2Points = 1000;

    /// <summary> The multihit3 points. </summary>
    public const int multihit3Points = 1500;

    /// <summary> The multihit4 points. </summary>
    public const int multihit4Points = 2000;

    /// <summary> The multihit5 points. </summary>
    public const int multihit5Points = 4000;

    /// <summary> The multihit6 points.</summary>
    public const int multihit6Points = 7500;

    /// <summary> The multihit7 points. </summary>
    public const int multihit7Points = 10000;

    /// <summary> The multihit8 points. </summary>
    public const int multihit8Points = 20000;

    /// <summary> The bumper push force. </summary>
    public const float bumperPushForce = 3;

    /// <summary> The powerup shield total time.</summary>
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

    /// <summary>
    /// The freeze player length of time.
    /// </summary>
    public const float freezePlayerLengthOfTime = .5f;

    /// <summary>
    /// The bosses start dropping freezes from level.
    /// </summary>
    public const int bossesStartDroppingFreezesFromLevel = 20;

    /// <summary> The total bricks broken. </summary>
    [System.NonSerialized]
    public int totalBricksBroken = 0;

    /// <summary> The SFX enabled. 0=false,1=true </summary>
    [System.NonSerialized]
    public int SFXEnabled = 0;

    /// <summary>
    /// The player is logged in.
    /// </summary>
    [System.NonSerialized]
    public bool playerIsLoggedIn = false;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        totalBricksBroken = PlayerPrefs.GetInt(DataVariables.totalBricksDestroyed);
        SFXEnabled = PlayerPrefs.GetInt(DataVariables.SFXEnabled);
    }

    /// <summary>
    /// Increases the bricks broken.
    /// </summary>
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

    // ************************************
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    // ************************************
    private static GameVariables s_Instance = null;

    // ************************************
    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    // ************************************
    public static GameVariables instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(GameVariables)) as GameVariables;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                Debug.LogError("Could not locate an GameVariables object!");
                Debug.Break();
            }

            return s_Instance;
        }
    }

    // ************************************
    // Ensure that the instance is destroyed when the game is stopped in the editor.
    // ************************************
    void OnApplicationQuit()
    {
        s_Instance = null;
    }

    #endregion
}