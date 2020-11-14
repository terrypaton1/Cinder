#region

using System.Collections;
using UnityEngine;

#endregion

public class TNTBrick : BrickBase {
	/// <summary>
	/// The explosion range.
	/// </summary>
	float explosionRange = 1;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		_brickAnimation = GetComponent<Animator>();
	}

	/// <summary>
	/// Raises the collision enter2D event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnCollisionEnter2D(Collision2D collision) {
		if (brickHasBeenDestroyed)
			return;
//		Debug.Log("TNT:" + collision.gameObject.tag);
		if (collision.gameObject.CompareTag("Ball")) {
			BrickHitByBall();
			// tell the ball you hit me
			var _ball = collision.gameObject.GetComponent<Ball>();
			_ball.HitABrick();
//			Debug.Log("Explode this brick");
		}
	}

	/// <summary>
	/// Startup this instance.
	/// </summary>
	protected override void Startup() {
		SetupLayers();
		explosionRange = GameVariables.tntExplosionRange;
		resetHitsToDestroyCount =	amountOfHitsToDestroy;
		BrickManager.instance.RegisterBrick(this, false);
		colliders = GetComponentsInChildren<Collider2D>();
//		Debug.Log("colliders:" + colliders.Length);
		brickPointsValue = 50;
//		Debug.Log("setup brick points value");
		if (colliders.Length == 0) {
			Debug.LogError("Didn't find the colliders!");
		}
		ApplyNormalLayers();
	}

	/// <summary>
	/// Destroys the brick.
	/// </summary>
	/// <returns>The brick.</returns>
	public override IEnumerator DestroyBrickSequence(bool playSound = true) {

		_brickAnimation.Play("BrickDestroyed");
		brickHasBeenDestroyed = true;

		Messenger<int>.Broadcast(GlobalEvents.PointsCollected, brickPointsValue, MessengerMode.DONT_REQUIRE_LISTENER);
		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.TNTExplosion, transform.position, MessengerMode.DONT_REQUIRE_LISTENER);
		PlaySound(SoundList.TNTBrick);
		yield return 0;
		DisableColliders();

		BrickManager.instance.TestDestroyBricksAroundTNT(transform.position, explosionRange, this);
		while (visualScale.x > 0.1f) {			
			visualScale.x *= .8f;
			visualScale.y *= .8f;
			visualObjects.transform.localScale = visualScale;
			yield return 0;
		}
		BrickManager.instance.BrickDestroyed(this);
		visualObjects.SetActive(false);
	}

	/// <summary>
	/// Updates the amount of hits left display. TNT brick doesn't use this
	/// </summary>
	public override void UpdateAmountOfHitsLeftDisplay() {
		// TNT brick doesn't use this
	}
}
