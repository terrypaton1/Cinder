using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class UIManager : BaseObject
{
    [Header("Screen references")]
    [SerializeField]
    protected UIScreen mainMenu;

    [SerializeField]
    protected UIScreen levelChooser;

    [SerializeField]
    protected UIScreen credits;

    [SerializeField]
    protected UIScreen loadingScreen;

    [SerializeField]
    protected UIScreen levelComplete;

    [SerializeField]
    protected UIScreen gameComplete;

    [SerializeField]
    protected UIScreen gameUI;

    [SerializeField]
    protected UIScreen pauseGame;

    [SerializeField]
    protected UIScreen gameOver;

    private Dictionary<UIScreens, UIScreen> screens;

    private UIScreens currentScreen;

    protected void OnEnable()
    {
        CoreConnector.UIManager = this;

        screens = new Dictionary<UIScreens, UIScreen>();
        screens.Add(UIScreens.LevelChooser, levelChooser);
        screens.Add(UIScreens.MainMenu, mainMenu);
        screens.Add(UIScreens.Credits, credits);
        screens.Add(UIScreens.LoadingScreen, loadingScreen);
        screens.Add(UIScreens.LevelComplete, levelComplete);
        screens.Add(UIScreens.GameComplete, gameComplete);
        screens.Add(UIScreens.Game, gameUI);
        screens.Add(UIScreens.GameOver, gameOver);
        screens.Add(UIScreens.PauseGame, pauseGame);
    }

    UIScreen GetScreen(UIScreens type)
    {
        screens.TryGetValue(type, out var theOut);

        if (theOut is null)
        {
            Debug.Log("Could find screen:" + type);
            return null;
        }

        return theOut;
    }


    private void DisplayUIScreen(UIScreens displayScreenID)
    {
        switch (displayScreenID)
        {
            case UIScreens.LevelChooser:
                ShowLevelChooser();
                CoreConnector.GameManager.ExitGame();
                break;
        }

        currentScreen = displayScreenID;
    }

    private void InitialLoad()
    {
        ShowMainMenu();
    }

    public void ShowGamePlayUI()
    {
        HideAllScreens();
        var screen = GetScreen(UIScreens.Game);
        screen.Show();
    }

    public void PressMainMenuPlayButton()
    {
        // todo if the player is on the first level, take them straight to the game, don't go to the level chooser
        DisplayUIScreen(UIScreens.LevelChooser);
    }

    public void PlayGame()
    {
        StartCoroutine(goGame());
    }

    private IEnumerator goGame()
    {
        HideAllScreens();

        DisplayLevelLoader();

        yield return new WaitForSeconds(0.2f);

        // todo change this, because all UI will be loaded before playing
        // load in the game scene.
        var async = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        yield return async;

        ShowGamePlayUI();
    }

    public void PressCreditsButton()
    {
        ShowCredits();
    }

    public void PressQuitButton()
    {
#if !UNITY_EDITOR
				Application.Quit();
#endif
    }

    public void PressedResumeButton()
    {
        CoreConnector.GameManager.ResumeGame();
    }

    public void PressedMainMenuButton()
    {
        ShowMainMenu();
        CoreConnector.GameManager.ExitGame();
    }


    public void HideAllScreens()
    {
        foreach (var valuePair in screens)
        {
            var screen = valuePair.Value;
            screen.Hide();
        }
    }

    private void ShowMainMenu()
    {
        HideAllScreens();
        var screen = GetScreen(UIScreens.MainMenu);
        screen.Show();
    }

    private void ShowLevelChooser()
    {
        // shut dow a game if one is active
        // todo ensure any current running games are shutfown

        HideAllScreens();
        var screen = GetScreen(UIScreens.LevelChooser);
        screen.Show();
    }

    private void ShowCredits()
    {
        HideAllScreens();
        var screen = GetScreen(UIScreens.Credits);
        screen.Show();
    }

    protected void Start()
    {
        InitialLoad();
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // depending on which screen we are on, we need to go back
            switch (currentScreen)
            {
                case UIScreens.MainMenu:

                    break;
                case UIScreens.Credits:
                    ShowMainMenu();
                    break;
                case UIScreens.Game:
                    // we are currently in game. if the game isn't paused, then show the pause menu
                    //   Messenger.Broadcast(GlobalEvents.PauseGame);
                    break;
                case UIScreens.LevelChooser:
                    ShowMainMenu();
                    break;
            }
        }
    }

    public void GoNextLevel()
    {
        // this seems to be the UI button press that triggers the next level
        CoreConnector.GameManager.NextLevel();
    }

    public void GoLevelChooser()
    {
        ShowLevelChooser();
        CoreConnector.GameManager.ExitGame();
    }

    public void DisplayLevelLoader()
    {
        HideAllScreens();
        var screen = GetScreen(UIScreens.LoadingScreen);
        screen.Show();
    }

    public void PressedPauseButton()
    {
        CoreConnector.GameManager.PauseGame();
        HideAllScreens();
        var screen = GetScreen(UIScreens.PauseGame);
        screen.Show();
    }
    public void DisplayScreen(UIScreens screenID)
    {
        HideAllScreens();
        var screen = GetScreen(screenID);
        screen.Show();
    }

    public void LoadLevel(int levelNumber)
    {
        PlayerPrefs.SetInt(DataVariables.currentLevel, levelNumber);
        CoreConnector.GameManager.StartGame();
    }
}