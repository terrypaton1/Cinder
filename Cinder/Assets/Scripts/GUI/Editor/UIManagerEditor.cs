using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIManager))]
public class UIManagerEditor : Editor
{
    float btnHeight = 50;
    float btnPadding = 10;
    private float btnY;
    private List<UIScreen> scenes;
    UIScreen[] allScenes;

    [DrawGizmo(GizmoType.NotSelected)]
    static void RenderCustomGizmo(Transform objectTransform, GizmoType gizmoType)
    {
        //Draw here
        Debug.Log(1);
    }

    private void OnEnable()
    {
        SceneView.onSceneGUIDelegate += DrawSceneInspector;
        if (SceneView.lastActiveSceneView)
        {
            SceneView.lastActiveSceneView.Repaint();
        }
    }

    private void OnDisable()
    {
        // if you don't do this Unity throws a hissy fit when you select another object
        SceneView.onSceneGUIDelegate -= DrawSceneInspector;
    }


    private void DrawSceneInspector(SceneView scene)
    {
        Handles.BeginGUI();
        DisplayScenes();
        Handles.EndGUI();
    }

    private void PressedScene(int index)
    {
        HideAllScenes();
        var scene = allScenes[index];
        scene.Show();
    }

    private void DisplayScenes()
    {
        var uiManager = target as UIManager;

        if (allScenes == null)
        {
            allScenes = uiManager.gameObject.GetComponentsInChildren<UIScreen>();
        }

        var counter = 0;
        btnY = 10;
        foreach (var scene in allScenes)
        {
            var buttonText = scene.name;
            if (GUI.Button(new Rect(10, btnY, 130, btnHeight - 5), buttonText))
            {
                PressedScene(counter);
            }

            btnY += btnHeight;
            counter++;
        }
    }

    private void HideAllScenes()
    {
        foreach (var scene in allScenes)
        {
            scene.Hide();
        }
    }
}