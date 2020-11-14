using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
//using UnityEngine.SocialPlatforms.GameCenter;

public class GooglePlayGamesManager : BaseObject {

	SaveDataBundle playersSavedGameData;
	public static Action OnSaveGameSelected;
	public static Action<SaveDataBundle> OnSaveLoaded;
 
	private static SaveDataBundle m_currentSaveBundle;

	void Start() {

	//	PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();

		//PlayGamesPlatform.InitializeInstance(config);
		// recommended for debugging:
		//PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		if (PlayerPrefs.GetInt(DataVariables.playerHasLoggedIntoGooglePlayGames) == 1) {
//			PlayGamesPlatform.Activate();
//			//TODO
		}
	}

	void OnEnable() {
		Messenger.AddListener(GlobalEvents.LoginUser, LoginUser);
		Messenger.AddListener(SocialEvents.Signout, SignOut);
		Messenger.AddListener(SocialEvents.ShowHighScores, ShowHighScores);
		Messenger.AddListener(SocialEvents.ShowAchievements, ShowAchievements);
//		Messenger.AddListener(SocialEvents.SaveThePlayersGame, SaveThePlayersGame);
		Messenger<string,float>.AddListener(SocialEvents.ReportAchievementProgress, ReportAchievementProgress);
		Messenger<int>.AddListener(SocialEvents.ReportScore, ReportScore);

	}

	void OnDisable() {
		Messenger.RemoveListener(SocialEvents.LoginUser, LoginUser);
		Messenger.RemoveListener(SocialEvents.Signout, SignOut);
		Messenger.RemoveListener(SocialEvents.ShowHighScores, ShowHighScores);
		Messenger.RemoveListener(SocialEvents.ShowAchievements, ShowAchievements);
//		Messenger.RemoveListener(SocialEvents.SaveThePlayersGame, SaveThePlayersGame);
		Messenger<string,float>.RemoveListener(SocialEvents.ReportAchievementProgress, ReportAchievementProgress);
		Messenger<int>.RemoveListener(SocialEvents.ReportScore, ReportScore);

	}

	void ShowAchievements() {
//		Social.Active.ShowAchievementsUI();
		Log("ShowAchievements");
		Social.ShowAchievementsUI();
	}
		
	void ShowHighScores() {
		Log("ShowHighScores");
		//		Social.Active.ShowLeaderboardUI();
//		GameCenter.GameCenterPlatform.ShowLeaderboardUI();
//		PlayGamesPlatform.Instance.SetDefaultLeaderboardForUI(DataVariables.leaderboardID);
		Social.ShowLeaderboardUI();
	}

	/// <summary>
	/// Logins the user.
	/// </summary>
	void LoginUser() {
		Log("LoginUser");
//		Debug.Log("LoginUser");
//		PlayGamesPlatform.Activate();
				Social.localUser.Authenticate(ProcessAuthentication);
//		UnityEngine.SocialPlatforms.GameCenter.GameCenterPlatform.ShowLeaderboardUI();
//		Messenger.Broadcast(SocialEvents.AttemptingLogin, MessengerMode.DONT_REQUIRE_LISTENER);
	}

	/// <summary>
	/// Reports the score.
	/// </summary>
	/// <param name="score">Score.</param>
	void ReportScore(int score) {
//		Debug.Log("Reporting score " + score);	
		int currentBestScore = PlayerPrefs.GetInt(DataVariables.bestScore);	
		if (score > currentBestScore) {
			PlayerPrefs.SetInt(DataVariables.bestScore, score);
//			Social.ReportScore(score, DataVariables.leaderboardID, SubmittedScoreResult);
			// show a message, new best score
			if (currentBestScore > 0) {
				// don't show new best score unless it's greater than zero
				ShowInGameMessage("New best score!");
			}
		}
	}

	/// <summary>
	/// Submitteds the score result.
	/// </summary>
	/// <param name="success">If set to <c>true</c> success.</param>
	void SubmittedScoreResult(bool success) {
		if (success) {
			Log("Reported score successfully");
		} else {
			Log("Failed to report score");
		}
	}

	/// <summary>
	/// Processes the authentication.
	// This function gets called when Authenticate completes
	// Note that if the operation is successful, Social.localUser will contain data from the server.
	/// </summary>
	/// <param name="success">If set to <c>true</c> success.</param>
	void ProcessAuthentication(bool success) {
		if (success) {
//			Debug.Log("Authenticated, checking achievements");
			Log("Authenticated, checking achievements");
			// Request loaded achievements, and register a callback for processing them
			Social.LoadAchievements(ProcessLoadedAchievements);
			Messenger<bool>.Broadcast(GlobalEvents.LoginResult, true);
		} else {
//			Debug.Log("Failed to authenticate");
			//		Log.ConsoleScreenWrite("Failed to authenticate");
			Log("Failed to authenticate");
			Messenger<bool>.Broadcast(GlobalEvents.LoginResult, success);
		}
	}

	/// <summary>
	/// Processes the loaded achievements. 
	// This function gets called when the LoadAchievement call completes
	/// </summary>
	/// <param name="achievements">Achievements.</param>
	void ProcessLoadedAchievements(IAchievement[] achievements) {
		if (achievements.Length == 0) {
//			Debug.Log("Error: no achievements found");
			Log("Error: no achievements found");
		} else {
			Debug.Log("Got " + achievements.Length + " achievements");
//			for (int i=0;i<achievements.Length;i++){
//				Log(achievements[i].id);
//			}
			Log("Got " + achievements.Length + " achievements");
		}
	}

	/// <summary>
	/// Reports the achievement progress.
	/// </summary>
	/// <param name="_achievement">Achievement.</param>
	/// <param name="_percentAchieved">Percent achieved.</param>
	void ReportAchievementProgress(string _achievement, float _percentAchieved) {		
		Social.ReportProgress(_achievement, _percentAchieved, result => {
			if (result) {
//				Debug.Log("Successfully reported achievement progress");
				Log("Successfully reported achievement progress");
			} else {
				Debug.Log("Failed to report achievement");
				Log("Failed to report achievement");
			}
		});
	}

	/// <summary>
	/// Signs the player out of google play games.
	/// </summary>
	void SignOut() {
		Log("signing out of google play games");
//		((PlayGamesPlatform)Social.Active).SignOut();
//		Social.localUser.

		Messenger<bool>.Broadcast(GlobalEvents.LoginResult, false);
	}

}
