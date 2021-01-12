using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField]
    protected Camera loadingCamera;

    [SerializeField]
    protected Canvas loadingScreen;

    private const string Main = "Main";
    private const string Game = "Game";
    private const string Levels_01_10 = "All_Levels";
    private int currentLoadingIndex;

    private readonly string[] loadSceneQueue = {Main, Game, Levels_01_10};
    private IEnumerator gameLevelCoroutine;

    private void OnEnable()
    {
        // start the process of loading the UI and then core game
        Init();
    }

    protected void Awake()
    {
        QualitySettings.vSyncCount = 0;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Init()
    {
        loadingScreen.enabled = true;
        loadingCamera.enabled = true;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadNextSceneInQueue();
        currentLoadingIndex++;
    }

    private void StartUpGame()
    {
        StartCoroutine(StartSequence());
    }

    private IEnumerator StartSequence()
    {
        yield return null;
        
        yield return CoreConnector.LevelsManager.CacheAllLevelsSequence();

        CoreConnector.GameManager.PerformInitialSetup();
        // show main menu ui

        CoreConnector.UIManager.DisplayScreen(UIScreens.MainMenu);

        loadingCamera.enabled = false;
        loadingScreen.enabled = false;
    }

    private void LoadNextSceneInQueue()
    {
        if (currentLoadingIndex >= loadSceneQueue.Length)
        {
            StartUpGame();
            return;
        }

        var nextSceneToLoad = loadSceneQueue[currentLoadingIndex];
        SceneManager.LoadScene(nextSceneToLoad, LoadSceneMode.Additive);
    }
}