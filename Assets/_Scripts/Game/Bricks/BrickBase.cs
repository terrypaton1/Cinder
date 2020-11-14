using System.Collections;
using UnityEngine;

public class BrickBase : BaseObject
{
    [SerializeField]
    protected BrickMutltiHitSprites mutltiHitSpritesReference;

    [HideInInspector]
    public int resetHitsToDestroyCount = 1;

    [Range(1, 50)]
    public int amountOfHitsToDestroy = 1;

    public GameObject visualObjects;

    public Animator _brickAnimation;

    protected bool brickHasBeenDestroyed;

    protected int brickPointsValue = 10;

    private int bricksLayerDuringFlameBall;

    private int bricksLayerNormal;

    protected Collider2D[] colliders;

    protected Vector2 visualScale = Vector2.one;

    public bool BrickHasBeenDestroyed => brickHasBeenDestroyed;

    protected void Start()
    {
        StartCoroutine(TestStartingUpGame());
    }

    protected void OnEnable()
    {
        Messenger.AddListener(GlobalEvents.ActivateFlameBall, ActivateFlameBall);
        Messenger.AddListener(GlobalEvents.DisableFlameBall, ApplyNormalLayers);
    }

    protected void OnDisable()
    {
        Messenger.RemoveListener(GlobalEvents.ActivateFlameBall, ActivateFlameBall);
        Messenger.RemoveListener(GlobalEvents.DisableFlameBall, ApplyNormalLayers);
    }

      protected override void CollisionEnterCode(Collision2D collision)
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

    protected void SetupLayers()
    {
//		Debug.Log("SetupLayers");
        bricksLayerDuringFlameBall = LayerMask.NameToLayer("BricksDuringFlameBall");
//		Debug.Log("BricksDuringFlameBall:" + bricksLayerDuringFlameBall);
        bricksLayerNormal = LayerMask.NameToLayer("Bricks");
    }

    public virtual void SetupFallingPointObject(FallingPoints _fallingPointObject)
    {
//	empty
    }

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

    protected void EnableColliders()
    {
        foreach (var colliderReference in colliders)
            colliderReference.enabled = true;
    }

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

    protected virtual void StartItemFallingFromDestroyedBrick()
    {
    }

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