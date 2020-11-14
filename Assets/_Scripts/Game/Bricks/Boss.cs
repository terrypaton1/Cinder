#region

using System.Collections;
using UnityEngine;

#endregion

#if UNITY_EDITOR
[SelectionBase][ExecuteInEditMode]
#endif

public class Boss : BrickBase {
	/// <summary>
	/// The falling freeze prefab.
	/// </summary>
	public GameObject fallingFreezePrefab;

	/// <summary>
	/// The can drop freeze power.
	/// </summary>
	bool canDropFreezePower;

	/// <summary>
	/// The falling points reference.
	/// </summary>
	FallingFreeze fallingFreezeReference;

	/// <summary>
	/// The freeze drop trigger count.
	/// </summary>
	int freezeDropTriggerCount;

	void Awake() {
		UpdateAmountOfHitsLeftDisplay();
		_brickAnimation = GetComponent<Animator>();
		mutltiHitSpritesReference.gameObject.SetActive(false);
		// determine if boss can drop freezes
		var levelNumber =	PlayerPrefs.GetInt(DataVariables.currentLevel);
		if (levelNumber >= GameVariables.bossesStartDroppingFreezesFromLevel) {
			canDropFreezePower = true;
		}
	}

	void OnEnable() {
		Messenger.AddListener(MenuEvents.RestartGame, ResetBrick);
		Messenger.AddListener(GlobalEvents.ActivateFlameBall, ActivateFlameBall);
		Messenger.AddListener(GlobalEvents.DisableFlameBall, ApplyNormalLayers);
	}

	void OnDisable() {
		Messenger.RemoveListener(MenuEvents.RestartGame, ResetBrick);
		Messenger.RemoveListener(GlobalEvents.ActivateFlameBall, ActivateFlameBall);
		Messenger.RemoveListener(GlobalEvents.DisableFlameBall, ApplyNormalLayers);
	}

	void OnDestroy() {
		// destroy the falling powerup too!
//		Debug.Log("destroy the falling powerup too!");
		if (fallingFreezeReference != null)
			Destroy(fallingFreezeReference.gameObject);
	}

	/// <summary>
	/// Updates the amount of hits left display.
	/// </summary>
	public override void UpdateAmountOfHitsLeftDisplay() {
//		base.UpdateAmountOfHitsLeftDisplay();
		mutltiHitSpritesReference.gameObject.SetActive(false);
	}

	/// <summary>
	/// Setups the falling point object.
	/// </summary>
	/// <param name="_fallingPointObject"OnTriggerEnter2D>Falling point object.</param>
	public override void SetupFallingPointObject(FallingPoints _fallingPointObject) {
//		Debug.Log("SetupFallingPointObject");
	}

	/// <summary>
	/// Brick has been hit by a ball.
	/// </summary>
	override public void BrickHitByBall() {
		if (canDropFreezePower) {
			// judge wether we should drop a freeze
			freezeDropTriggerCount++;
			// every 5 hits, the boss will drop a freeze, if one isn't falling already
			if (!fallingFreezeReference.isFalling) {
				// there is no freeze falling, we could drop another!
				if (freezeDropTriggerCount >= GameVariables.bossDropFreezeTriggerCount) {
					// drop a freeze! 
					StartFallingFreeze();
					freezeDropTriggerCount=0;
				}
			}
		}
		base.BrickHitByBall();
		DisplayBossHealth();
	}

	/// <summary>
	/// Starts the falling freeze 'powerup'
	/// </summary>
	void StartFallingFreeze() {
		var position = transform.position;
		position.x += Random.Range(-1.4f, 1.4f);
		fallingFreezeReference.StartFalling(position);
	}

	void DisplayBossHealth() {
		if (amountOfHitsToDestroy > 0) {
			var percent = amountOfHitsToDestroy / (float)resetHitsToDestroyCount;
			if (BossHealthRemainingDisplay.instance)
				BossHealthRemainingDisplay.instance.DisplayPercent(percent);

		} else {
			// hide the health bar

			Messenger.Broadcast(GlobalEvents.HideBossHealthBar);
		}
	}

	/// <summary>
	/// Resets the brick.
	/// </summary>
	public override void ResetBrick() {
		fallingFreezeReference.Disable();
		freezeDropTriggerCount=0;
		base.ResetBrick();
		Messenger.Broadcast(GlobalEvents.DisplayBossHealthBar);
	}

	/// <summary>
	/// Starts the item falling from destroyed brick.
	/// </summary>
	protected override void StartItemFallingFromDestroyedBrick() {
		
	}

	/// <summary>
	/// Destroys the boss.
	/// </summary>
	/// <returns>The brick.</returns>
	public override IEnumerator DestroyBrickSequence(bool playSound = true) {
		_brickAnimation.Play("BrickDestroyed");
		StartItemFallingFromDestroyedBrick();
		brickHasBeenDestroyed = true;
		// based on the boss starting health, award points
		var pointsForBoss = GameVariables.bossPointsValue * resetHitsToDestroyCount;
		#if UNITY_EDITOR
		Debug.Log("pointsForBoss:" + pointsForBoss);
		#endif
		Messenger<int>.Broadcast(GlobalEvents.PointsCollected, pointsForBoss, MessengerMode.DONT_REQUIRE_LISTENER);
		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.BossExplosion, transform.position, MessengerMode.DONT_REQUIRE_LISTENER);
		if (playSound) {
			PlaySound(SoundList.brickDestroyed);
		}
		yield return 0;
		DisableColliders();
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
	/// Startup this instance.
	/// </summary>
	protected override void Startup() {

		SetupLayers();

		var fallingPowerupReference = Instantiate(fallingFreezePrefab);

		fallingPowerupReference.transform.parent = transform.parent;
		fallingPowerupReference.transform.position = new Vector3(8, 0, 0);
		fallingFreezeReference = fallingPowerupReference.GetComponent<FallingFreeze>();

		fallingFreezeReference.SetupFreeze();
		

		fallingFreezeReference.Disable();

		resetHitsToDestroyCount =	amountOfHitsToDestroy;
		BrickManager.instance.RegisterBrick(this, false);

		colliders = GetComponentsInChildren<Collider2D>();
		ApplyNormalLayers();
//		Debug.Log("colliders:" + colliders.Length);
		brickPointsValue = GameVariables.bossPointsValue;
//		Debug.Log("setup brick points value");
		if (colliders.Length == 0) {
			Debug.LogError("Didn't find the colliders!");
		}
		UpdateAmountOfHitsLeftDisplay();
		_brickAnimation.Play("BrickStartingState");
		StartCoroutine(RevealBrick());
		Messenger.Broadcast(GlobalEvents.DisplayBossHealthBar);
	}
}
