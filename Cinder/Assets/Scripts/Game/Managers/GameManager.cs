using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : BaseObject
{
    [SerializeField]
    public ParticleManager particleManager;

    [SerializeField]
    public BallManager ballManager;

    [SerializeField]
    public BrickManager brickManager;

    [FormerlySerializedAs("powerupManager")]
    [SerializeField]
    public PowerupManager powerUpManager;

    [Space(10)]
    [SerializeField]
    public PlayersBatManager playersBatManager;

    [SerializeField]
    public ScoreManager scoreManager;

    [SerializeField]
    public BONUSManager bonusManager;

    [Space(10)]
    [SerializeField]
    public LevelTimer levelTimer;

    [SerializeField]
    public PlayerLifeManager playerLifeManager;

    [FormerlySerializedAs("fallingPointsManager")]
    [SerializeField]
    public FallingObjectsManager fallingObjectsManager;

    [SerializeField]
    public GameVariables gameVariables;

    [SerializeField]
    public Backgrounds backgrounds;

    [SerializeField]
    public TouchPosition touchPosition;

    [SerializeField]
    public GameSettings gameSettings;

    private GameState _lastGameState = GameState.Setup;

    private IEnumerator coroutine;

    private GameState gameState = GameState.Setup;
    private IEnumerator StartGameCoroutine;

    protected void Update()
    {
        if (gameState == GameState.Playing || gameState == GameState.Setup)
        {
            brickManager.UpdateLoop();
        }
    }

    protected void OnEnable()
    {
        CoreConnector.GameManager = this;
    }

    public void StartGame()
    {
        StopRunningCoroutine(StartGameCoroutine);
        StartGameCoroutine = StartGameSequence();
        StartCoroutine(StartGameCoroutine);
    }

    public void PerformInitialSetup()
    {
        fallingObjectsManager.BuildFallingPointsPool();
        fallingObjectsManager.BuildFallingPowerUpsPool();
        ballManager.Setup();
    }

    private IEnumerator StartGameSequence()
    {
        ballManager.ResetAllBalls();
        playersBatManager.HideAllBats();
        Time.timeScale = 1;

        CoreConnector.UIManager.DisplayLevelLoader();

        yield return new WaitForSeconds(0.2f);

        var levelNumber = PlayerPrefs.GetInt(DataVariables.currentLevel);

        CoreConnector.GameUIManager.playerLifeDisplay.Show();
        CoreConnector.LevelsManager.DisplayLevel(levelNumber);
        brickManager.LoadLevelsBricks();
        backgrounds.DisplayForLevel(levelNumber);
        fallingObjectsManager.HideAll();

        yield return new WaitForSeconds(0.2f);
        CoreConnector.UIManager.HideAllScreens();
        CoreConnector.UIManager.DisplayScreen(UIScreens.Game);

        CoreConnector.GameUIManager.DisplayInGameButtons(true);

        playerLifeManager.RestartLevel();

        yield return new WaitForSeconds(0.5f);
        touchPosition.ResumeGame();

        StartPlay(0.25f);
    }

    public void NextLevel()
    {
        brickManager.NextLevel();
        ballManager.GoingToNextLevel();
        levelTimer.ResetTimer();
        playersBatManager.GoingToNextLevel();
        levelTimer.ResetTimer();

        ChangeGameState(GameState.Setup);

        IncrementPlayersCurrentLevel();
        StartGame();
    }

    private void IncrementPlayersCurrentLevel()
    {
        var levelNumber = PlayerPrefs.GetInt(DataVariables.currentLevel);
        levelNumber++;
        PlayerPrefs.SetInt(DataVariables.currentLevel, levelNumber);
    }

    private void PlayerLostAllLives()
    {
        levelTimer.StopTimer();

        ChangeGameState(GameState.GameOver);
        CoreConnector.GameManager.gameVariables.StoreTotalBricksBroken();

        var playersFinalScore = CoreConnector.GameManager.scoreManager.playerScore;
        // not doing with the players final score. But here it is!
        Debug.Log("players final score:" + playersFinalScore);

        CoreConnector.UIManager.DisplayScreen(UIScreens.GameOver);
    }

    public void QuitGame()
    {
        ChangeGameState(GameState.GameOver);

        Time.timeScale = 1;

        CoreConnector.GameManager.gameVariables.StoreTotalBricksBroken();
        CoreConnector.LevelsManager.HideAllLevels();
        CoreConnector.GameUIManager.gameMessages.HideInGameMessageInstantly();
        playersBatManager.Reset();
        CoreConnector.GameUIManager.playerLifeDisplay.Hide();
        ballManager.ResetAllBalls();
        fallingObjectsManager.HideAll();
    }

    public void PauseGame()
    {
        touchPosition.PauseGame();
        levelTimer.StopTimer();
        ChangeGameState(GameState.Paused);
        Time.timeScale = 0;
    }

    private void ChangeGameState(GameState newState)
    {
        _lastGameState = gameState;
        gameState = newState;
    }

    public void ResumeGame()
    {
//		Debug.Log("Resume Game");
        CoreConnector.UIManager.ShowGamePlayUI();
        CoreConnector.GameUIManager.DisplayInGameButtons(true);

        levelTimer.StartTimer();
        touchPosition.ResumeGame();
        gameState = _lastGameState;
        Time.timeScale = 1;
    }

    public void LoseLife()
    {
        StartCoroutine(PlayerLifeLostSequence());
    }

    private IEnumerator PlayerLifeLostSequence()
    {
        // It is intentional that there is no message telling the player they have lost a life.
        ChangeGameState(GameState.LifeLost);
        PlaySound(SoundList.LifeLost);

        playerLifeManager.PlayerLosesALife();
        playersBatManager.PlayerLosesLife();
        
        ballManager.LifeLost();
        brickManager.LifeLost();
        powerUpManager.LifeLost();
        
        CoreConnector.GameUIManager.LifeLost();

        levelTimer.StopTimer();

        yield return new WaitForSeconds(0.5f);

        if (playerLifeManager.PlayerLives < 1)
        {
            // game over;
            CoreConnector.GameManager.PlayerLostAllLives();
            yield break;
        }

        // player still has lives left!
        StartPlay(2.0f);
    }

    private void StartPlay(float initialDelay)
    {
        StopRunningCoroutine(coroutine);
        coroutine = StartPlaySequence(initialDelay);
        StartCoroutine(coroutine);
    }

    private IEnumerator StartPlaySequence(float initialDelay)
    {
        ChangeGameState(GameState.StartPlay);
        playersBatManager.RestartLevel();
        ballManager.RestartLevel();
        powerUpManager.RestartLevel();

        yield return new WaitForSeconds(initialDelay);

        ballManager.AddNewBall();
        levelTimer.StartTimer();
        ChangeGameState(GameState.Playing);
    }

    public void RestartGame()
    {
        // this should perhaps go through the startGaMe
        levelTimer.ResetTimer();

        brickManager.RestartLevel();
        playersBatManager.RestartLevel();
        ballManager.RestartLevel();

        CoreConnector.GameUIManager.bossHealthRemainingDisplay.Hide();
        scoreManager.RestartLevel();
        powerUpManager.RestartLevel();

        //todo reset the players lives too?
        playerLifeManager.RestartLevel();

        bonusManager.RestartGame(); // todo do I need to do this line, I don't think so?

        StartPlay(0.5f);
    }

    public override void LevelComplete()
    {
        ChangeGameState(GameState.LevelComplete);
        StartCoroutine(LevelCompleteSequence());
    }

    private IEnumerator LevelCompleteSequence()
    {
        playersBatManager.LevelComplete();
        brickManager.LevelComplete();
        powerUpManager.LevelComplete();
        fallingObjectsManager.LevelComplete();

        levelTimer.StopTimer();

        // check if the max level beaten should be increased
        var maxLevelBeaten = PlayerPrefs.GetInt(DataVariables.maxLevelBeatenPrefix);
        var nextLevelNumber = PlayerPrefs.GetInt(DataVariables.currentLevel) + 1;
        if (nextLevelNumber > maxLevelBeaten)
        {
            maxLevelBeaten = nextLevelNumber;
            // todo move this to a data manager class
            PlayerPrefs.SetInt(DataVariables.maxLevelBeatenPrefix, maxLevelBeaten);
        }

        if (nextLevelNumber > GameVariables.totalAmountOfLevels)
        {
            PlaySound(SoundList.levelComplete);
            // int playersFinalScore = CoreConnector.GameManager.scoreManager.playerScore;
            // Debug.Log("players final score:" + playersFinalScore);
            CoreConnector.UIManager.DisplayScreen(UIScreens.GameComplete);
        }
        else
        {
            PlaySound(SoundList.levelComplete);
            CoreConnector.UIManager.DisplayScreen(UIScreens.LevelComplete);
        }

        yield return null;

        ballManager.LevelComplete();
    }

    public void ActivateFlameBall()
    {
        brickManager.ActivateFlameBall();
        ballManager.ActivateFlameBall();
    }

    public void DisableFlameBall()
    {
        brickManager.DisableFlameBall();
        ballManager.DisableFlameBall();
    }

    public bool IsGamePlaying()
    {
        return gameState == GameState.Playing;
    }

    public void ExitGame()
    {
        CoreConnector.LevelsManager.HideAllLevels();
    }
}