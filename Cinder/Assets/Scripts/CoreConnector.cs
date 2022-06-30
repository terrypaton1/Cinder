using UnityEngine;
using UnityEngine.Assertions;

public class CoreConnector : MonoBehaviour
{
    private static GameManager _gameManager;
    private static GameUIManager _gameUIManager;
    private static UIManager _uiManager;
    private static GameSoundManager _soundManager;
    private static LevelManager _levelManager;

    public static GameManager GameManager
    {
        get
        {
            Assert.IsNotNull(_gameManager, "Error: gameManager reference is null");
            return _gameManager;
        }
        set => _gameManager = value;
    }


    public static UIManager UIManager
    {
        get
        {
            Assert.IsNotNull(_uiManager, "Error: uiManager reference is null");
            return _uiManager;
        }
        set => _uiManager = value;
    }

    public static GameUIManager GameUIManager
    {
        get
        {
            //Assert.IsNotNull(_gameUIManager, "Error: gameUIManager reference is null");
            return _gameUIManager;
        }
        set => _gameUIManager = value;
    }

    public static GameSoundManager SoundManager
    {
        get
        {
            Assert.IsNotNull(_soundManager, "Error: soundManager reference is null");
            return _soundManager;
        }
        set => _soundManager = value;
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