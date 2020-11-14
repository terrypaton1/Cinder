using UnityEngine;

public class FallingPoints : BaseObject {
	public GameObject pointsDisplay10;

	public GameObject pointsDisplay50;

	/// <summary>
	/// The points display100.
	/// </summary>
	public GameObject pointsDisplay100;

	/// <summary>
	/// The points display500.
	/// </summary>
	public GameObject pointsDisplay500;

	/// <summary>
	/// The visual objects.
	/// </summary>
	public GameObject visualObjects;

	/// <summary>
	/// The collider.
	/// </summary>
	Collider2D _collider;

	/// <summary>
	/// The points value.
	/// </summary>
	int _pointsValue;

	int category;

	/// <summary>
	/// The current falling speed.
	/// </summary>
	float currentFallingSpeed;

	/// <summary>
	/// The is falling.
	/// </summary>
	bool isFalling;

	/// <summary>
	/// The maximum falling speed. Updated from GameVariables on setup
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
			Vector2 position = rigid2D.transform.position;
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
//			Debug.Log("Points Collected by Player");
			PlaySound(SoundList.PointsCollected);
			Messenger<int>.Broadcast(GlobalEvents.PointsCollected, _pointsValue, MessengerMode.DONT_REQUIRE_LISTENER);
			if (collision.contacts.Length > 0) {
				Messenger<ParticleTypes,Vector3>.Broadcast (GlobalEvents.SpawnParticleEffect, ParticleTypes.FallingPointsCollected, collision.contacts [0].point, MessengerMode.DONT_REQUIRE_LISTENER);
			}
			Disable();
		}
		if (collision.gameObject.CompareTag("deadzone")) {
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
	/// Level complete. Stop the falling powerup
	/// </summary>
	void LevelComplete() {
	}

	/// <summary>
	/// Setup the specified _powerupType.
	/// </summary>
	/// <param name="_powerupType">Powerup type.</param>
	public void Setup(int newPointsValue, int _category) {
		category = _category;
		maximumFallingSpeed = GameVariables.maximumFallingItemSpeed;
		_pointsValue = newPointsValue;
		// disable all pointsDisplay
		HideAllVisualObjects();
		SetupVisualDisplay();
//		Debug.Log("setup falling points:" + _pointsValue);
		_collider = GetComponentInChildren<Collider2D>();
		rigid2D = GetComponent<Rigidbody2D>();
//		Debug.Log("colliders:" + colliders.Length);
		if (_collider == null) {
			Debug.LogError("Didn't find a colliders for falling points");
		}
		currentFallingSpeed = 0;
		isFalling = false;
	}

	/// <summary>
	/// Setups the visual display.
	/// </summary>
	void SetupVisualDisplay() {
		switch (category) {
			default:
			case 0:
				pointsDisplay10.SetActive(true);
				break;
			case 1:
				pointsDisplay50.SetActive(true);
				break;
			case 2:
				pointsDisplay100.SetActive(true);
				break;
			case 3:
				pointsDisplay500.SetActive(true);
				break;
		}
	}

	/// <summary>
	/// Starts the falling.
	/// </summary>
	/// <param name="position">Position.</param>
	public void StartFalling(Vector3 position) {
		// start falling, slowly at first, then faster
		//		Debug.Log("start falling");
		SetupVisualDisplay();
		transform.position = position;
		_collider.enabled = true;
		currentFallingSpeed = 0;
		isFalling = true;
	}

	/// <summary>
	/// Disables the falling powerup.
	/// </summary>
	public void Disable() {
//		Debug.Log("DisableFallingPoint");
		HideAllVisualObjects();
		_collider.enabled = false;
		isFalling = false;
		currentFallingSpeed = 0;
	}

	/// <summary>
	/// Hides all visual objects.
	/// </summary>
	void HideAllVisualObjects() {
		pointsDisplay10.SetActive(false);
		pointsDisplay50.SetActive(false);
		pointsDisplay100.SetActive(false);
		pointsDisplay500.SetActive(false);
	}
}
