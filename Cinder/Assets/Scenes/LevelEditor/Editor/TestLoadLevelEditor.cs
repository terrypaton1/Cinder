using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class TestLoadLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("LOAD"))
        {
            var level = target as LevelManager;
            level.DisplayLevel(level.loadLevel);
        }

        if (GUILayout.Button("Unload"))
        {
            var level = target as LevelManager;
            level.UnLoadAllLevels();
        }
    }
}