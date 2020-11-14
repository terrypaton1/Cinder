#region

using System.Collections;
using UnityEngine;

#endregion

public class InitialLoadScreen : BaseObject {
	/// <summary>
	/// The loading sprite.
	/// </summary>
	[SerializeField] UISprite loadingSprite;

	/// <summary>
	/// The rotation amount.
	/// </summary>
	readonly Vector3 rotationAmount = new Vector3(0, 0, -180f);

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		StartCoroutine(InitialLoadSequence());
//		Log("LoginUser");

	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update() {
		loadingSprite.transform.Rotate(rotationAmount*Time.deltaTime);
	}

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable() {
		Messenger<bool>.AddListener(GlobalEvents.LoginResult, LoginResult);

	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable() {
		Messenger<bool>.RemoveListener(GlobalEvents.LoginResult, LoginResult);
	}

	/// <summary>
	/// Initials the load sequence.
	/// </summary>
	/// <returns>The load sequence.</returns>
	IEnumerator InitialLoadSequence() {
		yield return new WaitForSeconds(.25f);

		if (PlayerPrefs.GetInt(DataVariables.playerHasLoggedIntoGooglePlayGames) == 1){
			Messenger.Broadcast(SocialEvents.LoginUser);
		}

		Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.MainMenu);
	}

	/// <summary>
	/// Logins the result.
	/// </summary>
	/// <param name="success">If set to <c>true</c> success.</param>
	void LoginResult(bool success) {
		if (success) {
			Debug.Log("Authenticated");
		} else {
			Debug.Log("Failed to authenticate");
		}
		// go to the main menu regardless of result
	}
}
