using UnityEngine;
using System.Collections;

public class Blocker : MonoBehaviour {
/// <summary>
/// Awake this instance.
/// </summary>
	void Awake() {
		#if UNITY_EDITOR
		Debug.Log("using blocker to stop ball from going out");
		#else
	Destroy(gameObject);
		#endif
	}
}
