using UnityEngine;
using System.Collections;

/// <summary>
/// in Game messages.
/// </summary>
public class BossHealthRemainingDisplay : MonoBehaviour {
	/// <summary>
	/// The user interface widget.
	/// </summary>
	public UIWidget _uiWidget;
	/// <summary>
	/// The message box.
	/// </summary>
	[SerializeField] GameObject _messageBox;
	/// <summary>
	/// The progress bar.
	/// </summary>
	[SerializeField] UISprite _progressBar;

	/// <summary>
	/// The message animation.
	/// </summary>
	Animator messageAnimation;

	void Awake() {
		messageAnimation = GetComponent<Animator>();
		_messageBox.SetActive(false);
	}

	public void DisplayPercent(float percent) {
		_progressBar.transform.localScale = new Vector3(percent, 1, 1);
	}

	/// <summary>
	/// Displaies the in game message.
	/// </summary>
	/// <param name="_message">Message.</param>
	void DisplayBossHealthBar() {

		_messageBox.SetActive(true);
		messageAnimation.Play("BossHealthRemainingShow");
	}

	/// <summary>
	/// Hides the in game message instantly.
	/// </summary>
	void HideBossHealthInstantly() {
		StopAllCoroutines();
		messageAnimation.Play("BossHealthRemainingShow");
	}

	/// <summary>
	/// Hides the in game message.
	/// </summary>
	void HideBossHealthBar() {
		StopAllCoroutines();
		StartCoroutine(HideInGameMessageSequence());
	}

	/// <summary>
	/// Hides the in game message sequence.
	/// </summary>
	IEnumerator HideInGameMessageSequence() {
		messageAnimation.Play("BossHealthRemainingHide");
		yield return new WaitForSeconds(1f);
		_messageBox.SetActive(false);
	}

	/// <summary>
	/// The player loses a life
	/// </summary>
	void LifeLost() {
		StopAllCoroutines();
		messageAnimation.Play("BossHealthRemainingHide");
	}

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable() {

		Messenger.AddListener(GlobalEvents.DisplayBossHealthBar, DisplayBossHealthBar);
		Messenger.AddListener(GlobalEvents.HideBossHealthInstantly, HideBossHealthInstantly);
		Messenger.AddListener(GlobalEvents.HideBossHealthBar, HideBossHealthBar);
		Messenger.AddListener(GlobalEvents.LifeLost, LifeLost);
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable() {
		Messenger.RemoveListener(GlobalEvents.DisplayBossHealthBar, DisplayBossHealthBar);
		Messenger.RemoveListener(GlobalEvents.HideBossHealthInstantly, HideBossHealthInstantly);
		Messenger.RemoveListener(GlobalEvents.HideBossHealthBar, HideBossHealthBar);
		Messenger.RemoveListener(GlobalEvents.LifeLost, LifeLost);
	}
	#region instance

	void OnDestroy() {	
		s_Instance = null;
	}
	// ************************************
	// s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
	// ************************************
	private static BossHealthRemainingDisplay s_Instance = null;

	// ************************************
	// This defines a static instance property that attempts to find the manager object in the scene and
	// returns it to the caller.
	// ************************************
	public static BossHealthRemainingDisplay instance {
		get {
			if (s_Instance == null) {
				// This is where the magic happens.
				//  FindObjectOfType(...) returns the first AManager object in the scene.
				s_Instance = FindObjectOfType(typeof(BossHealthRemainingDisplay)) as BossHealthRemainingDisplay;
			}
			// If it is still null, create a new instance
			if (s_Instance == null) {
				Debug.Log("Could not locate an BossHealthRemainingDisplay object!");
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
