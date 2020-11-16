using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Setup = 0,
    Playing = 10,
    GameOver = 20,
    LevelComplete = 40,
    Paused = 50
}

public class GameManager : BaseObject
{
    public bool levelIsComplete;

    public TouchPosition touchPosition;

    private GameState _gameState = GameState.Setup;

    private GameState _lastGameState = GameState.Setup;

    private string currentLevelString = "";

    public GameState gameState => _gameState;

    protected void Start()
    {
        StartCoroutine(StartGameSequence());
    }

    protected void OnEnable()
    {
//		DontDestroyOnLoad(gameObject);
        Messenger.AddListener(MenuEvents.RestartGame, RestartGame);
        Messenger.AddListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.AddListener(GlobalEvents.PauseGame, PauseGame);
        Messenger.AddListener(GlobalEvents.ResumeGame, ResumeGame);
        Messenger.AddListener(GlobalEvents.QuitGame, QuitGame);
        Messenger.AddListener(GlobalEvents.GameOver, GameOver);
        Messenger.AddListener(MenuEvents.NextLevel, NextLevel);
    }

    protected void OnDisable()
    {
        Messenger.RemoveListener(MenuEvents.RestartGame, RestartGame);
        Messenger.RemoveListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.RemoveListener(GlobalEvents.PauseGame, PauseGame);
        Messenger.RemoveListener(GlobalEvents.ResumeGame, ResumeGame);
        Messenger.RemoveListener(GlobalEvents.QuitGame, QuitGame);
        Messenger.RemoveListener(GlobalEvents.GameOver, GameOver);
        Messenger.RemoveListener(MenuEvents.NextLevel, NextLevel);
    }

    private IEnumerator StartGameSequence()
    {
//		Debug.Log("lets load a level");		
        // check if a level is loaded already
        if (GameRunner.instance)
        {
            // a level is already loaded
        }
        else
        {
            yield return StartCoroutine(LoadLevel());
        }

        // now destroy the GameRunner instance
        Destroy(GameRunner.instance.gameObject);
        while (GameRunner.instance == null)
        {
//			Debug.Log("waiting for level to load");
            yield return 0;
        }

//		Debug.Log("level loaded an ready to go!");
        yield return new WaitForSeconds(1f);
        Messenger<bool>.Broadcast(MenuEvents.DisplayLevelLoader, false, MessengerMode.DONT_REQUIRE_LISTENER);
        Messenger<bool>.Broadcast(MenuEvents.DisplayInGameButtons, true, MessengerMode.DONT_REQUIRE_LISTENER);
        yield return new WaitForSeconds(.5f);
        BallManager.instance.AddNewBall();

        Messenger.Broadcast(GlobalEvents.StartLevelTimer, MessengerMode.DONT_REQUIRE_LISTENER);
        _gameState = GameState.Playing;
    }

    private void NextLevel()
    {
//		Debug.Log("NextLevel");

        _gameState = GameState.Setup;
        Messenger.Broadcast(GlobalEvents.HideAllMenus);
        StartCoroutine(LoadNextLevelSequence());
    }

    private IEnumerator LoadNextLevelSequence()
    {
//		Debug.Log("LoadNextLevelSequence");
        // unload the current game level
        Messenger<bool>.Broadcast(MenuEvents.DisplayLevelLoader, true, MessengerMode.DONT_REQUIRE_LISTENER);
        var levelNumber = PlayerPrefs.GetInt(DataVariables.currentLevel);
        UnloadCurrentLevel();
        yield return new WaitForSeconds(.5f);
        levelNumber++;
        PlayerPrefs.SetInt(DataVariables.currentLevel, levelNumber);
        StartCoroutine(StartGameSequence());
    }

    private void GameOver()
    {
//		Debug.Log("Game over");

        _gameState = GameState.GameOver;
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        GameVariables.instance.StoreTotalBricksBroken();

        var levelNumber = PlayerPrefs.GetInt(DataVariables.currentLevel);
        // reports game over and on the level game over happened
        Messenger<string, string>.Broadcast(GlobalEvents.AnalyticsEvent, BrickAnalytics.gameOver,
            "Level" + levelNumber);
        var playersFinalScore = ScoreManager.instance.playerScore;
//		Debug.Log("players final score:" + playersFinalScore);
        Messenger<int>.Broadcast(SocialEvents.ReportScore, playersFinalScore, MessengerMode.DONT_REQUIRE_LISTENER);
        Messenger<bool>.Broadcast(MenuEvents.DisplayGameOver, true);
        // animate in the game over screen
        yield return 0; //new WaitForSeconds(.1f);
    }

