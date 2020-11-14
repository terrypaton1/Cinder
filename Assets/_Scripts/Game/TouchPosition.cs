using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class TouchPosition : BaseObject {
	/// <summary>
	/// The camera reference.
	/// </summary>
	[SerializeField]
	Camera camera_ref;
	/// <summary>
	/// The user interface camera.
	/// </summary>
	[SerializeField] UICamera _uiCamera;
	/// <summary>
	/// The game is paused.
	/// </summary>
		bool gameIsPaused = false;
	/// <summary>
	/// The last touch position.
	/// </summary>
	Vector2 lastTouchPosition = Vector2.zero;

	void Touched(GameObject go, Vector2 _vector) {
		Debug.Log("Touched! " + _vector);
		lastTouchPosition = _vector;
	}

	//	void is
	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable() {
		Messenger.AddListener(GlobalEvents.PauseGame, PauseGame);
		Messenger.AddListener(GlobalEvents.ResumeGame, ResumeGame);
		//		UICamera.genericEventHandler = this.gameObject;
//		UICamera.onDrag   += Touched;

	}


	/// <summary>
	/// Remove listeners
	/// </summary>
	void OnDisable() {
		Messenger.RemoveListener(GlobalEvents.PauseGame, PauseGame);
		Messenger.RemoveListener(GlobalEvents.ResumeGame, ResumeGame);
	}

	/// <summary>
	/// Pauses the game.
	/// </summary>
	public void PauseGame() {
//		Debug.Log("*** game paused");
		gameIsPaused = true;

	}
	/// <summary>
	/// Resumes the game.
	/// </summary>
	void ResumeGame() {
		StartCoroutine(ResumeGameSequence());

	}
	/// <summary>
	/// Resumes the game sequence.
	/// </summary>
	/// <returns>The game sequence.</returns>
	IEnumerator ResumeGameSequence() {
		yield return new WaitForSeconds(.1f);
		gameIsPaused = false;
	}

	/// <summary>
	/// Gets the players position.
	/// </summary>
	/// <returns>The players position.</returns>
	public Vector2 GetPlayersPosition() {
	if (gameIsPaused) return lastTouchPosition;
		#if UNITY_EDITOR
		Vector3 p = camera_ref.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera_ref.nearClipPlane));
		lastTouchPosition.x = p.x;
		lastTouchPosition.y = p.y;
		return 	lastTouchPosition;
		#else
		if (Input.touchCount < 1)
			return lastTouchPosition;
		Touch _touch =	Input.GetTouch(0);
//		Debug.Log("pos:" + _touch.position);
//		Debug.Log("_touch.pos.x:" + _touch.position.x + " / Input.mousePosition.x:" + Input.mousePosition.x);
		Vector3 p = camera_ref.ScreenToWorldPoint(new Vector3(_touch.position.x, _touch.position.y, camera_ref.nearClipPlane));
		lastTouchPosition.x = p.x;
		lastTouchPosition.y = p.y;
		return 	lastTouchPosition;
		#endif
	}

	#region instance

	void OnDestroy() {	
		s_Instance = null;
	}
	// ************************************
	// s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
	// ************************************
	private static TouchPosition s_Instance = null;

	// ************************************
	// This defines a static instance property that attempts to find the manager object in the scene and
	// returns it to the caller.
	// ************************************
	public static TouchPosition instance {
		get {
			if (s_Instance == null) {
				// This is where the magic happens.
				//  FindObjectOfType(...) returns the first AManager object in the scene.
				s_Instance = FindObjectOfType(typeof(TouchPosition)) as TouchPosition;
			}

			// If it is still null, create a new instance
			if (s_Instance == null) {
				Debug.Log("Could not locate an TouchPosition object!");
//				UnityEngine.Debug.Break();
			}

			return s_Instance;
		}
	}

	// ************************************
	// Ensure that the instance is destroyed when the game is stopped in the editor.
	// ************************************
	void OnApplicationQuit() {
		s_Instance = null;
	}

	#endregion
}
