using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PowerupBrick))]
public class PowerupBrickEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var powerUp = (PowerupBrick) target;
        powerUp.typeOfPowerUp = (PowerupType) EditorGUILayout.EnumPopup("Type of PowerUp:", powerUp.typeOfPowerUp);

        if (powerUp.typeOfPowerUp == PowerupType.Random)
        {
            GUILayout.Label("Random PowerUp - fill the array with your choices");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("randomPowerupChoices"), true);
        }

        base.OnInspectorGUI();

        if (!GUI.changed)
        {
            return;
        }

        powerUp.EvaluateDisplay();
        if (powerUp.typeOfPowerUp == PowerupType.Random)
        {
            if (powerUp.randomPowerupChoices.Length == 0)
            {
                Debug.LogError("ERROR: No random choices have been set");
            }
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
}