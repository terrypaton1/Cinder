using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerLifeManager : BaseObject {
	/// <summary>
	/// The player lives.
	/// </summary>
	int playerLives;
	/// <summary>
	/// The player lives text.
	/// </summary>
	[SerializeField] UILabel playerLivesText;

	void Awake() {
		playerLives = GameVariables.playerStartingLives;
		UpdateLivesDisplay();
	}

	void OnEnable() {
		Messenger.AddListener(MenuEvents.RestartGame, RestartLevel);
	}

	void OnDisable() {
		Messenger.RemoveListener(MenuEvents.RestartGame, RestartLevel);
	}
	/// <summary>
	/// Restarts the level.
	/// </summary>
	void RestartLevel() {
		StopAllCoroutines();
	}
	/// <summary>
	/// Gets the player lives.
	/// </summary>
	/// <value>The player lives.</value>
	public int PlayerLives {
		get {
			return playerLives;
		}
	}
	/// <summary>
	/// Gives the player extra life.
	/// </summary>
	public void GivePlayerExtraLife(){
		PlaySound(SoundList.ExtraLife);
		playerLives++;
		UpdateLivesDisplay();
		// show a message

	}
	/// <summary>
	/// Players the loses A life.
	/// </summary>
	public void PlayerLosesALife() {
		StartCoroutine(PlayerLifeLostSequence());
	}

	/// <summary>
	/// Players the life lost sequence.
	/// </summary>
	/// <returns>The life lost sequence.</returns>
	IEnumerator PlayerLifeLostSequence() {
//		Debug.Log("Life lost");
		PlaySound(SoundList.LifeLost);

		Messenger.Broadcast(GlobalEvents.LifeLost);
		// show a message that the player has lost a life
		playerLives--;
		UpdateLivesDisplay();
		PlayersBatManager.instance.PlayerLosesLife();
		yield return new WaitForSeconds(.5f);
		// if player loses all lives, game over
		if (playerLives < 1) {
			// game over;
			Messenger.Broadcast(GlobalEvents.GameOver);
			yield break;
		}
		// player still has lives left!
		// wait a little while
		yield return new WaitForSeconds(2f);
		// if the player has lives left, spawn a new ball
//		Debug.Log("add a new ball");
		BallManager.instance.AddNewBall();
		Messenger.Broadcast(GlobalEvents.ResumeLevelTimer,MessengerMode.DONT_REQUIRE_LISTENER);


	}

	/// <summary>
	/// Updates the lives display.
	/// </summary>
	void UpdateLivesDisplay() {
		// animate the text with a tween?
		playerLivesText.text = playerLives.ToString();
	}

	#region instance

	void OnDestroy() {	
		s_Instance = null;
	}
	// ************************************
	// s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
	// ************************************
	private static PlayerLifeManager s_Instance = null;

	// ************************************
	// This defines a static instance property that attempts to find the manager object in the scene and
	// returns it to the caller.
	// ************************************
	public static PlayerLifeManager instance {
		get {
			if (s_Instance == null) {
				// This is where the magic happens.
				//  FindObjectOfType(...) returns the first AManager object in the scene.
				s_Instance = FindObjectOfType(typeof(PlayerLifeManager)) as PlayerLifeManager;
			}

			// If it is still null, create a new instance
			if (s_Instance == null) {
				Debug.LogError("Could not locate an PlayerLifeManager object!");
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
