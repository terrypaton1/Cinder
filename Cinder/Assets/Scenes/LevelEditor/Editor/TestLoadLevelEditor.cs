using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestLoadLevel))]
public class TestLoadLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("LOAD"))
        {
            var exportLevel = target as TestLoadLevel;
            exportLevel.UnLoadAllLevels();
            exportLevel.DisplayLevel(exportLevel.loadLevel);
        }

        if (GUILayout.Button("Unload"))
        {
            var exportLevel = target as TestLoadLevel;
            exportLevel.UnLoadAllLevels();
        }
    }
}