#region

using UnityEditor;
using UnityEngine.SceneManagement;

#endregion

[InitializeOnLoadAttribute]
public static class PlayModeStateChanged
{
    private const string LoadingScene = "Loading";

    static PlayModeStateChanged()
    {
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