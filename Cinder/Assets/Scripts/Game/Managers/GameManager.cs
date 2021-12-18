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

    private GameState lastGameState = GameState.Setup;

    private IEnumerator startPlayCoroutine;

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
        fallingObjectsManager.Initialize();
        bonusManager.Setup();
        powerUpManager.OneTimeSetup();
        ballManager.Setup();
        bonusManager.HideAllLetters();
    }

    private IEnumerator StartGameSequence()
    {
        ballManager.ResetAllBalls();
        playersBatManager.HideAllBats();
        Time.timeScale = 1.0f;
        CoreConnector.UIManager.DisplayLevelLoader();

        var levelNumber = PlayerPrefs.GetInt(Constants.currentLevel);

        CoreConnector.GameUIManager.playerLifeDisplay.Show();
        CoreConnector.LevelManager.DisplayLevel(levelNumber);

        yield return new WaitForSeconds(0.1f);
        brickManager.LoadLevelsBricks();
        yield return new WaitForSeconds(0.1f);
        backgrounds.DisplayForLevel(levelNumber);
        yield return new WaitForSeconds(0.1f);
        fallingObjectsManager.HideAll();
        yield return new WaitForSeconds(0.1f);
        CoreConnector.UIManager.HideAllScreens();

        yield return new WaitForSeconds(0.1f);
        CoreConnector.UIManager.DisplayScreen(UIScreens.Game);
        CoreConnector.GameUIManager.DisplayInGameButtons(true);
        bonusManager.RestartGame();
        playerLifeManager.RestartLevel();
        yield return new WaitForSeconds(0.1f);

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

    private static void IncrementPlayersCurrentLevel()
    {
        var levelNumber = PlayerPrefs.GetInt(Constants.currentLevel);
        levelNumber++;
        PlayerPrefs.SetInt(Constants.currentLevel, levelNumber);
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

    public void PauseGame()
    {
        touchPosition.PauseGame();
        levelTimer.StopTimer();
        ChangeGameState(GameState.Paused);
        Time.timeScale = 0;
    }

    private void ChangeGameState(GameState newState)
    {
        lastGameState = gameState;
        gameState = newState;
    }

    public void ResumeGame()
    {
        CoreConnector.UIManager.ShowGamePlayUI();
        CoreConnector.GameUIManager.DisplayInGameButtons(true);

        levelTimer.StartTimer();
        touchPosition.ResumeGame();
        gameState = lastGameState;
        Time.timeScale = 1.0f;
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
        fallingObjectsManager.LifeLost();
        CoreConnector.GameUIManager.LifeLost();

        levelTimer.StopTimer();

        yield return new WaitForSeconds(1.0f);

        if (playerLifeManager.PlayerLives < 1)
        {
            // game over;
            CoreConnector.GameManager.PlayerLostAllLives();
            yield break;
        }

        // player still has lives left!
        StartPlay(0.5f);
    }

    private void StartPlay(float initialDelay)
    {
        StopRunningCoroutine(startPlayCoroutine);
        startPlayCoroutine = StartPlaySequence(initialDelay);
        StartCoroutine(startPlayCoroutine);
    }

    private IEnumerator StartPlaySequence(float initialDelay)
    {
        ChangeGameState(GameState.StartPlay);
        ballManager.RestartLevel();
        powerUpManager.RestartLevel();
        playersBatManager.RestartLevel();

        // during this time I want to track the mouse
        yield return new WaitForSeconds(initialDelay);

        ballManager.AddNewBall();
        levelTimer.StartTimer();
        ChangeGameState(GameState.Playing);
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
        var maxLevelBeaten = PlayerPrefs.GetInt(Constants.maxLevelBeatenPrefix);
        var nextLevelNumber = PlayerPrefs.GetInt(Constants.currentLevel) + 1;
        if (nextLevelNumber > maxLevelBeaten)
        {
            maxLevelBeaten = nextLevelNumber;
            // todo move this to a data manager class
            PlayerPrefs.SetInt(Constants.maxLevelBeatenPrefix, maxLevelBeaten);
        }

        if (nextLevelNumber > GameVariables.totalAmountOfLevels)
        {
            PlaySound(SoundList.levelComplete);
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

    public static void ExitGame()
    {
        // todo change to broadcasting an exit ganme behaviour
        CoreConnector.LevelManager.UnLoadAllLevels();
        CoreConnector.GameManager.ballManager.ResetAllBalls();
        CoreConnector.GameUIManager.playerLifeDisplay.Hide();
        CoreConnector.GameManager.bonusManager.ResetFallingObjectsAvailable();
        CoreConnector.GameManager.fallingObjectsManager.HideAll();
        CoreConnector.GameManager.particleManager.ExitGame();
    }
}