using System.Collections;
using UnityEngine;


#if UNITY_EDITOR
[SelectionBase][ExecuteInEditMode]
#endif

public class Boss : BrickBase {
	public GameObject fallingFreezePrefab;

	bool canDropFreezePower;

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

	public override void UpdateAmountOfHitsLeftDisplay() {
//		base.UpdateAmountOfHitsLeftDisplay();
		mutltiHitSpritesReference.gameObject.SetActive(false);
	}
	
	public override void SetupFallingPointObject(FallingPoints _fallingPointObject) {
//		Debug.Log("SetupFallingPointObject");
	}
	
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
	
	public override void ResetBrick() {
		fallingFreezeReference.Disable();
		freezeDropTriggerCount=0;
		base.ResetBrick();
		Messenger.Broadcast(GlobalEvents.DisplayBossHealthBar);
	}
	
	protected override void StartItemFallingFromDestroyedBrick() {
		
	}
	
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
