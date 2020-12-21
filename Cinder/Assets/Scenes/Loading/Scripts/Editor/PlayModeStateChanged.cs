using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[InitializeOnLoadAttribute]
public static class PlayModeStateChanged
{
    static PlayModeStateChanged()
    {
        EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    private static void LogPlayModeState(PlayModeStateChange state)
    {
        Debug.Log(state);
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            Debug.Log("Auto loading loading scene");
            SceneManager.LoadScene("Loading");
        }
    }
}