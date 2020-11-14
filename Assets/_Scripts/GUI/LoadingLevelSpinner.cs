#region

using UnityEngine;

#endregion

public class LoadingLevelSpinner : MonoBehaviour {
	readonly Vector3 rotateSpeed = new Vector3(0, 0, -540);

	// Update is called once per frame
	void Update() {
		transform.Rotate(rotateSpeed*Time.deltaTime);
	}
}
