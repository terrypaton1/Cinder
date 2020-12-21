using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelsManager))]
[CanEditMultipleObjects]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("ALL LEVELS ON"))
        {
            var classRef = target as LevelsManager;
            ShowAllRenderers(classRef);
        }

        if (GUILayout.Button("ALL LEVELS OFF"))
        {
            var classRef = target as LevelsManager;
            HideAllRenderers(classRef);
        }

        base.OnInspectorGUI();
    }

    private void HideAllRenderers(LevelsManager classRef)
    {
        foreach (var brick in classRef.allLevels)
        {
            brick.Hide();
        }
    }

    private void ShowAllRenderers(LevelsManager classRef)
    {
        foreach (var brick in classRef.allLevels)
        {
            brick.Show();
        }
    }
}