using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FallingPowerup : BaseObject {
	// this object is disabled until activated
	/// <summary>
	/// The type of the powerup.
	/// </summary>
	PowerupType _powerupType;
	/// <summary>
	/// The collider.
	/// </summary>
	Collider2D _collider;
	/// <summary>
	/// The visual objects.
	/// </summary>
	public GameObject visualObjects;
	/// <summary>
	/// The Rigidbody2D
	/// </summary>
	Rigidbody2D rigid2D;
	/// <summary>
	/// The current falling speed.
	/// </summary>
	float currentFallingSpeed = 0;
	/// <summary>
	/// The maximum falling speed.
	/// </summary>
	float maximumFallingSpeed;
	/// <summary>
	/// The is falling.
	/// </summary>
	bool isFalling = false;

	void OnEnable() {
		Messenger.AddListener(MenuEvents.LevelComplete, LevelComplete);
		Messenger.AddListener(GlobalEvents.LifeLost, LifeLost);
	}

	void OnDisable() {
		Messenger.RemoveListener(MenuEvents.LevelComplete, LevelComplete);
		Messenger.RemoveListener(GlobalEvents.LifeLost, LifeLost);
	}

	/// <summary>
	/// The player loses a life
	/// </summary>
	void LifeLost() {

		if (!isFalling)
			return;
		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.DestroyFallingItems,transform.position, MessengerMode.DONT_REQUIRE_LISTENER);

		DisableFallingPowerup();
	}

	/// <summary>
	/// Level complete. Stop the falling powerup and dispose of it
	/// </summary>
	void LevelComplete() {		
		isFalling = false;
		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.DestroyFallingItems,transform.position, MessengerMode.DONT_REQUIRE_LISTENER);
		DisableFallingPowerup();
	}

	/// <summary>
	/// the Fixed update.
	/// </summary>
	void FixedUpdate() {
		if (isFalling) {
			currentFallingSpeed = Mathf.Lerp(currentFallingSpeed, maximumFallingSpeed, Time.deltaTime);
			Vector3 position = rigid2D.transform.position;
			position.y -= currentFallingSpeed;
			rigid2D.MovePosition(position);
		}
	}

	/// <summary>
	/// Setup the specified _powerupType.
	/// </summary>
	/// <param name="_powerupType">Powerup type.</param>
	public void Setup(PowerupType newPowerupType) {
		maximumFallingSpeed = GameVariables.maximumFallingItemSpeed;
		_powerupType = newPowerupType;
//		Debug.Log("setup falling powerup:" + _powerupType.ToString());
		_collider = GetComponentInChildren<Collider2D>();
		rigid2D = GetComponent<Rigidbody2D>();
//		Debug.Log("colliders:" + colliders.Length);
		if (_collider == null) {
			Debug.LogError("Didn't find a colliders for falling powerup");
		}
		currentFallingSpeed = 0;
		isFalling = false;
	}

	/// <summary>
	/// Starts the falling.
	/// </summary>
	/// <param name="position">Position.</param>
	public void StartFalling(Vector3 position) {
		// start falling, slowly at first, then faster
//		Debug.Log("start falling");
		transform.position = position;
		visualObjects.SetActive(true);
		_collider.enabled = true;
		currentFallingSpeed = 0;
		isFalling = true;
	}

	/// <summary>
	/// Disables the falling powerup.
	/// </summary>
	public void DisableFallingPowerup() {
//		Debug.Log("DisableFallingPowerup");
		visualObjects.SetActive(false);
		_collider.enabled = false;
	}

	/// <summary>
	/// Raises the collision enter2D event.
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionEnter2D(Collision2D collision) {		
		if (collision.gameObject.CompareTag("playersbat")) {
			Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.PowerupCollected, collision.contacts [0].point, MessengerMode.DONT_REQUIRE_LISTENER);
		
			Messenger<PowerupType>.Broadcast(GlobalEvents.ActivatePowerup, _powerupType);
			DisableFallingPowerup();
		}
		if (collision.gameObject.CompareTag("deadzone")) {
			Vector3 powerupLostEffectPosition = transform.position;
			// find the bottom of the screen
			powerupLostEffectPosition.y = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
			Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.PowerupLost, powerupLostEffectPosition, MessengerMode.DONT_REQUIRE_LISTENER);
			DisableFallingPowerup();
		}
	}
}
