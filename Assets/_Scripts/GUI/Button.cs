#region

using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

#endregion

public class Button : BaseObject {
	[SerializeField][FormerlySerializedAs("this_button")]
	public Buttons thisButtonID;

	Animator _animator;

	void Awake() {
		_animator = GetComponent<Animator>();
	}

	IEnumerator EvaluateButtonPress() {
		if (_animator != null) {
			_animator.Play("ButtonPressAnimation");
		}
		yield return new WaitForSeconds(.3f);
		switch (thisButtonID) {
			
			case Buttons.QuitGame:		
				Debug.Log("QuitGame");
				#if !UNITY_EDITOR	
				Application.Quit();
				#endif
				break;
			case Buttons.MainMenu:			
				Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.MainMenu, MessengerMode.DONT_REQUIRE_LISTENER);
				break;
			case Buttons.Game:
				Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.Game, MessengerMode.DONT_REQUIRE_LISTENER);
				break;
			case Buttons.Credits:
				Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.Credits, MessengerMode.DONT_REQUIRE_LISTENER);
				break;
			case Buttons.LevelChooser:			
				Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.LevelChooser, MessengerMode.DONT_REQUIRE_LISTENER);
				break;
			case Buttons.DonateToTheDeveloper:	
				Debug.Log("DONATE TO THE DEVELOPER!");
				Messenger.Broadcast(MenuEvents.DonateToTheDeveloper, MessengerMode.DONT_REQUIRE_LISTENER);
				break;
			case Buttons.Achievements:	
				Debug.Log("Achievements!");
				Messenger.Broadcast(SocialEvents.ShowAchievements, MessengerMode.DONT_REQUIRE_LISTENER);
				break;
			case Buttons.Highscores:	
				Debug.Log("Achievements!");
				Messenger.Broadcast(SocialEvents.ShowHighScores, MessengerMode.DONT_REQUIRE_LISTENER);
				break;
			case Buttons.CancelSignoutOfSocial:	
				Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.MainMenu);
				break;
			case Buttons.ConfirmSignoutOfSocial:	
//				Log("Reseting game data");

				PlayerPrefs.SetInt(DataVariables.playerHasLoggedIntoGooglePlayGames, 0);
//				PlayerPrefs.SetInt(DataVariables.currentLevel, 0);
//				PlayerPrefs.SetInt(DataVariables.maxLevelBeatenPrefix, 0);
//				PlayerPrefs.SetInt(DataVariables.bestScore, 0);
				Messenger.Broadcast(SocialEvents.Signout);
				Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.MainMenu);
				Messenger.Broadcast(SocialEvents.ConfirmSignoutOfSocial, MessengerMode.DONT_REQUIRE_LISTENER);
				break;
		/*
			case Buttons.ResetGame:	
				Log("Reseting game data");
				Messenger<string>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.MainMenu);
				break;
				*/
		}
	}

	/// <summary>
	/// Raises the click event.
	/// </summary>
protected	void OnClick() {
		StartCoroutine(EvaluateButtonPress());
	}
}

/// <summary>
/// Buttons.
/// </summary>
public  enum Buttons {
	MainMenu = 10,
	Credits = 20,
	LevelChooser = 30,
	Game = 40,
	RestartLevel = 50,
	PauseGame = 60,
	ResumeGame = 70,
	QuitGame = 80,
	DonateToTheDeveloper = 90,
	NextLevel = 100,
	RestartGame = 110,
	Highscores = 120,
	Achievements = 130,
	LoginUser = 140,
	CancelSignoutOfSocial = 150,
	ConfirmSignoutOfSocial = 160
}