#region

using UnityEngine;

#endregion

public class FallingFreeze : BaseObject {
	/// <summary>
	/// The visual objects.
	/// </summary>
	public GameObject visualObjects;

	/// <summary>
	/// The is falling.
	/// </summary>
	public bool isFalling;

	/// <summary>
	/// The collider.
	/// </summary>
	Collider2D _collider;

	// this object is disabled until activated
	/// <summary>
	/// The type of the powerup.
	/// </summary>
	readonly PowerupType _powerupType = PowerupType.FreezePlayer;

	/// <summary>
	/// The current falling speed.
	/// </summary>
	float currentFallingSpeed;

	/// <summary>
	/// The maximum falling speed.
	/// </summary>
	float maximumFallingSpeed;

	/// <summary>
	/// The Rigidbody2D
	/// </summary>
	Rigidbody2D rigid2D;

	/// <summary>
	/// the Fixed update.
	/// </summary>
	void FixedUpdate() {
		if (isFalling) {
			currentFallingSpeed = Mathf.Lerp(currentFallingSpeed, maximumFallingSpeed, Time.deltaTime);
			var position = rigid2D.transform.position;
			position.y -= currentFallingSpeed;
			rigid2D.MovePosition(position);
		}
	}

	void OnEnable() {
		Messenger.AddListener(MenuEvents.LevelComplete, LevelComplete);
		Messenger.AddListener(GlobalEvents.LifeLost, LifeLost);
	}

	void OnDisable() {
		Messenger.RemoveListener(MenuEvents.LevelComplete, LevelComplete);
		Messenger.RemoveListener(GlobalEvents.LifeLost, LifeLost);
	}

	/// <summary>
	/// Raises the collision enter2D event.
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionEnter2D(Collision2D collision) {		
		if (collision.gameObject.CompareTag("playersbat")) {
			Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.PowerupCollected, collision.contacts [0].point, MessengerMode.DONT_REQUIRE_LISTENER);
			Messenger<PowerupType>.Broadcast(GlobalEvents.ActivatePowerup, _powerupType);
			Disable();
		}
		if (collision.gameObject.CompareTag("deadzone")) {
			var powerupLostEffectPosition = transform.position;
			// find the bottom of the screen
			powerupLostEffectPosition.y = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
			Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.PowerupLost, powerupLostEffectPosition, MessengerMode.DONT_REQUIRE_LISTENER);
			Disable();
		}
	}

	/// <summary>
	/// The player loses a life
	/// </summary>
	void LifeLost() {

		if (!isFalling)
			return;
		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.DestroyFallingItems, transform.position, MessengerMode.DONT_REQUIRE_LISTENER);
		Disable();
	}

	/// <summary>
	/// Level complete. Stop the falling powerup and dispose of it
	/// </summary>
	void LevelComplete() {	
		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.DestroyFallingItems, transform.position, MessengerMode.DONT_REQUIRE_LISTENER);
		Disable();
	}

	public void SetupFreeze() {
		maximumFallingSpeed = GameVariables.maximumFallingItemSpeed;
		//	Debug.Log("setup falling powerup:" + _powerupType.ToString());
		_collider = GetComponentInChildren<Collider2D>();
		rigid2D = GetComponent<Rigidbody2D>();
		//	Debug.Log("colliders:" + colliders.Length);
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
	public void Disable() {
//		Debug.Log("DisableFallingPowerup");
		isFalling = false;
		visualObjects.SetActive(false);
		_collider.enabled = false;
	}
}
