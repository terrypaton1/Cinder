using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// this class is for debugging. It tests if the game has the game scene loaded, if not then it will try and load it
public class GameRunner : MonoBehaviour
{
    [SerializeField]
    protected Camera _camera;

    protected void Awake()
    {
        if (BrickManager.instance == null)
        {
            // load up the game scene
            StartCoroutine(LoadGameScene());
        }

        _camera.gameObject.SetActive(false);
    }

    private IEnumerator LoadGameScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        yield return async;
        Debug.Log("Loading Game scene complete");
        Messenger<bool>.Broadcast(MenuEvents.DisplayInGameButtons, true, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    protected void OnDestroy()
    {
        s_Instance = null;
    }

    private static GameRunner s_Instance;

    public static GameRunner instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(GameRunner)) as GameRunner;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
#if UNITY_EDITOR
                Debug.Log("Could not locate a GameRunner object!");
#endif
//				Debug.Break();
            }

            return s_Instance;
        }
    }

    protected void OnApplicationQuit()
    {
        s_Instance = null;
    }
}