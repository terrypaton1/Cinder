using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class TestLoadLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var levelManager = target as LevelManager;
        if (levelManager == null)
        {
            return;
        }

        if (GUILayout.Button("LOAD"))
        {
            levelManager.DisplayLevel(levelManager.loadLevel);
        }

        if (GUILayout.Button("Unload"))
        {
            levelManager.UnLoadAllLevels();
        }
    }
}