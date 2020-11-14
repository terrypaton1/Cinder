using UnityEngine;

public class FallingPowerup : BaseObject {
	
	PowerupType _powerupType;
	Collider2D _collider;
	public GameObject visualObjects;
	
	Rigidbody2D rigid2D;
	
	float currentFallingSpeed = 0;
	
	float maximumFallingSpeed;
	
	bool isFalling = false;

	void OnEnable() {
		Messenger.AddListener(MenuEvents.LevelComplete, LevelComplete);
		Messenger.AddListener(GlobalEvents.LifeLost, LifeLost);
	}

	void OnDisable() {
		Messenger.RemoveListener(MenuEvents.LevelComplete, LevelComplete);
		Messenger.RemoveListener(GlobalEvents.LifeLost, LifeLost);
	}
	
	void LifeLost() {

		if (!isFalling)
			return;
		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.DestroyFallingItems,transform.position, MessengerMode.DONT_REQUIRE_LISTENER);

		DisableFallingPowerup();
	}
	
	void LevelComplete() {		
		isFalling = false;
		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.DestroyFallingItems,transform.position, MessengerMode.DONT_REQUIRE_LISTENER);
		DisableFallingPowerup();
	}
	
	void FixedUpdate() {
		if (isFalling) {
			currentFallingSpeed = Mathf.Lerp(currentFallingSpeed, maximumFallingSpeed, Time.deltaTime);
			Vector3 position = rigid2D.transform.position;
			position.y -= currentFallingSpeed;
			rigid2D.MovePosition(position);
		}
	}
	
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
	
	public void StartFalling(Vector3 position) {
		// start falling, slowly at first, then faster
//		Debug.Log("start falling");
		transform.position = position;
		visualObjects.SetActive(true);
		_collider.enabled = true;
		currentFallingSpeed = 0;
		isFalling = true;
	}
	
	public void DisableFallingPowerup() {
//		Debug.Log("DisableFallingPowerup");
		visualObjects.SetActive(false);
		_collider.enabled = false;
	}
	
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
