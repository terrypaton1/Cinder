#region

using UnityEngine;

#endregion

/// <summary>
/// Powerup type.
/// </summary>
public enum PowerupType {
	Multiball = 0,
	SmallBat = 10,
	WideBat = 20,
	Flameball = 40,
	LaserBat = 50,
	SplitBat = 60,
	CrazyBall = 70,
	Shield = 80,
	Random = 90,
	FreezePlayer = 100
}

[ExecuteInEditMode][SelectionBase]
public class PowerupBrick : BrickBase {
	/// <summary>
	/// The random powerup choices.
	/// </summary>
	public PowerupType[] randomPowerupChoices;

	/// <summary>
	/// The type of power up.
	/// </summary>
	public PowerupType typeOfPowerUp;

	/// <summary>
	/// The falling power up prefab.
	/// </summary>
	public GameObject fallingPowerUpPrefab;

	/// <summary>
	/// The powerup wide.
	/// </summary>
	[SerializeField] Sprite powerupWide;

	/// <summary>
	/// The powerup multiball.
	/// </summary>
	[SerializeField] Sprite powerupMultiball;

	/// <summary>
	/// The powerup small bat.
	/// </summary>
	[SerializeField] Sprite powerupSmallBat;

	/// <summary>
	/// The powerup crazy ball.
	/// </summary>
	[SerializeField] Sprite powerupCrazyBall;

	/// <summary>
	/// The powerup laser.
	/// </summary>
	[SerializeField] Sprite powerupLaser;

	/// <summary>
	/// The powerup shield.
	/// </summary>
	[SerializeField] Sprite powerupShield;

	/// <summary>
	/// The powerup split bat.
	/// </summary>
	[SerializeField] Sprite powerupSplitBat;

	/// <summary>
	/// The powerup random.
	/// </summary>
	[SerializeField] Sprite powerupRandom;

	/// <summary>
	/// The powerup random.
	/// </summary>
	[SerializeField] Sprite powerupFlameball;

	/// <summary>
	/// The sprite renderer.
	/// </summary>
	[SerializeField] SpriteRenderer _spriteRenderer;

	/// <summary>
	/// The falling power up.
	/// </summary>
	FallingPowerup fallingPowerUp;

	void Awake() {
		_brickAnimation = GetComponent<Animator>();
	}

	void OnDestroy() {
		// destroy the falling powerup too!
//		Debug.Log("destroy the falling powerup too!");
		if (fallingPowerUp != null)
			Destroy(fallingPowerUp.gameObject);
	}

	/// <summary>
	/// Rename the game object so it reflects the type of power up it is
	/// </summary>
	void OnGUI() {
		var name = "PowerUp" + typeOfPowerUp;
		if (gameObject.name != name) {
			gameObject.name = name;
		}
		EvaluateDisplay();
	}

	/// <summary>
	/// Evaluates the display of the powerup.
	/// </summary>
	public void EvaluateDisplay() {
		switch (typeOfPowerUp) {
			case PowerupType.Multiball:
				_spriteRenderer.sprite = powerupMultiball;
				break;
			case PowerupType.WideBat:
				_spriteRenderer.sprite = powerupWide;
				break;
			case PowerupType.SmallBat:
				_spriteRenderer.sprite = powerupSmallBat;
				break;
			case PowerupType.CrazyBall:
				_spriteRenderer.sprite = powerupCrazyBall;
				break;
			case PowerupType.LaserBat:
				_spriteRenderer.sprite = powerupLaser;
				break;
			case PowerupType.Shield:
				_spriteRenderer.sprite = powerupShield;
				break;
			case PowerupType.SplitBat:
				_spriteRenderer.sprite = powerupSplitBat;
				break;

			case PowerupType.Random:
				_spriteRenderer.sprite = powerupRandom;
				break;
			case PowerupType.Flameball:
				_spriteRenderer.sprite = powerupFlameball;
				break;
//				
		}
	}

	/// <summary>
	/// Updates the amount of hits left display. powerup brick doesn't use this
	/// </summary>
	public override void UpdateAmountOfHitsLeftDisplay() {
		// powerup brick doesn't use this
	}

	/// <summary>
	/// Resets the brick.
	/// </summary>
	public override void ResetBrick() {
		fallingPowerUp.DisableFallingPowerup();
		base.ResetBrick();
	}

	/// <summary>
	/// Starts the item falling from destroyed brick.
	/// </summary>
	protected override void StartItemFallingFromDestroyedBrick() {
		fallingPowerUp.StartFalling(transform.position);
	}

	/// <summary>
	/// Startup this instance.
	/// </summary>
	override protected  void Startup() {
		SetupLayers();
		// create a powerup ready for when the brick is destroyed
		var fallingPowerupReference = Instantiate(fallingPowerUpPrefab);

		fallingPowerupReference.transform.parent = transform.parent;
		fallingPowerupReference.transform.position = new Vector3(8, 0, 0);
		fallingPowerUp = fallingPowerupReference.GetComponent<FallingPowerup>();

		if (typeOfPowerUp == PowerupType.Random) {
			// choose randomly from randomPowerupChoices
			if (randomPowerupChoices.Length == 0) {
				Debug.LogError("ERROR: No random choices have been set");
			}
			var randomPowerUpNum = Random.Range(0, randomPowerupChoices.Length);
			var	randomTypeOfPowerUp = randomPowerupChoices [randomPowerUpNum];
//			Debug.Log("random randomTypeOfPowerUp:" + randomTypeOfPowerUp);
			fallingPowerUp.Setup(randomTypeOfPowerUp);
		} else {
			fallingPowerUp.Setup(typeOfPowerUp);
		}

		fallingPowerUp.DisableFallingPowerup();
		BrickManager.instance.RegisterBrick(this, false);
		colliders = GetComponentsInChildren<Collider2D>();
//		Debug.Log("colliders:" + colliders.Length);
		brickPointsValue = 50;
//		Debug.Log("setup brick points value");
		if (colliders.Length == 0) {
			Debug.LogError("Didn't find the colliders!");
		}
		UpdateAmountOfHitsLeftDisplay();
		EvaluateDisplay();
		_brickAnimation.Play("BrickStartingState");
		StartCoroutine(RevealBrick());
	}
}
