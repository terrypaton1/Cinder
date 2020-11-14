#region

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#endregion

public class BallManager : BaseObject {
	/// <summary>
	/// The ball prefab reference.
	/// </summary>
	public Ball ballPrefabReference;

	/// <summary>
	/// The current ball list.
	/// </summary>
	public List<Ball>	BallList = new List<Ball>();

	/// <summary>
	/// The ball max speed for current level.
	/// </summary>
	float ballMaxSpeedForCurrentLevel = 5;

	/// <summary>
	/// The ball start position.
	/// </summary>
	Vector3 ballStartPosition;

	void Awake() {
		BallList = new List<Ball>();
	}

	void OnEnable() {
		Messenger.AddListener(MenuEvents.RestartGame, RestartLevel);
		Messenger.AddListener(MenuEvents.LevelComplete, LevelComplete);
		Messenger.AddListener(MenuEvents.NextLevel, GoingToNextLevel);
	}

	void OnDisable() {
		Messenger.RemoveListener(MenuEvents.RestartGame, RestartLevel);
		Messenger.RemoveListener(MenuEvents.LevelComplete, LevelComplete);
		Messenger.RemoveListener(MenuEvents.NextLevel, GoingToNextLevel);
	}

	/// <summary>
	/// the Level is complete.
	/// </summary>
	void LevelComplete() {
		StopAllCoroutines();
	}

	/// <summary>
	/// Goings to next level.
	/// </summary>
	void GoingToNextLevel() {
		// Destory all the current balls
		StopAllCoroutines();
	}

	/// <summary>
	/// Starts the level.
	/// </summary>
	public void AddNewBall() {
		StartCoroutine(InstantiateNewBall());
	}

	/// <summary>
	/// Adds the multiballs.
	/// </summary>
	/// <param name="xPosition">X position.</param>
	public void AddMultiballs(float xPosition = 0) {
		StartCoroutine(InstantiateMultiballs(xPosition));
	}

	/// <summary>
	/// Restarts the level.
	/// </summary>
	void RestartLevel() {
//		UnityEngine.Debug.Log("RestartLevel");
		// destroy all current balls
		StopAllCoroutines();
		DestroyAllBalls();

		StartCoroutine(StartLevelSequence());
	}

	/// <summary>
	/// Destroies all balls.
	/// </summary>
	void DestroyAllBalls() {
		for (var i = 0; i < BallList.Count; i++) {
			// destroy each of the balls
			Destroy(BallList [i].gameObject);
		}
		BallList = new List<Ball>();
	}

	/// <summary>
	/// Starts the level sequence.
	/// </summary>
	/// <returns>The level sequence.</returns>
	IEnumerator StartLevelSequence() {
//		UnityEngine.Debug.Log("StartLevelSequence");
		BallList = new List<Ball>();
		yield return 0;
		AddNewBall();
	}

