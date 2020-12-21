using UnityEngine;
using UnityEngine.Assertions;

public class CoreConnector : MonoBehaviour
{
    /*[SerializeField]
    protected GameSettings gameSettings;

    public ref GameSettings GetGameSettings()
    {
        return ref gameSettings;
    }*/
    private static GameMessages gameMessages;
    private static CoreConnector instance;
    private static GameManager gameManager;
    private static GameSettings gameSettings;
    private static GameUIManager gameUIManager;
    private static UIManager uiManager;
    private static ParticleManager particleManager;
    private static GameSoundManager soundManager;
    private static LevelsManager levelsManager;
    
    public static CoreConnector Instance
    {
        get { return instance; }
    }

    public static GameManager GameManager
    {
        get
        {
            Assert.IsNotNull(gameManager, "Error: gameManager reference is null");
            return gameManager;
        }
        set => gameManager = value;
    }

    public static GameSettings GameSettings
    {
        get
        {
            Assert.IsNotNull(gameSettings, "Error: gameSettings reference is null");
            return gameSettings;
        }
        set => gameSettings = value;
    }

    public static UIManager UIManager
    {
        get
        {
            Assert.IsNotNull(uiManager, "Error: uiManager reference is null");
            return uiManager;
        }
        set => uiManager = value;
    }

    public static GameUIManager GameUIManager
    {
        get
        {
            Assert.IsNotNull(gameUIManager, "Error: gameUIManager reference is null");
            return gameUIManager;
        }
        set => gameUIManager = value;
    }

    public static ParticleManager ParticleManager
    {
        get
        {
            Assert.IsNotNull(particleManager, "Error: particleManager reference is null");
            return particleManager;
        }
        set => particleManager = value;
    }

    public static GameSoundManager SoundManager
    {
        get
        {
            Assert.IsNotNull(soundManager, "Error: soundManager reference is null");
            return soundManager;
        }
        set => soundManager = value;
    }

    public static LevelsManager LevelsManager
    {
        get
        {
            Assert.IsNotNull(levelsManager, "Error: levelsManager reference is null");
            return levelsManager;
        }
        set => levelsManager = value;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            //Debug.Log("Core Connector already found");
            return;
        }

        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnDisable()
    {
        instance = null;
    }

    public static bool IsLevelsLoaded
    {
        get { return levelsIsLoaded; }
        set => levelsIsLoaded = value;
    }

    private static bool levelsIsLoaded;
}