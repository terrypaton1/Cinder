using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Brick)), CanEditMultipleObjects]
public class BrickEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!GUI.changed)
        {
            return;
        }

        BrickBase myTarget = (Brick) target;
        myTarget.UpdateHitsRemainingDisplay();
    }

    protected void Awake()
    {
        BrickBase myTarget = (Brick) target;
        myTarget.UpdateHitsRemainingDisplay();
    }
}