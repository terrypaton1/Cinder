using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level))]
[CanEditMultipleObjects]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("ON"))
        {
            var classRef = target as Level;
            ShowAllRenderers(classRef);
        }

        if (GUILayout.Button("OFF"))
        {
            var classRef = target as Level;
            HideAllRenderers(classRef);
        }

        base.OnInspectorGUI();
    }

    private void HideAllRenderers(Level classRef)
    {
        foreach (var brick in classRef.bricks)
        {
            brick.Hide();
        }
    }

    private void ShowAllRenderers(Level classRef)
    {
        foreach (var brick in classRef.bricks)
        {
            brick.EnableVisuals();
        }
    }
}