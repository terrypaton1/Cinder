using UnityEngine;
using System.Collections;

/// <summary>
/// in Game messages.
/// </summary>
public class GameMessages : MonoBehaviour {
	/// <summary>
	/// The user interface widget.
	/// </summary>
	public UIWidget _uiWidget;
	/// <summary>
	/// The message text.
	/// </summary>
	[SerializeField] UILabel _messageText;
	/// <summary>
	/// The message box.
	/// </summary>
	[SerializeField] GameObject _messageBox;
	/// <summary>
	/// The message animation.
	/// </summary>
	Animator messageAnimation;

	void Awake() {
		messageAnimation = GetComponent<Animator>();
		_messageBox.SetActive(false);
	}

	/// <summary>
	/// Displaies the in game message.
	/// </summary>
	/// <param name="_message">Message.</param>
	void DisplayInGameMessage(string _message) {
		_messageText.text = _message;
		StopCoroutine(ShowMessageSequence());
		StartCoroutine(ShowMessageSequence());
	}

	/// <summary>
	/// Shows the message sequence.
	/// </summary>
	/// <returns>The message sequence.</returns>
	IEnumerator ShowMessageSequence() {
		_messageBox.SetActive(true);
//		Debug.Log("ShowMessage");
		messageAnimation.Play("ShowMessage");
		yield return new WaitForSeconds(3f);
//		Debug.Log("HideMessage");
		messageAnimation.Play("HideMessage");
	}

	/// <summary>
	/// Hides the in game message instantly.
	/// </summary>
	void HideInGameMessageInstantly() {
		StopAllCoroutines();
		messageAnimation.Play("HideMessage");
	}

	/// <summary>
	/// Hides the in game message.
	/// </summary>
	void HideInGameMessage() {
		StopAllCoroutines();
		StartCoroutine(HideInGameMessageSequence());
	}
	/// <summary>
	/// Hides the in game message sequence.
	/// </summary>
	IEnumerator HideInGameMessageSequence() {
		messageAnimation.Play("HideMessage");
		yield return new WaitForSeconds(1f);
		_messageBox.SetActive(false);
	}
	/// <summary>
	/// The player loses a life
	/// </summary>
	void LifeLost() {
		StopAllCoroutines();
		messageAnimation.Play("HideMessage");
	}
	/// <summary>
	/// Restarts the game.
	/// </summary>
	void RestartGame() {
		StopAllCoroutines();
		messageAnimation.Play("HideMessage");
	}

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable() {
		Messenger<string>.AddListener(GlobalEvents.DisplayInGameMessage, DisplayInGameMessage);
		Messenger.AddListener(GlobalEvents.HideInGameMessageInstantly, HideInGameMessageInstantly);
		Messenger.AddListener(GlobalEvents.HideInGameMessage, HideInGameMessage);
		Messenger.AddListener(GlobalEvents.LifeLost, LifeLost);
		Messenger.AddListener(MenuEvents.RestartGame, RestartGame);
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable() {
		Messenger<string>.RemoveListener(GlobalEvents.DisplayInGameMessage, DisplayInGameMessage);
		Messenger.RemoveListener(GlobalEvents.HideInGameMessageInstantly, HideInGameMessageInstantly);
		Messenger.RemoveListener(GlobalEvents.HideInGameMessage, HideInGameMessage);
		Messenger.RemoveListener(GlobalEvents.LifeLost, LifeLost);
		Messenger.RemoveListener(MenuEvents.RestartGame, RestartGame);
	}

}
