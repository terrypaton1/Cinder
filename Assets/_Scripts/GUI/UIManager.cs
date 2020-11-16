using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : BaseObject
{
    [SerializeField]
    protected UIWidget screenMainMenu;

    [SerializeField]
    protected UIWidget screenCredits;

    [SerializeField]
    protected UIWidget screenLevelchooser;

    [SerializeField]
    protected UIWidget levelLoader;

    [SerializeField]
    protected UIWidget initialLoadScreen;

    [SerializeField]
    protected UIWidget SignoutOfSocialDialogHolder;

    private UIScreens currentScreen;

    protected void Start()
    {
        Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.InitialLoad);
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
                    DisplayUIScreen(UIScreens.MainMenu);
                    break;
                case UIScreens.Game:
                    // we are currently in game. if the game isn't paused, then show the pause menu

                    Messenger.Broadcast(GlobalEvents.PauseGame);
                    break;
                case UIScreens.LevelChooser:
                    DisplayUIScreen(UIScreens.MainMenu);
                    break;
            }
        }
    }

    protected void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        Messenger<UIScreens>.AddListener(GlobalEvents.DisplayUIScreen, DisplayUIScreen);
        Messenger<bool>.AddListener(MenuEvents.DisplayLevelLoader, DisplayLevelLoader);
    }

    protected void OnDisable()
    {
        Messenger<UIScreens>.RemoveListener(GlobalEvents.DisplayUIScreen, DisplayUIScreen);
        Messenger<bool>.RemoveListener(MenuEvents.DisplayLevelLoader, DisplayLevelLoader);
    }

    private void DisplayLevelLoader(bool display)
    {
        levelLoader.gameObject.SetActive(display);
    }

    private void unloadGameIfOneIsLoaded()
    {
//		Debug.Log("unloadGameIfOneIsLoaded");

        var _scene = SceneManager.GetSceneByName("Game");
        if (_scene.IsValid())
        {
            Debug.Log("_scene:" + _scene.name);
            SceneManager.UnloadSceneAsync(_scene);
        }
    }

    private void gotoMainScene()
    {
        var _scene = SceneManager.GetActiveScene();
        if (_scene.name != "Main")
        {
            Debug.Log("load the main scene");
//			SceneManager.LoadScene("Main");
            screenMainMenu.gameObject.SetActive(true);
            screenCredits.gameObject.SetActive(false);
            screenLevelchooser.gameObject.SetActive(false);
            initialLoadScreen.gameObject.SetActive(false);
        }

        // unload all levels
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var name = SceneManager.GetSceneAt(i).name;
            if (name.Contains(DataVariables.levelNamePrefix))
            {
                SceneManager.UnloadScene(name);
            }
        }

        // check if scene is unloaded then unload it
        var tempScene = SceneManager.GetSceneByName("Game");
        if (tempScene != null)
        {
//			SceneManager.UnloadSceneAsync ("Game");
//		SceneManager.UnloadScene("Game");
        }
    }

    private void DisplayUIScreen(UIScreens displayScreenID)
    {
//		Debug.Log("DisplayUIScreen:" + displayScreenID);
        Messenger<string, string>.Broadcast(GlobalEvents.AnalyticsEvent, BrickAnalytics.changeScene,
            displayScreenID.ToString());
        switch (displayScreenID)
        {
            case UIScreens.MainMenu:
                gotoMainScene();
                HideAllScreens();
                screenMainMenu.gameObject.SetActive(true);
                unloadGameIfOneIsLoaded();
                DisplayLevelLoader(false);
                break;
            case UIScreens.Game:
                StartCoroutine(goGame());
                break;
            case UIScreens.Credits:
                gotoMainScene();
                HideAllScreens();
                screenCredits.gameObject.SetActive(true);
                unloadGameIfOneIsLoaded();
                break;
            case UIScreens.LevelChooser:
                gotoMainScene();
                HideAllScreens();
                screenLevelchooser.gameObject.SetActive(true);
                unloadGameIfOneIsLoaded();
                DisplayLevelLoader(false);
                break;
            case UIScreens.InitialLoad:
                gotoMainScene();
                HideAllScreens();
                initialLoadScreen.gameObject.SetActive(true);
                break;
            case UIScreens.ConfirmLogOut:
                HideAllScreens();
                SignoutOfSocialDialogHolder.gameObject.SetActive(true);
                break;
        }

        currentScreen = displayScreenID;
    }

    private void HideAllScreens()
    {
        screenMainMenu.gameObject.SetActive(false);
        screenCredits.gameObject.SetActive(false);
        screenLevelchooser.gameObject.SetActive(false);
        initialLoadScreen.gameObject.SetActive(false);
        SignoutOfSocialDialogHolder.gameObject.SetActive(false);
    }

    private IEnumerator goGame()
    {
        screenMainMenu.gameObject.SetActive(false);
        screenCredits.gameObject.SetActive(false);
        screenLevelchooser.gameObject.SetActive(false);
        initialLoadScreen.gameObject.SetActive(false);

        DisplayLevelLoader(true);
        yield return new WaitForSeconds(.2f);
        // load in the game scene.
        var async = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        yield return async;
//		Debug.Log("Loading Game_scene complete");
    }
}