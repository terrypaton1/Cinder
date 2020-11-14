using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// this class is for debugging. It tests if the game has the game scene loaded, if not then it will try and load it
public class GameRunner : MonoBehaviour {
	/// <summary>
	/// The camera.
	/// </summary>
	[SerializeField] Camera _camera;


	void Awake() {
		if (BrickManager.instance == null) {
			// load up the game scene
			StartCoroutine(LoadGameScene());
		}
		_camera.gameObject.SetActive(false);
	}

	/// <summary>
	/// Loads the level.
	/// </summary>
	/// <returns>The level.</returns>
	IEnumerator LoadGameScene() {
		AsyncOperation async = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
		yield return async;
		Debug.Log("Loading Game scene complete");
		Messenger<bool>.Broadcast(MenuEvents.DisplayInGameButtons, true, MessengerMode.DONT_REQUIRE_LISTENER);
	}

	#region instance

	void OnDestroy() {	
		s_Instance = null;
	}
	// ************************************
	// s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
	// ************************************
	private static GameRunner s_Instance ;

	// ************************************
	// This defines a static instance property that attempts to find the manager object in the scene and
	// returns it to the caller.
	// ************************************
	public static GameRunner instance {
		get {
			if (s_Instance == null) {
				// This is where the magic happens.
				//  FindObjectOfType(...) returns the first AManager object in the scene.
				s_Instance = FindObjectOfType(typeof(GameRunner)) as GameRunner;
			}

			// If it is still null, create a new instance
			if (s_Instance == null) {
				#if UNITY_EDITOR
				Debug.Log("Could not locate a GameRunner object!");
				#endif
//				Debug.Break();
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
