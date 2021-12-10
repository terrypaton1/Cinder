using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ExportLevel))]
public class ExportLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Output"))
        {
            var exportLevel = target as ExportLevel;
            exportLevel.OutputLevelContents();
        }
    }
}