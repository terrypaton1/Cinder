#region

using UnityEngine;

#endregion

public class GooglePlayLoginButton : BaseObject {
	/// <summary>
	/// The logged in label.
	/// </summary>
	[SerializeField]
	UILabel textLabel;

	[SerializeField]
	GameObject loginPromptMessage;

	/// <summary>
	/// The animator.
	/// </summary>
	Animator _animator;

	/// <summary>
	/// The player logged in.
	/// </summary>
	bool playerLoggedIn;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		_animator = GetComponent<Animator>();
	}

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable() {
		Messenger<bool>.AddListener(GlobalEvents.LoginResult, LoginResult);
		Messenger.AddListener(SocialEvents.ConfirmSignoutOfSocial, CheckLoginState);	
		Messenger.AddListener(SocialEvents.AttemptingLogin, AttemptingLogin);
		CheckLoginState();
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable() {
		Messenger<bool>.RemoveListener(GlobalEvents.LoginResult, LoginResult);
		Messenger.RemoveListener(SocialEvents.AttemptingLogin, AttemptingLogin);
		Messenger.RemoveListener(SocialEvents.ConfirmSignoutOfSocial, CheckLoginState);	
	}

	/// <summary>
	/// Raises the click event.
	/// </summary>
	void OnClick() {
		if (_animator != null) {
			_animator.Play("ButtonPressAnimation");
		}
		if (!playerLoggedIn) {
			Log("Trying to log the player in");

			Messenger.Broadcast(SocialEvents.AttemptingLogin, MessengerMode.DONT_REQUIRE_LISTENER);
			Messenger.Broadcast(SocialEvents.LoginUser);
		} else {
			Debug.Log("user is logged in");
			// display the confirm sign the player out dialog
			Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.ConfirmLogOut, MessengerMode.DONT_REQUIRE_LISTENER);

		}
	}

	/// <summary>
	/// Attemptings the login.
	/// </summary>
	void AttemptingLogin() {
		PlayerPrefs.SetInt(DataVariables.playerHasLoggedIntoGooglePlayGames, 1);
		textLabel.text = "Logging in ...";
		CheckLoginState();
	}

	/// <summary>
	/// The login result has returned from google play games, this could mean the maximum level unlocked has changed, update the details
	/// </summary>
	/// <param name="success">If set to <c>true</c> success.</param>
	void LoginResult(bool success) {
		Log("GooglePlayLoginButton:" + success);
		if (success) {
			// save game has been loaded, update any details we need to.
			CheckLoginState();
		} else {
			// user is not logged in
			playerLoggedIn = false;
			textLabel.text = "Log in";
			loginPromptMessage.SetActive(true);
		}
	}

	/// <summary>
	/// Checks the state of the login.
	/// </summary>
	void CheckLoginState() {
		if (PlayerPrefs.GetInt(DataVariables.playerHasLoggedIntoGooglePlayGames) == 0) {
			PlayerIsNotLoggedIn();
			return;
		}
//		Debug.Log("CheckLoginState");
		Log("CheckLoginState");


		if (Social.localUser.authenticated){
//		if (PlayGamesPlatform.Instance.IsAuthenticated()) {
			// user is logged in
			GameVariables.instance.playerIsLoggedIn = true;
			playerLoggedIn = true;
			textLabel.text = "Logged in :)";
			loginPromptMessage.SetActive(false);
		} else {
			// user is not logged in
			PlayerIsNotLoggedIn();
			// start checking if the player is logged in repeqatedly, every few seconds
			if (gameObject.activeInHierarchy)
				Invoke("CheckLoginState", 2);
		}
	}

	/// <summary>
	/// Players the is not logged in.
	/// </summary>
	void PlayerIsNotLoggedIn() {
		playerLoggedIn = false;
		textLabel.text = "Log in";
		GameVariables.instance.playerIsLoggedIn = false;
		loginPromptMessage.SetActive(true);

	}
}
