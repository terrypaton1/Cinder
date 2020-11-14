#region

using UnityEditor;
using UnityEngine;

#endregion

[CustomEditor(typeof(PowerupBrick))]
public class PowerupBrickEditor : Editor {
	/// <summary>
	///  show The base inspector prefab slots. Hiding these by default to make things cleaner when editing levels
	/// </summary>
	bool showPrefabSlots;

	/// <summary>
	/// Raises the inspector GU event.
	/// </summary>
	public override void OnInspectorGUI() {

		var _powerup = (PowerupBrick)target;
		_powerup.typeOfPowerUp = (PowerupType)EditorGUILayout.EnumPopup("Type of Powerup:", _powerup.typeOfPowerUp);
		if (_powerup.typeOfPowerUp == PowerupType.Random) {
			GUILayout.Label("Random powerup - fill the array with your choices");
			EditorGUILayout.PropertyField(serializedObject.FindProperty("randomPowerupChoices"), true);
//			serializedObject.ApplyModifiedProperties();
		}
		showPrefabSlots = GUILayout.Toggle(showPrefabSlots, "Show base inspector attributes");
		if (showPrefabSlots) {
			base.OnInspectorGUI();
		}
		if (GUI.changed) {
			_powerup.EvaluateDisplay();
			if (_powerup.typeOfPowerUp == PowerupType.Random) {
				if (_powerup.randomPowerupChoices.Length == 0) {
					Debug.LogError("ERROR: No random choices have been set");
				}
			}
			serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(target);
		}
	}
}