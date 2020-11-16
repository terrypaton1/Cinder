using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    protected UIWidget buttonsHolder;

    [SerializeField]
    protected UIWidget pauseMenuHolder;

    [SerializeField]
    protected UIWidget GameOverMenuHolder;

    [SerializeField]
    protected UIWidget LevelCompleteMenuHolder;

    [SerializeField]
    protected UIWidget GameCompleteMenuHolder;

    [SerializeField]
    protected Animator inGameMenuButtonsAnimator;

    protected void Awake()
    {
        HideAllMenus();
    }

    protected void OnEnable()
    {
//		DontDestroyOnLoad(gameObject);
        Messenger<bool>.AddListener(MenuEvents.DisplayInGameButtons, DisplayInGameButtons);
        Messenger<bool>.AddListener(MenuEvents.DisplayPauseMenu, DisplayPauseMenu);
        Messenger<bool>.AddListener(MenuEvents.DisplayLevelComplete, DisplayLevelComplete);
        Messenger<bool>.AddListener(MenuEvents.DisplayGameComplete, DisplayGameComplete);
        Messenger<bool>.AddListener(MenuEvents.DisplayGameOver, DisplayGameOver);
        Messenger.AddListener(MenuEvents.RestartGame, RestartGame);
        Messenger.AddListener(GlobalEvents.PauseGame, PauseGame);
        Messenger.AddListener(GlobalEvents.ResumeGame, ResumeGame);
        Messenger.AddListener(GlobalEvents.HideAllMenus, HideAllMenus);
        Messenger<bool>.AddListener(GlobalEvents.LoginResult, LoginResult);
    }

    protected void OnDisable()
    {
        Messenger<bool>.RemoveListener(MenuEvents.DisplayInGameButtons, DisplayInGameButtons);
        Messenger<bool>.RemoveListener(MenuEvents.DisplayPauseMenu, DisplayPauseMenu);
        Messenger<bool>.RemoveListener(MenuEvents.DisplayLevelComplete, DisplayLevelComplete);
        Messenger<bool>.RemoveListener(MenuEvents.DisplayGameComplete, DisplayGameComplete);
        Messenger<bool>.RemoveListener(MenuEvents.DisplayGameOver, DisplayGameOver);
        Messenger.RemoveListener(MenuEvents.RestartGame, RestartGame);
        Messenger.RemoveListener(GlobalEvents.PauseGame, PauseGame);
        Messenger.RemoveListener(GlobalEvents.ResumeGame, ResumeGame);
        Messenger.RemoveListener(GlobalEvents.HideAllMenus, HideAllMenus);
        Messenger<bool>.RemoveListener(GlobalEvents.LoginResult, LoginResult);
    }

    private void HideAllMenus()
    {
        LevelCompleteMenuHolder.gameObject.SetActive(false);
        buttonsHolder.gameObject.SetActive(false);
        pauseMenuHolder.gameObject.SetActive(false);
        GameOverMenuHolder.gameObject.SetActive(false);
        GameCompleteMenuHolder.gameObject.SetActive(false);
    }

    private void DisplayGameComplete(bool display)
    {
        HideAllMenus();
        if (display)
        {
            GameCompleteMenuHolder.gameObject.SetActive(true);
        }
    }

    private void LoginResult(bool success)
    {
        if (success)
        {
            // save game has been loaded, update any details we need to.
        }
    }

    private void PauseGame()
    {
        DisplayPauseMenu(true);
    }

    private void ResumeGame()
    {
        DisplayInGameButtons(true);
    }

    void RestartGame()
    {
#if UNITY_EDITOR
        Debug.Log("TODO: Restart level");
#endif
//		StartCoroutine(LoadLevel());
    }

    private void DisplayLevelComplete(bool display)
    {
        HideAllMenus();
        if (display)
        {
            LevelCompleteMenuHolder.gameObject.SetActive(true);
        }
    }

    private void DisplayGameOver(bool display)
    {
        HideAllMenus();
        if (display)
        {
            GameOverMenuHolder.gameObject.SetActive(true);
        }
    }

    private void DisplayInGameButtons(bool displayButton)
    {
        HideAllMenus();
//		if (pause_button==null)return;
        if (displayButton)
        {
            buttonsHolder.gameObject.SetActive(true);
            inGameMenuButtonsAnimator.Play("AnimateInGameMenuButtonsIn");
        }
    }

    private void DisplayPauseMenu(bool display)
    {
        HideAllMenus();
        if (display)
        {
            pauseMenuHolder.gameObject.SetActive(true);
        }
    }
}