	/// <summary>
	/// Instantiates the new ball.
	/// </summary>
	IEnumerator InstantiateNewBall() {
		//Debug.Log("InstantiateNewBall");
		ballStartPosition.y = GameVariables.ballStartHeight;
		ballStartPosition.x = 0;
		// show a particle effect of a new ball being instantiated
		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.NewBallOne, ballStartPosition, MessengerMode.DONT_REQUIRE_LISTENER);
		// wait a little while
		yield return new WaitForSeconds(.5f);
		var _ball = Instantiate(ballPrefabReference, ballStartPosition, Quaternion.identity);
		_ball.transform.parent = transform;
		BallList.Add(_ball);
		yield return new WaitForSeconds(.5f);
		// get speed for ball
		ballMaxSpeedForCurrentLevel = CalculateBallMaxSpeed();
		// launch the ball
		_ball.LaunchBall(ballMaxSpeedForCurrentLevel);
	}

	/// <summary>
	/// Instantiates the multiballs.
	/// </summary>
	/// <returns>The multiballs.</returns>
	/// <param name="xPosition">X position.</param>
	IEnumerator InstantiateMultiballs(float xPosition) {
		ballStartPosition.y = GameVariables.ballStartHeight;
		// show a particle effect of a new ball being instantiated
		//	Debug.Log("show a particle effect of a new ball being instantiated");
		ballStartPosition.x = xPosition;
		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.NewBallOne, ballStartPosition, MessengerMode.DONT_REQUIRE_LISTENER);
		ballStartPosition.x = -xPosition;
		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.NewBallTwo, ballStartPosition, MessengerMode.DONT_REQUIRE_LISTENER);
		// wait a little while
		yield return new WaitForSeconds(.5f);
		// add ball one
		ballStartPosition.x = xPosition;
		var ball1 = Instantiate(ballPrefabReference, ballStartPosition, Quaternion.identity);
		ball1.transform.parent = transform;
		BallList.Add(ball1);
		// add ball two
		ballStartPosition.x = -xPosition;
		var ball2 = Instantiate(ballPrefabReference, ballStartPosition, Quaternion.identity);
		ball1.transform.parent = transform;
		BallList.Add(ball2);
		yield return new WaitForSeconds(.5f);
		// get speed for ball
		ballMaxSpeedForCurrentLevel = CalculateBallMaxSpeed();
		// launch the balls
		ball1.LaunchBall(ballMaxSpeedForCurrentLevel);
		ball2.LaunchBall(ballMaxSpeedForCurrentLevel);
	}

	/// <summary>
	/// Calculates the ball max speed.
	/// </summary>
	/// <returns>The ball max speed.</returns>
	float CalculateBallMaxSpeed() {
		// get the current level
		var levelNumber =	PlayerPrefs.GetInt(DataVariables.currentLevel);
		if (levelNumber < 6) {
			return GameVariables.ballMaxSpeedPhase1;
		}
		if (levelNumber < 21) {
//			Debug.Log("GameVariables.instance.ballMaxSpeedPhase2:"+GameVariables.ballMaxSpeedPhase2);
			return GameVariables.ballMaxSpeedPhase2;
		}
		if (levelNumber < 41) {
			return GameVariables.ballMaxSpeedPhase3;
		}
		return GameVariables.ballMaxSpeedPhase4;
	}

	/// <summary>
	/// Balls the destroyed.
	/// </summary>
	/// <param name="ball">Ball.</param>
	public void BallDestroyed(Ball ball) {
		var ballLostEffectPsoition = ball.transform.position;
		// find the bottom of the screen
		ballLostEffectPsoition.y = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.BallLost, ballLostEffectPsoition, MessengerMode.DONT_REQUIRE_LISTENER);
		PlaySound(SoundList.ballLost);
		RemoveBall(ball);
		// need to check if there are any balls left in the game
		// if there are no balls in the scene, the player loses a life.
		// and the player has lives left, then
		if (BallList.Count < 1) {
			PlayerLifeManager.instance.PlayerLosesALife();			
		}
	}

	/// <summary>
	/// Removes a ball.
	/// </summary>
	/// <param name="ball">Ball.</param>
	public void RemoveBall(Ball ball) {
		if (BallList.Contains(ball)) {
			BallList.Remove(ball);
		}
		Destroy(ball.gameObject);
	}

	#region instance

	void OnDestroy() {	
		s_Instance = null;
	}

	// ************************************
	// s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
	// ************************************
	private static BallManager s_Instance;

	// ************************************
	// This defines a static instance property that attempts to find the manager object in the scene and
	// returns it to the caller.
	// ************************************
	public static BallManager instance {
		get {
			if (s_Instance == null) {
				// This is where the magic happens.
				//  FindObjectOfType(...) returns the first AManager object in the scene.
				s_Instance = FindObjectOfType(typeof(BallManager)) as BallManager;
			}

			// If it is still null, create a new instance
			if (s_Instance == null) {
				Debug.Log("Could not locate an BallManager object!");
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
