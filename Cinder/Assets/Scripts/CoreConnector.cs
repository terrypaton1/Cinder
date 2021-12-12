using UnityEngine;
using UnityEngine.Assertions;

public class CoreConnector : MonoBehaviour
{
    private static GameManager gameManager;
    private static GameUIManager gameUIManager;
    private static UIManager uiManager;
    private static GameSoundManager soundManager;
    private static LevelManager _levelManager;

    public static GameManager GameManager
    {
        get
        {
            Assert.IsNotNull(gameManager, "Error: gameManager reference is null");
            return gameManager;
        }
        set => gameManager = value;
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

    public static GameSoundManager SoundManager
    {
        get
        {
            Assert.IsNotNull(soundManager, "Error: soundManager reference is null");
            return soundManager;
        }
        set => soundManager = value;
    }

    public static LevelManager LevelManager
    {
        get
        {
            Assert.IsNotNull(_levelManager, "Error: testLoadLevel reference is null");
            return _levelManager;
        }
        set => _levelManager = value;
    }
}