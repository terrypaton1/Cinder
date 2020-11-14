using System.Collections;
using UnityEngine;

public class TNTBrick : BrickBase {
	
	float explosionRange = 1;

	void Awake() {
		_brickAnimation = GetComponent<Animator>();
	}

	protected override void CollisionEnterCode(Collision2D collision) {
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

	public override void UpdateAmountOfHitsLeftDisplay() {
		// TNT brick doesn't use this
	}
}