    private void QuitGame()
    {
//		Debug.Log("QuitGame");
        // stop everything
        _gameState = GameState.GameOver;
        Time.timeScale = 1;
        GameVariables.instance.StoreTotalBricksBroken();
        Messenger<bool>.Broadcast(MenuEvents.DisplayLevelLoader, true, MessengerMode.DONT_REQUIRE_LISTENER);
        UnloadCurrentLevel();
        Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.LevelChooser);
        Messenger<string, string>.Broadcast(GlobalEvents.AnalyticsEvent, BrickAnalytics.changeScene, "QuitGame");
    }

    private void UnloadCurrentLevel()
    {
        var levelNumber = PlayerPrefs.GetInt(DataVariables.currentLevel);
        if (levelNumber < 10)
        {
            currentLevelString = DataVariables.levelNamePrefix + "0" + levelNumber;
        }
        else
        {
            currentLevelString = DataVariables.levelNamePrefix + levelNumber;
        }

//		Debug.Log("currentLevelString:" + currentLevelString);
        SceneManager.UnloadScene(currentLevelString);
    }

    private void PauseGame()
    {
        _lastGameState = _gameState;
        _gameState = GameState.Paused;
        //		Debug.Log("Pause Game");
        var levelNumber = PlayerPrefs.GetInt(DataVariables.currentLevel);
        Messenger<string, string>.Broadcast(GlobalEvents.AnalyticsEvent, BrickAnalytics.gamePaused,
            "Level" + levelNumber);
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        _gameState = _lastGameState;
//		Debug.Log("Resume Game");
        Time.timeScale = 1;
    }

    void RestartGame()
    {
        _gameState = GameState.Playing;
        var levelNumber = PlayerPrefs.GetInt(DataVariables.currentLevel);
        Messenger<string, string>.Broadcast(GlobalEvents.AnalyticsEvent, BrickAnalytics.gameRestarted,
            "Level" + levelNumber);
#if UNITY_EDITOR
        Debug.Log("RestartGame");
#endif
    }

    private void LevelComplete()
    {
//		Debug.Log("LEVEL COMPLETE");
        _gameState = GameState.LevelComplete;
        levelIsComplete = true;
        StartCoroutine(LevelCompleteSequence());
    }

    private IEnumerator LevelCompleteSequence()
    {
//		Debug.Log("LevelCompleteSequence");
// check for boss beaten achievements
        PlayersBatManager.instance.ChangeToNewBat(PlayerBatTypes.Normal);
        var levelNumber = PlayerPrefs.GetInt(DataVariables.currentLevel);

        Messenger<string, string>.Broadcast(GlobalEvents.AnalyticsEvent, BrickAnalytics.levelComplete,
            "Level" + levelNumber);

        if (levelNumber == 5)
        {
            Messenger<string, float>.Broadcast(SocialEvents.ReportAchievementProgress, AchievementIDs.BeatLevel5Boss,
                100);
        }

        if (levelNumber == 10)
        {
            Messenger<string, float>.Broadcast(SocialEvents.ReportAchievementProgress, AchievementIDs.BeatLevel10Boss,
                100);
        }

        if (levelNumber == 20)
        {
            Messenger<string, float>.Broadcast(SocialEvents.ReportAchievementProgress, AchievementIDs.BeatLevel20Boss,
                100);
        }

        if (levelNumber == 30)
        {
            Messenger<string, float>.Broadcast(SocialEvents.ReportAchievementProgress, AchievementIDs.BeatLevel30Boss,
                100);
        }

        if (levelNumber == 40)
        {
            Messenger<string, float>.Broadcast(SocialEvents.ReportAchievementProgress, AchievementIDs.BeatLevel40Boss,
                100);
        }

        if (levelNumber == 50)
        {
            Messenger<string, float>.Broadcast(SocialEvents.ReportAchievementProgress, AchievementIDs.BeatLevel50Boss,
                100);
        }

        if (levelNumber == 60)
        {
            Messenger<string, float>.Broadcast(SocialEvents.ReportAchievementProgress, AchievementIDs.BeatLevel60Boss,
                100);
        }

        if (levelNumber == 66)
        {
            Messenger<string, float>.Broadcast(SocialEvents.ReportAchievementProgress, AchievementIDs.BeatLevel66Boss,
                100);
        }

// check if the max level beaten should be increased
        var maxLevelBeaten = PlayerPrefs.GetInt(DataVariables.maxLevelBeatenPrefix);
        var nextLevelNumber = PlayerPrefs.GetInt(DataVariables.currentLevel) + 1;
        if (nextLevelNumber > maxLevelBeaten)
        {
            maxLevelBeaten = nextLevelNumber;
            PlayerPrefs.SetInt(DataVariables.maxLevelBeatenPrefix, maxLevelBeaten);
            Messenger.Broadcast(SocialEvents.SaveThePlayersGame, MessengerMode.DONT_REQUIRE_LISTENER);
        }

        if (nextLevelNumber > GameVariables.totalAmountofLevels)
        {
            PlaySound(SoundList.levelComplete);
            Messenger<string, string>.Broadcast(GlobalEvents.AnalyticsEvent, BrickAnalytics.GameComplete,
                "Level" + levelNumber);
            var playersFinalScore = ScoreManager.instance.playerScore;
//		Debug.Log("players final score:" + playersFinalScore);
            Messenger<int>.Broadcast(SocialEvents.ReportScore, playersFinalScore, MessengerMode.DONT_REQUIRE_LISTENER);
            Messenger<bool>.Broadcast(MenuEvents.DisplayGameComplete, true);
        }
        else
        {
            PlaySound(SoundList.levelComplete);
            Messenger<bool>.Broadcast(MenuEvents.DisplayLevelComplete, true);
        }

        // check if the game is complete
        yield return 0;
    }

    private IEnumerator LoadLevel()
    {
        // load in the game scene, get current level, display a level loader
        Messenger<bool>.Broadcast(MenuEvents.DisplayLevelLoader, true);
        var levelNumber = PlayerPrefs.GetInt(DataVariables.currentLevel);
        if (levelNumber < 10)
        {
            currentLevelString = DataVariables.levelNamePrefix + "0" + levelNumber;
        }
        else
        {
            currentLevelString = DataVariables.levelNamePrefix + levelNumber;
        }

        var async = SceneManager.LoadSceneAsync(currentLevelString, LoadSceneMode.Additive);

        yield return async;

        Messenger<bool>.Broadcast(MenuEvents.DisplayLevelLoader, false);
    }

    protected void OnDestroy()
    {
        s_Instance = null;
    }

    private static GameManager s_Instance;

    public static GameManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                Debug.LogError("Could not locate an ScoreManager object!");
                Debug.Break();
            }

            return s_Instance;
        }
    }

    protected void OnApplicationQuit()
    {
        s_Instance = null;
    }
}