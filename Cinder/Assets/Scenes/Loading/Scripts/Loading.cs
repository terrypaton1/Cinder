using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField]
    protected Camera loadingCamera;

    private const string Main = "Main";
    private const string Game = "Game";
    private const string Levels_01_10 = "All_Levels";
    private int currentLoadingIndex;
    private const int targetFPS = 60;

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
        Application.targetFrameRate = targetFPS;
    }

    protected void Update()
    {
        if (Application.targetFrameRate == targetFPS)
        {
            return;
        }

        Debug.Log("setting frame rate to " + targetFPS);
        Application.targetFrameRate = targetFPS;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Init()
    {
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
        StartCoroutine(StartUpGameSequence());
    }

    private IEnumerator StartUpGameSequence()
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Game is fully loaded");
        loadingCamera.enabled = false;
        CoreConnector.GameManager.PerformInitialSetup();
    }

    private void LoadNextSceneInQueue()
    {
        if (currentLoadingIndex >= loadSceneQueue.Length)
        {
            //Debug.Log("All levels loaded");
            StartUpGame();
            return;
        }

        var nextSceneToLoad = loadSceneQueue[currentLoadingIndex];
        SceneManager.LoadScene(nextSceneToLoad, LoadSceneMode.Additive);
    }
}