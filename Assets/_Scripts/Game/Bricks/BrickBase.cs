#region

using System.Collections;
using UnityEngine;

#endregion

public class BrickBase : BaseObject
{
    /// <summary>
    /// The mutlti hit sprites reference.
    /// </summary>
    [SerializeField]
    protected BrickMutltiHitSprites mutltiHitSpritesReference;

    /// <summary>
    /// The amount to reset the hit counter back to on restart
    /// </summary>
    [HideInInspector]
    public int resetHitsToDestroyCount = 1;

    [Range(1, 50)]
    public int amountOfHitsToDestroy = 1;

    public GameObject visualObjects;

    /// <summary>
    /// The brick animation.
    /// </summary>
    public Animator _brickAnimation;

    protected bool brickHasBeenDestroyed;

    /// <summary>
    /// The brick points value.
    /// </summary>
    protected int brickPointsValue = 10;

    int bricksLayerDuringFlameBall;

    int bricksLayerNormal;

    /// <summary>
    /// The colliders.
    /// </summary>
    protected Collider2D[] colliders;

    protected Vector2 visualScale = Vector2.one;

    /// <summary>
    /// Gets a value indicating whether this <see cref="BrickBase"/> brick has been destroyed.
    /// </summary>
    /// <value><c>true</c> if brick has been destroyed; otherwise, <c>false</c>.</value>
    public bool BrickHasBeenDestroyed => brickHasBeenDestroyed;

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        StartCoroutine(TestStartingUpGame());
    }

    void OnEnable()
    {
        Messenger.AddListener(GlobalEvents.ActivateFlameBall, ActivateFlameBall);
        Messenger.AddListener(GlobalEvents.DisableFlameBall, ApplyNormalLayers);
    }

    void OnDisable()
    {
        Messenger.RemoveListener(GlobalEvents.ActivateFlameBall, ActivateFlameBall);
        Messenger.RemoveListener(GlobalEvents.DisableFlameBall, ApplyNormalLayers);
    }

    /// <summary>
    /// Raises the collision enter2D event.
    /// </summary>
    /// <param name="coll">Coll.</param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (brickHasBeenDestroyed)
            return;
