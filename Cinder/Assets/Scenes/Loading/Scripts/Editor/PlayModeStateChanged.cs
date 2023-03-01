#region

using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

[InitializeOnLoadAttribute]
public static class PlayModeStateChanged
{
    private const string LoadingScene = "Loading";

    static PlayModeStateChanged()
    {
        Debug.Log("Auto play");
        EditorApplication.playModeStateChanged += PlayModeState;
    }

    private static void PlayModeState(PlayModeStateChange state)
    {
        if (state != PlayModeStateChange.EnteredPlayMode)
        {
            return;
        }

        SceneManager.LoadScene(LoadingScene);
    }
}