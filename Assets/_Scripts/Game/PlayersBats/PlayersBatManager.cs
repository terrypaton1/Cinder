using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public enum PlayerBatTypes {
	None,
	Normal,
	Wide,
	Small,
	Split,
	Laser
}

public class PlayersBatManager : BaseObject {
	/// <summary>
	/// The bat Y position.
	/// </summary>
	float batYPosition = 4;
	/// <summary>
	/// The current rotation.
	/// </summary>
	float currentRotation = 0;
	/// <summary>
	/// The touch position.
	/// </summary>
	public TouchPosition touchPosition;
	/// <summary>
	/// The minimum X position.
	/// </summary>
	float minimumXPosition = 0;
	/// <summary>
	/// The maximum X position.
	/// </summary>
	float maximumXPosition = 0;
	/// <summary>
	/// The last X position.
	/// </summary>
	float lastXPosition = 0;
	/// <summary>
	/// The target rotation.
	/// </summary>
	float targetRotation = 0;
	/// <summary>
	/// The last movement direction.
	/// </summary>
	float lastMovementDirection = 0;
	/// <summary>
	/// The current bat position.
	/// </summary>
	Vector3 currentBatPosition;
	/// <summary>
	/// The current bat.
	/// </summary>
	PlayersBatBase currentBat;
	/// <summary>
	/// The players bat trail.
	/// </summary>
	[SerializeField]
	GameObject playersBatTrail;
	/// <summary>
	/// The normal bat.
	/// </summary>
	[SerializeField]
	PlayersBatBase _normalBat;
	/// <summary>
	/// The wide bat.
	/// </summary>
	[SerializeField]
	PlayersBatBase _wideBat;
	/// <summary>
	/// The small bat.
	/// </summary>
	[SerializeField]
	PlayersBatBase _smallBat;
	/// <summary>
	/// The split bat.
	/// </summary>
	[SerializeField]
	PlayersBatBase _splitBat;
	/// <summary>
	/// The laser bat.
	/// </summary>
	[SerializeField]
	PlayersBatBase _laserBat;
	/// <summary>
	/// The type of the current bat.
	/// </summary>
	PlayerBatTypes currentBatType;
	/// <summary>
	/// The type of the previous bat.
	/// </summary>
	PlayerBatTypes previousBatType;
	/// <summary>
	/// The type of the next bat.
	/// </summary>
	PlayerBatTypes nextBatType;
	/// <summary>
	/// The play is active.
	/// </summary>
	bool playIsActive = false;
	/// <summary>
	/// The is morphing a bat.
	/// </summary>
	bool isMorphingBat = false;
	bool freezePlayerActive = false;

	/// <summary>
	/// Fixed update loop
	/// </summary>
	void FixedUpdate() {
		PositionAndRotateCurrentActiveBat();
	}

	/// <summary>
	/// Positions and rotate current active bat.
	/// </summary>
	void PositionAndRotateCurrentActiveBat() {
		if (freezePlayerActive) {
			// emit particles where the player is			
			Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.FreezePlayer, currentBat.rigidRef.position, MessengerMode.DONT_REQUIRE_LISTENER);
			return;
		}
		if (!playIsActive)
			return;
		Vector2 mousePosition = touchPosition.GetPlayersPosition();
		mousePosition.y = batYPosition;
		mousePosition.x = Mathf.Clamp(mousePosition.x, minimumXPosition, maximumXPosition);

//		Log("mousePosition:"+mousePosition);
//		Debug.Break();
		currentBatPosition = currentBat.rigidRef.position;
		// lerp towards the mouse position
		float newXPosition = Mathf.Lerp(currentBatPosition.x, mousePosition.x, Time.deltaTime * 60f);
		float dir = currentBatPosition.x - mousePosition.x;
		if (dir > 0 && lastMovementDirection < 0 || dir < 0 && lastMovementDirection > 0) {
			targetRotation = 0;
//			Debug.Log("changed direction");
		}
		lastMovementDirection = dir;
		currentBatPosition.x = newXPosition;
		// angle based on the amount moving
		float newDistance = lastXPosition - currentBat.rigidRef.position.x;
		if (Mathf.Abs(newDistance) > .02f) {
			targetRotation += (newDistance * 10);
		}
		targetRotation *= .9f;
		targetRotation = Mathf.Clamp(targetRotation, -60f, 60f);
		currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.deltaTime * 16f);
		// position the current bat
		currentBat.rigidRef.MovePosition(currentBatPosition);
		currentBat.rigidRef.MoveRotation(currentRotation);
		playersBatTrail.transform.position = currentBatPosition;
