#region

using System;
using UnityEngine;

#endregion

public class ScoreManager : BaseObject {
	bool bonusLife1Collected;
	bool bonusLife2Collected;
	bool bonusLife3Collected;
	bool bonusLife4Collected;

	/// <summary>
	/// The player score.
	/// </summary>
	[NonSerialized]
	public int playerScore;

	void OnEnable() {
		Messenger.AddListener(MenuEvents.RestartGame, RestartLevel);
		Messenger<int>.AddListener(GlobalEvents.PointsCollected, PointsCollected);
	}

	void OnDisable() {
		Messenger.RemoveListener(MenuEvents.RestartGame, RestartLevel);
		Messenger<int>.RemoveListener(GlobalEvents.PointsCollected, PointsCollected);
	}

	/// <summary>
	/// Restarts the level.
	/// </summary>
	void RestartLevel() {
//		Debug.Log("RestartLevel");
		playerScore = 0;
		Messenger<int>.Broadcast(MenuEvents.UpdatePointsDisplay, playerScore, MessengerMode.DONT_REQUIRE_LISTENER);
	}

	/// <summary>
	/// Points collected.
	/// </summary>
	/// <param name="_points">Points.</param>
	void PointsCollected(int _points) {
		playerScore += _points;
		CheckForBonusLife();
		// update the ppoints display
		Messenger<int>.Broadcast(MenuEvents.UpdatePointsDisplay, playerScore, MessengerMode.DONT_REQUIRE_LISTENER);
	}

	void CheckForBonusLife() {
//		Debug.Log("playerScore:"+playerScore);
//		Debug.Log("GameVariables.instance.bonusLife1PointsThreshold:"+GameVariables.instance.bonusLife1PointsThreshold);
		if (!bonusLife1Collected) {
			if (playerScore >= GameVariables.bonusLife1PointsThreshold) {
				// give player an extra life
				bonusLife1Collected = true;
				ShowInGameMessage("Extra life!");
				PlayerLifeManager.instance.GivePlayerExtraLife();
			}
		}
		if (!bonusLife2Collected) {
			if (playerScore >= GameVariables.bonusLife2PointsThreshold) {
				bonusLife2Collected = true;
				ShowInGameMessage("Extra life!");
				PlayerLifeManager.instance.GivePlayerExtraLife();
			}
		}
		if (!bonusLife3Collected) {
			if (playerScore >= GameVariables.bonusLife3PointsThreshold) {
				bonusLife3Collected = true;
				ShowInGameMessage("Extra life!");
				PlayerLifeManager.instance.GivePlayerExtraLife();
			}
		}
		if (!bonusLife4Collected) {
			if (playerScore >= GameVariables.bonusLife4PointsThreshold) {
				bonusLife4Collected = true;
				ShowInGameMessage("Extra life!");
				PlayerLifeManager.instance.GivePlayerExtraLife();
			}
		}
	}

	#region instance

	void OnDestroy() {	
		s_Instance = null;
	}

	// ************************************
	// s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
	// ************************************
	private static ScoreManager s_Instance;

	// ************************************
	// This defines a static instance property that attempts to find the manager object in the scene and
	// returns it to the caller.
	// ************************************
	public static ScoreManager instance {
		get {
			if (s_Instance == null) {
				// This is where the magic happens.
				//  FindObjectOfType(...) returns the first AManager object in the scene.
				s_Instance = FindObjectOfType(typeof(ScoreManager)) as ScoreManager;
			}

			// If it is still null, create a new instance
			if (s_Instance == null) {
				Debug.LogError("Could not locate an ScoreManager object!");
				Debug.Break();
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
