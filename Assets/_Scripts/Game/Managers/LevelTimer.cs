#region

using UnityEngine;

#endregion

/// <summary>
/// Level timer. Used to  track how long a level has been played. If it starts taking too long, we drop a power-up (one at a time)
/// </summary>
public class LevelTimer : MonoBehaviour {
	/// <summary>
	/// The random powerup choices.
	/// </summary>
	public PowerupType[] randomPowerupChoices;

	/// <summary>
	/// The falling power up prefab.
	/// </summary>
	public GameObject fallingPowerUpPrefab;

	/// <summary>
	/// The falling power up.
	/// </summary>
	FallingPowerup fallingPowerUp;

	/// <summary>
	/// The powerup drop started. 
	/// </summary>
	bool powerupDropStarted;

	/// <summary>
	/// The name of the repeating function.
	/// </summary>
	readonly string repeatingFunctionName = "IncrementTime";

	/// <summary>
	/// The repeating time step.
	/// </summary>
	readonly float repeatingTimeStep = 1f;

	/// <summary>
	/// The time before first powerup drops. (Seconds)
	/// </summary>
	float timeBeforeFirstPowerupDrops;

	/// <summary>
	/// The time between powerups. (Seconds)
	/// </summary>
	float timeBetweenPowerups;

	/// <summary>
	/// The time between powerups timer. (Seconds)
	/// </summary>
	float timeBetweenPowerupsTimer;

	/// <summary>
	/// The time passed.
	/// </summary>
	float timePassed;

	void OnEnable() {
		//		DontDestroyOnLoad(gameObject);
		Messenger.AddListener(MenuEvents.NextLevel, ResetTimer);
		Messenger.AddListener(MenuEvents.RestartGame, ResetTimer);
		Messenger.AddListener(MenuEvents.LevelComplete, StopRepeating);
		Messenger.AddListener(GlobalEvents.PauseGame, StopRepeating);
		Messenger.AddListener(GlobalEvents.ResumeGame, StartTimer);
//		Messenger.AddListener(GlobalEvents.QuitGame, StopRepeating);
		Messenger.AddListener(GlobalEvents.GameOver, StopRepeating);
		Messenger.AddListener(GlobalEvents.StartLevelTimer, StartTimer);
		Messenger.AddListener(GlobalEvents.LifeLost, StopRepeating);
		Messenger.AddListener(GlobalEvents.ResumeLevelTimer, StartTimer);
		timeBeforeFirstPowerupDrops = 1.5f * 60;
//		timeBeforeFirstPowerupDrops=10;
		timeBetweenPowerups = 20;
//		timeBetweenPowerups = 6;
		var fallingPowerupReference = Instantiate(fallingPowerUpPrefab);
		fallingPowerupReference.transform.parent = transform.parent;
		fallingPowerUp = fallingPowerupReference.GetComponent<FallingPowerup>();
		fallingPowerUp.Setup(PowerupType.Multiball);
		fallingPowerUp.DisableFallingPowerup();
	}

	/// <summary>
	/// Remove listeners
	/// </summary>
	void OnDisable() {
		Messenger.RemoveListener(MenuEvents.NextLevel, ResetTimer);
		Messenger.RemoveListener(MenuEvents.RestartGame, ResetTimer);
		Messenger.RemoveListener(MenuEvents.LevelComplete, StopRepeating);
		Messenger.RemoveListener(GlobalEvents.PauseGame, StopRepeating);
		Messenger.RemoveListener(GlobalEvents.ResumeGame, StartTimer);
//		Messenger.RemoveListener(GlobalEvents.QuitGame, StopRepeating);
		Messenger.RemoveListener(GlobalEvents.GameOver, StopRepeating);
		Messenger.RemoveListener(GlobalEvents.StartLevelTimer, StartTimer);
		Messenger.RemoveListener(GlobalEvents.LifeLost, StopRepeating);
		Messenger.RemoveListener(GlobalEvents.ResumeLevelTimer, StartTimer);
	}

	/// <summary>
	/// Starts the timer.
	/// </summary>
	void StartTimer() {
//		Debug.Log("start timer");
		StopRepeating();
		InvokeRepeating(repeatingFunctionName, repeatingTimeStep, repeatingTimeStep);
	}

	/// <summary>
	/// Increments the time.
	/// </summary>
	void IncrementTime() {
//		Debug.Log("IncrementTime:"+timePassed);
		// check how much time has passed;
		// check on the game state
		if (GameManager.instance.gameState != GameState.Playing)
			return;
		if (!powerupDropStarted) {
			timePassed += repeatingTimeStep;
//			Debug.Log("timePassed:" + timePassed);
			if (timePassed > timeBeforeFirstPowerupDrops) {
				powerupDropStarted = true;
//				Debug.Log("drop first powerup");
				DropPowerup();
			}
		}
		if (powerupDropStarted) {
			timeBetweenPowerupsTimer += repeatingTimeStep;
//			Debug.Log("timeBetweenPowerupsTimer:" + timeBetweenPowerupsTimer);
			if (timeBetweenPowerupsTimer > timeBetweenPowerups) {
				timeBetweenPowerupsTimer = 0;
				DropPowerup();
			}
		}
	}

	/// <summary>
	/// Resets the timer.
	/// </summary>
	void ResetTimer() {
		fallingPowerUp.DisableFallingPowerup();
		powerupDropStarted = false;
		timePassed = 0;
		timeBetweenPowerupsTimer = 0;
		StartTimer();
	}

	/// <summary>
	/// Stops the repeating.
	/// </summary>
	void StopRepeating() {
//		Debug.Log("StopRepeating");
		CancelInvoke(repeatingFunctionName);
	}

	/// <summary>
	/// Drops the powerup.
	/// </summary>
	void DropPowerup() {
//		Debug.Log("drop a powerup");
		var randomPowerUpNum = Random.Range(0, randomPowerupChoices.Length);
		var	randomTypeOfPowerUp = randomPowerupChoices [randomPowerUpNum];
//		Debug.Log("random randomTypeOfPowerUp:" + randomTypeOfPowerUp);
		fallingPowerUp.Setup(randomTypeOfPowerUp);
		// create a particle effect where the powerup will be
		var powerupStartPosition = new Vector3(Random.Range(-1.3f, 1.3f), 8, 0);
		fallingPowerUp.StartFalling(powerupStartPosition);
		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.LaserHitsBrick, powerupStartPosition, MessengerMode.DONT_REQUIRE_LISTENER);
	}
}