//		Debug.Log("currentRotation:"+currentRotation);
//		Debug.Log("newDistance:" + newDistance);
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {	
		currentBatPosition = new Vector3(0, GameVariables.playersBatYPosition, 0);
		minimumXPosition = Camera.main.ViewportToWorldPoint(Vector3.zero).x; 
		maximumXPosition = Camera.main.ViewportToWorldPoint(Vector3.one).x;
		InvokeRepeating("MonitorPlayersPosition", 1, 0.2f);
		StartCoroutine(StartGameSequence());
	}

	/// <summary>
	/// Starts the game sequence.
	/// </summary>
	/// <returns>The game sequence.</returns>
	IEnumerator StartGameSequence() {
		HideAllBats();
		ChangeToNewBat(PlayerBatTypes.Normal);
		yield return new WaitForSeconds(1);
		playIsActive = true;
	}

	/// <summary>
	/// Changes to new bat.
	/// </summary>
	public void ChangeToNewBat(PlayerBatTypes _newBatType) {
		if (_newBatType == currentBatType) {
			// this power up is already active
			return;
		}
		if (isMorphingBat) {
			StopCoroutine(TransitionToNextBatType());
		}
		previousBatType = currentBatType;
		nextBatType = _newBatType;
		// depending on the previous bat type, we need to transition to the next
		if (previousBatType != PlayerBatTypes.None) {
			Debug.Log("transition to next bat type:" + nextBatType.ToString());

			StopCoroutine(PlayerLosesLifeSequence());
			StartCoroutine(TransitionToNextBatType());
		} else {
			// no bat was previously enabled, switch the bat type instantly
			SwitchToNextBat();
			// might want a first reavel of the players bat, growing out of a small dot
			currentBat.MorphToPlayState();
		}
	}



	/// <summary>
	/// Hides all bats.
	/// </summary>
	void HideAllBats() {
		_normalBat.gameObject.SetActive(false);
		_wideBat.gameObject.SetActive(false);
		_smallBat.gameObject.SetActive(false);
		_splitBat.gameObject.SetActive(false);
		_laserBat.gameObject.SetActive(false);
	}

	/// <summary>
	/// Transitions the type of the to next bat.
	/// </summary>
	/// <returns>The to next bat type.</returns>
	IEnumerator TransitionToNextBatType() {
		// first we transition the current bat to look normal
		isMorphingBat = true;
		if (currentBat != _normalBat) {
			currentBat.MorphToNormal();
			yield return new WaitForSeconds(1f);
		}
		// then we swap to the new bat (which starts looking like a normal bat)
		SwitchToNextBat();
		// then we morph to the playing state of the next bat

		if (currentBat != _normalBat) {
			currentBat.MorphToPlayState();
		} else {
			// make normal bat
			currentBat.MorphToNormal();
		}
		yield return 0;
		
		Debug.Log("Bat transition finished");
		isMorphingBat = false;

	}

	/// <summary>
	/// Switchs to next bat.
	/// </summary>
	void SwitchToNextBat() {
		HideAllBats();
		currentBatType = nextBatType;
		switch (nextBatType) {
			case PlayerBatTypes.Normal:
				currentBat = _normalBat;
				break;
			case PlayerBatTypes.Wide:
				currentBat = _wideBat;
				break;
			case PlayerBatTypes.Small:
				currentBat = _smallBat;
				break;
			case PlayerBatTypes.Split:
				currentBat = _splitBat;
				break;
			case PlayerBatTypes.Laser:
				currentBat = _laserBat;
				break;
		}
		currentBat.transform.position = currentBatPosition;
		currentBat.gameObject.SetActive(true);
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy() {
		CancelInvoke("MonitorPlayersPosition");
		s_Instance = null;
	}

	/// <summary>
	/// Monitors the players position.
	/// </summary>
	void MonitorPlayersPosition() {
		lastXPosition = currentBat.rigidRef.transform.position.x;
	}

	/// <summary>
	/// Players the loses life.
	/// </summary>
	public void PlayerLosesLife() {
		playIsActive = false;
		freezePlayerActive = false;
		StartCoroutine(PlayerLosesLifeSequence());
	}

	IEnumerator PlayerLosesLifeSequence() {
//		Debug.Log("PlayerLosesLifeSequence");
		Messenger.Broadcast(GlobalEvents.HideInGameMessage);
		currentBat.PlayerLosesLife();
		yield return new WaitForSeconds(1f);
		playIsActive = true;
		currentBatType = PlayerBatTypes.None;
		ChangeToNewBat(PlayerBatTypes.Normal);
	}

	void OnEnable() {
		Messenger.AddListener(MenuEvents.RestartGame, RestartLevel);
		Messenger.AddListener(MenuEvents.LevelComplete, LevelComplete);
		Messenger.AddListener(MenuEvents.NextLevel, GoingToNextLevel);
		Messenger.AddListener(GlobalEvents.LifeLost, LoseLife);
	}

	void OnDisable() {
		Messenger.RemoveListener(MenuEvents.RestartGame, RestartLevel);
		Messenger.RemoveListener(MenuEvents.LevelComplete, LevelComplete);
		Messenger.RemoveListener(MenuEvents.NextLevel, GoingToNextLevel);
		Messenger.RemoveListener(GlobalEvents.LifeLost, LoseLife);
	}

	/// <summary>
	/// Loses the life.
	/// </summary>
	void LoseLife() {
		StopCoroutine(TransitionToNextBatType());
	}

	/// <summary>
	/// Restarts the level.
	/// </summary>
	void RestartLevel() {

		freezePlayerActive = false;
		StopCoroutine(TransitionToNextBatType());
	}

	/// <summary>
	/// Levels the complete.
	/// </summary>
	void LevelComplete() {
		freezePlayerActive = false;
	}

	/// <summary>
	/// Goings to next level.
	/// </summary>
	void GoingToNextLevel() {
		// reset the players bat
		freezePlayerActive = false;
	}

	/// <summary>
	/// Activates the freeze player.
	/// </summary>
	public void ActivateFreezePlayer() {
		freezePlayerActive = true;
	}

	/// <summary>
	/// Disables the freeze player.
	/// </summary>
	public void DisableFreezePlayer() {
		freezePlayerActive = false;
	}

	#region instance

	// ************************************
	// s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
	// ************************************
	private static PlayersBatManager s_Instance = null;

	// ************************************
	// This defines a static instance property that attempts to find the manager object in the scene and
	// returns it to the caller.
	// ************************************
	public static PlayersBatManager instance {
		get {
			if (s_Instance == null) {
				// This is where the magic happens.
				//  FindObjectOfType(...) returns the first AManager object in the scene.
				s_Instance = FindObjectOfType(typeof(PlayersBatManager)) as PlayersBatManager;
			}

			// If it is still null, create a new instance
			if (s_Instance == null) {
				Debug.Log("Could not locate an PlayersBatManager object!");
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
