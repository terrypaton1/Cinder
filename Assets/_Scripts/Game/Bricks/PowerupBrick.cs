using UnityEngine;

public enum PowerupType
{
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

[ExecuteInEditMode]
[SelectionBase]
public class PowerupBrick : BrickBase
{
    public PowerupType[] randomPowerupChoices;

    public PowerupType typeOfPowerUp;

    public GameObject fallingPowerUpPrefab;

    [SerializeField]
    protected Sprite powerupWide;

    [SerializeField]
    protected Sprite powerupMultiball;

    [SerializeField]
    protected Sprite powerupSmallBat;

    [SerializeField]
    protected Sprite powerupCrazyBall;

    [SerializeField]
    protected Sprite powerupLaser;

    [SerializeField]
    protected Sprite powerupShield;

    [SerializeField]
    protected Sprite powerupSplitBat;

    [SerializeField]
    protected Sprite powerupRandom;

    [SerializeField]
    protected Sprite powerupFlameball;

    [SerializeField]
    protected SpriteRenderer _spriteRenderer;

    FallingPowerup fallingPowerUp;

    protected void Awake()
    {
        _brickAnimation = GetComponent<Animator>();
    }

    protected void OnDestroy()
    {
        // destroy the falling powerup too!
//		Debug.Log("destroy the falling powerup too!");
        if (fallingPowerUp != null)
            Destroy(fallingPowerUp.gameObject);
    }

    protected void OnGUI()
    {
        var name = "PowerUp" + typeOfPowerUp;
        if (gameObject.name != name)
        {
            gameObject.name = name;
        }

        EvaluateDisplay();
    }

    public void EvaluateDisplay()
    {
        switch (typeOfPowerUp)
        {
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

    public override void UpdateAmountOfHitsLeftDisplay()
    {
        // powerup brick doesn't use this
    }

    public override void ResetBrick()
    {
        fallingPowerUp.DisableFallingPowerup();
        base.ResetBrick();
    }

    protected override void StartItemFallingFromDestroyedBrick()
    {
        fallingPowerUp.StartFalling(transform.position);
    }

    override protected void Startup()
    {
        SetupLayers();
        // create a powerup ready for when the brick is destroyed
        var fallingPowerupReference = Instantiate(fallingPowerUpPrefab);

        fallingPowerupReference.transform.parent = transform.parent;
        fallingPowerupReference.transform.position = new Vector3(8, 0, 0);
        fallingPowerUp = fallingPowerupReference.GetComponent<FallingPowerup>();

        if (typeOfPowerUp == PowerupType.Random)
        {
            // choose randomly from randomPowerupChoices
            if (randomPowerupChoices.Length == 0)
            {
                Debug.LogError("ERROR: No random choices have been set");
            }

            var randomPowerUpNum = Random.Range(0, randomPowerupChoices.Length);
            var randomTypeOfPowerUp = randomPowerupChoices[randomPowerUpNum];
//			Debug.Log("random randomTypeOfPowerUp:" + randomTypeOfPowerUp);
            fallingPowerUp.Setup(randomTypeOfPowerUp);
        }
        else
        {
            fallingPowerUp.Setup(typeOfPowerUp);
        }

        fallingPowerUp.DisableFallingPowerup();
        BrickManager.instance.RegisterBrick(this, false);
        colliders = GetComponentsInChildren<Collider2D>();
//		Debug.Log("colliders:" + colliders.Length);
        brickPointsValue = 50;
//		Debug.Log("setup brick points value");
        if (colliders.Length == 0)
        {
            Debug.LogError("Didn't find the colliders!");
        }

        UpdateAmountOfHitsLeftDisplay();
        EvaluateDisplay();
        _brickAnimation.Play("BrickStartingState");
        StartCoroutine(RevealBrick());
    }
}