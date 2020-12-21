using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PowerupBrick))]
public class PowerupBrickEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var _powerup = (PowerupBrick) target;
        _powerup.typeOfPowerUp = (PowerupType) EditorGUILayout.EnumPopup("Type of Powerup:", _powerup.typeOfPowerUp);

        if (_powerup.typeOfPowerUp == PowerupType.Random)
        {
            GUILayout.Label("Random powerup - fill the array with your choices");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("randomPowerupChoices"), true);
        }

        base.OnInspectorGUI();

        if (!GUI.changed)
        {
            return;
        }

        _powerup.EvaluateDisplay();
        if (_powerup.typeOfPowerUp == PowerupType.Random)
        {
            if (_powerup.randomPowerupChoices.Length == 0)
            {
                Debug.LogError("ERROR: No random choices have been set");
            }
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
}