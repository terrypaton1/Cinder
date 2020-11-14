#region

using UnityEditor;
using UnityEngine;

#endregion

[CustomEditor(typeof(Brick))][CanEditMultipleObjects]
public class BrickEditor : Editor {
	void Awake() {

		BrickBase myTarget = (Brick)target;
		myTarget.UpdateAmountOfHitsLeftDisplay();
	}

	public override void OnInspectorGUI() {

		base.OnInspectorGUI();
		if (GUI.changed) {
			BrickBase myTarget = (Brick)target;
			myTarget.UpdateAmountOfHitsLeftDisplay();
		}
	}
}