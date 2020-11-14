#region

using UnityEditor;

#endregion

[CustomEditor(typeof(TNTBrick))]
public class TNTEditor : Editor {
	public override void OnInspectorGUI() {
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("visualObjects"), true);
		EditorGUILayout.LabelField("TNT block, range is set in GameVariables");
		base.OnInspectorGUI();
	}
}