//		Debug.Log(coll.gameObject.tag);
        if (collision.gameObject.CompareTag("Ball"))
        {
            Messenger<ParticleTypes, Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.BallHitsBrick,
                collision.contacts[0].point, MessengerMode.DONT_REQUIRE_LISTENER);

            BrickHitByBall();
            // tell the ball you hit me
            var _ball = collision.gameObject.GetComponent<Ball>();
            _ball.HitABrick();
        }

        if (collision.gameObject.CompareTag("LaserBullet"))
        {
            Messenger<ParticleTypes, Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.LaserHitsBrick,
                collision.contacts[0].point, MessengerMode.DONT_REQUIRE_LISTENER);
            Debug.Log("amountOfHitsToDestroy:" + amountOfHitsToDestroy);
            BrickHitByBall();
            // tell the ball you hit me
            var _laserBullet = collision.gameObject.GetComponent<LaserBullet>();
            _laserBullet.HitABrick(collision.contacts[0].point);
        }
    }

    /// <summary>
    /// Sets up the layers.
    /// </summary>
    protected void SetupLayers()
    {
//		Debug.Log("SetupLayers");
        bricksLayerDuringFlameBall = LayerMask.NameToLayer("BricksDuringFlameBall");
//		Debug.Log("BricksDuringFlameBall:" + bricksLayerDuringFlameBall);
        bricksLayerNormal = LayerMask.NameToLayer("Bricks");
    }

    /// <summary>
    /// Setups the falling point object.
    /// </summary>
    /// <param name="_fallingPointObject">Falling point object.</param>
    public virtual void SetupFallingPointObject(FallingPoints _fallingPointObject)
    {
//	
    }

    /// <summary>
    /// Resets the brick.
    /// </summary>
    public virtual void ResetBrick()
    {
        brickHasBeenDestroyed = false;
        amountOfHitsToDestroy = resetHitsToDestroyCount;
        UpdateAmountOfHitsLeftDisplay();
        visualObjects.transform.localPosition = Vector3.zero;
        visualScale = Vector3.one;
        visualObjects.transform.localScale = visualScale;
        visualObjects.SetActive(true);
        EnableColliders();
        ApplyNormalLayers();
        _brickAnimation.Play("BrickStartingState");
        StartCoroutine(RevealBrick());
    }

    protected IEnumerator RevealBrick()
    {
        var randomTime = Random.Range(0f, .3f);
//		Debug.Log("randomTime:" + randomTime);
        yield return new WaitForSeconds(randomTime);
        _brickAnimation.Play("BrickRevealAnimation");
    }

    protected void ActivateFlameBall()
    {
//		Debug.Log("ActivateFlameBall on bricks");
        // change the layers

        gameObject.layer = bricksLayerDuringFlameBall;
        foreach (var colliderReference in colliders)
        {
            colliderReference.gameObject.layer = bricksLayerDuringFlameBall;
        }
    }

    protected void ApplyNormalLayers()
    {
//		Debug.Log("ApplyNormalLayers on bricks");
        gameObject.layer = bricksLayerNormal;
        foreach (var colliderReference in colliders)
        {
            colliderReference.gameObject.layer = bricksLayerNormal;
        }
    }

    public void AffectedByTNTExplosion()
    {
//		Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.BallHitsBrick, collision.contacts [0].point, MessengerMode.DONT_REQUIRE_LISTENER);
// check that its not being killed already
        if (brickHasBeenDestroyed) return; // brick has already been destroyed
        amountOfHitsToDestroy--;
        if (amountOfHitsToDestroy < 1)
        {
            StartCoroutine(DestroyBrickSequence(false));
        }
        else
        {
// brick has taken a hit, shake its visuals!
            StopCoroutine(ShakeBrick());
            StartCoroutine(ShakeBrick());
        }

        UpdateAmountOfHitsLeftDisplay();
    }

    /// <summary>
    /// Disables the colliders.
    /// </summary>
    protected void DisableColliders()
    {
        if (colliders == null)
            return;
        foreach (var colliderReference in colliders)
            colliderReference.enabled = false;
    }

    IEnumerator TestStartingUpGame()
    {
        while (BrickManager.instance == null)
        {
//			Debug.Log("cannot find game");
            yield return new WaitForSeconds(.5f);
        }

        Startup();
    }

    /// <summary>
    /// Enables the colliders.
    /// </summary>
    protected void EnableColliders()
    {
        foreach (var colliderReference in colliders)
            colliderReference.enabled = true;
    }

    /// <summary>
    /// Updates the amount of hits left display.
    /// </summary>
    public virtual void UpdateAmountOfHitsLeftDisplay()
    {
        if (amountOfHitsToDestroy < 2)
        {
            mutltiHitSpritesReference.gameObject.SetActive(false);
            return;
        }

        mutltiHitSpritesReference.gameObject.SetActive(true);
        mutltiHitSpritesReference.DisplayHitsLeft(amountOfHitsToDestroy);
    }

    /// <summary>
    /// Startup this instance.
    /// </summary>
    protected virtual void Startup()
    {
        SetupLayers();
        resetHitsToDestroyCount = amountOfHitsToDestroy;
        BrickManager.instance.RegisterBrick(this, true);

        colliders = GetComponentsInChildren<Collider2D>();
        ApplyNormalLayers();
//		Debug.Log("colliders:" + colliders.Length);
        brickPointsValue = 10;
//		Debug.Log("setup brick points value");
        if (colliders.Length == 0)
        {
            Debug.LogError("Didn't find the colliders!");
        }

        UpdateAmountOfHitsLeftDisplay();
        _brickAnimation.Play("BrickStartingState");
        StartCoroutine(RevealBrick());
    }

    /// <summary>
    /// Brick has been hit by a ball.
    /// </summary>
    public virtual void BrickHitByBall()
    {
        amountOfHitsToDestroy--;
        if (amountOfHitsToDestroy < 1)
        {
            StartCoroutine(DestroyBrickSequence());
        }
        else
        {
// brick has taken a hit, shake its visuals!
            StopCoroutine(ShakeBrick());
            StartCoroutine(ShakeBrick());
        }

        UpdateAmountOfHitsLeftDisplay();
    }

    /// <summary>
    /// Shakes the brick.
    /// </summary>
    private IEnumerator ShakeBrick()
    {
        // shake the visual element of this brick
        var count = 10;
        while (count > 0)
        {
            count--;
//			Debug.Log("Shake it");
            visualObjects.transform.localPosition = new Vector2(
                Random.Range(-GameVariables.brickShakeAmount, GameVariables.brickShakeAmount),
                Random.Range(-GameVariables.brickShakeAmount, GameVariables.brickShakeAmount));
            yield return 0;
        }

        visualObjects.transform.localPosition = Vector2.zero;
//		yield return 0;
    }

    /// <summary>
    /// Starts the item falling from destroyed brick.
    /// </summary>
    protected virtual void StartItemFallingFromDestroyedBrick()
    {
    }

    /// <summary>
    /// Destroys the brick.
    /// </summary>
    /// <returns>The brick.</returns>
    public virtual IEnumerator DestroyBrickSequence(bool playSound = true)
    {
        _brickAnimation.Play("BrickDestroyed");
        StartItemFallingFromDestroyedBrick();
        brickHasBeenDestroyed = true;

        Messenger<int>.Broadcast(GlobalEvents.PointsCollected, GameVariables.brickPointsValue,
            MessengerMode.DONT_REQUIRE_LISTENER);
        Messenger<ParticleTypes, Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.BrickExplosion,
            transform.position, MessengerMode.DONT_REQUIRE_LISTENER);
        if (playSound)
        {
            PlaySound(SoundList.brickDestroyed);
        }

        yield return 0;
        DisableColliders();
        while (visualScale.x > 0.1f)
        {
            visualScale.x *= .8f;
            visualScale.y *= .8f;
            visualObjects.transform.localScale = visualScale;
            yield return 0;
        }

        BrickManager.instance.BrickDestroyed(this);
        visualObjects.SetActive(false);
    }
}