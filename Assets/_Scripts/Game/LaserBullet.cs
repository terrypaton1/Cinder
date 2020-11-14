using UnityEngine;

public class LaserBullet : BaseObject {
	float currentLaserSpeed;
	float laserMaxSpeed = 1;

	/// <summary>
	/// The this rigidbody.
	/// </summary>
	Rigidbody2D thisRigidbody;

	void Awake() {
		thisRigidbody = GetComponentInChildren<Rigidbody2D>();
	}

	/// <summary>
	/// Update loop
	/// </summary>
	void FixedUpdate() {
		// check if the y velocity ever reaches zero
		// check the speed of the ball and make it always locked
		if (GameVariables.instance) {
//			Debug.Log(GameVariables.instance.ballMaxSpeed);
			currentLaserSpeed = Mathf.Lerp(currentLaserSpeed, laserMaxSpeed, Time.deltaTime * 7.5f);
//			Debug.Log("currentLaserSpeed:" + currentLaserSpeed);	
			var velocity = thisRigidbody.velocity;
			if (velocity.magnitude < currentLaserSpeed) {
				thisRigidbody.velocity = velocity.normalized * currentLaserSpeed;
			}
		}		
	}

	void OnEnable() {
		Messenger.AddListener(MenuEvents.RestartGame, Destroy);
		Messenger.AddListener(MenuEvents.LevelComplete, Destroy);
		Messenger.AddListener(GlobalEvents.LifeLost, Destroy);
	}

	void OnDisable() {
		Messenger.RemoveListener(MenuEvents.RestartGame, Destroy);
		Messenger.RemoveListener(MenuEvents.LevelComplete, Destroy);
		Messenger.RemoveListener(GlobalEvents.LifeLost, Destroy);
	}

	/// <summary>
	/// Raises the collision enter2 d event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnCollisionEnter2D(Collision2D collision) {
//		Debug.Log(collision.gameObject.tag);
		// if it hits a brick …
		if (collision.gameObject.CompareTag("brick")) {
			Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.LaserHitsBrick, collision.contacts [0].point, MessengerMode.DONT_REQUIRE_LISTENER);
//			Debug.Log("amountOfHitsToDestroy:"+amountOfHitsToDestroy);
			PlaySound(SoundList.LaserBulletHitsBrick);
			var _brick = collision.gameObject.GetComponent<BrickBase>();
			_brick.BrickHitByBall();
			Destroy();
		}else{

			PlaySound(SoundList.LaserBulletHitsWall);
			Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.LaserHitsBrick, collision.contacts [0].point, MessengerMode.DONT_REQUIRE_LISTENER);
			Destroy();
		}
	}

	/// <summary>
	/// Launch the specified velocity.
	/// </summary>
	/// <param name="velocity">Velocity.</param>
	public void Launch(Vector3 velocity) {
//		Debug.Log("velocity:" + velocity);
		laserMaxSpeed = GameVariables.laserBulletSpeed;
		thisRigidbody.velocity = velocity;

	}

	public void HitABrick(Vector2 particleSpawnPosition) {
		//		Debug.Log("Laser HitABrick!");
//		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.LaserHitsBrick, particleSpawnPosition, MessengerMode.DONT_REQUIRE_LISTENER);

			PlaySound(SoundList.LaserBulletHitsBrick);
		Destroy();
	}

	/// <summary>
	/// Destroy this instance.
	/// </summary>
	void Destroy() {
		Destroy(gameObject);
	}
}
