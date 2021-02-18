using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class BrickBase : BaseObject
{
    [SerializeField]
    protected Collider2D colliderRef;

    [SerializeField]
    protected SpriteRenderer sprite;

    [SerializeField]
    public GameObject visualObjects;

    [HideInInspector]
    public int resetHitsToDestroyCount = 1;

    [Range(1, 50)]
    public int amountOfHitsToDestroy = 1;

    protected Vector3 visualScale = Vector3.one;
    protected int brickPointsValue = 10;

    protected float targetScale = 1.0f;
    private Vector3 scale = Vector3.one;
    private float delayCounter;
    private IEnumerator coroutine;

    public bool BrickHasBeenDestroyed
    {
        get;
        set;
    }

    public virtual void Hide()
    {
        DisableColliders();
        DisableVisuals();
        StopRunningCoroutine();
    }

    public virtual void ResetBrick()
    {
        BrickHasBeenDestroyed = false;
        amountOfHitsToDestroy = resetHitsToDestroyCount;
        UpdateAmountOfHitsLeftDisplay();
        visualObjects.transform.localPosition = Vector3.zero;

        SetVisualScale(0.05f);
        EnableColliders();
        ApplyNormalLayers();
        delayCounter = Random.Range(0.0f, 0.5f);

        StopRunningCoroutine();
        coroutine = StartSequence();
        StartCoroutine(coroutine);
    }

    private void StopRunningCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(delayCounter);

        // set everything tiny
        SetVisualScale(0.01f);
        EnableVisuals();
        
        targetScale = 1.0f;

        var timePassed = 0.0f;
        var timeToTake = 0.5f;

        while (timePassed < timeToTake)
        {
            var percent = timePassed / timeToTake;
            timePassed += Time.deltaTime;

            var scaleVal = CoreConnector.GameManager.gameSettings.scaleInCurve.Evaluate(percent);
            SetVisualScale(scaleVal);
            yield return null;
        }
    }

    private void SetVisualScale(float scaleValue)
    {
        scale.x = scale.y = scaleValue;
        visualObjects.transform.localScale = scale;
    }

    public void DisableFlameBall()
    {
        ApplyNormalLayers();
    }

    public void ActivateFlameBall()
    {
        gameObject.layer = CoreConnector.GameManager.brickManager.bricksLayerDuringFlameBall;
        colliderRef.gameObject.layer = CoreConnector.GameManager.brickManager.bricksLayerDuringFlameBall;
    }

    private void ApplyNormalLayers()
    {
//		Debug.Log("ApplyNormalLayers on bricks");
        if (!Application.isPlaying)
        {
            return;
        }

        gameObject.layer = CoreConnector.GameManager.brickManager.bricksLayerNormal;
        colliderRef.gameObject.layer = CoreConnector.GameManager.brickManager.bricksLayerNormal;
    }

    public void AffectedByTNTExplosion()
    {
        if (BrickHasBeenDestroyed)
        {
            return;
        }

        amountOfHitsToDestroy--;
        if (amountOfHitsToDestroy < 1)
        {
            StartCoroutine(DestroyBrickSequence(false));
        }
        else
        {
            StartShake();
        }

        UpdateAmountOfHitsLeftDisplay();
    }

    private void StartShake()
    {
        StopRunningCoroutine();
        coroutine = ShakeBrick();
        StartCoroutine(coroutine);
    }

    protected void DisableColliders()
    {
        colliderRef.enabled = false;
    }

    protected void EnableColliders()
    {
        colliderRef.gameObject.isStatic = true;
        colliderRef.enabled = true;
    }

    public virtual void UpdateAmountOfHitsLeftDisplay()
    {
    }

    public virtual void BrickHitByBall()
    {
        // if brick is being destroyed already , then don't run any of this

        amountOfHitsToDestroy--;
        if (amountOfHitsToDestroy < 1)
        {
            StartCoroutine(DestroyBrickSequence());
        }
        else
        {
            // brick has taken a hit, shake its visuals!
            StartShake();
        }

        UpdateAmountOfHitsLeftDisplay();
    }

    private IEnumerator ShakeBrick()
    {
        // shake the visual element of this brick
        var count = 10;
        while (count > 0)
        {
            visualObjects.transform.localPosition = new Vector2(
                Random.Range(-GameVariables.brickShakeAmount, GameVariables.brickShakeAmount),
                Random.Range(-GameVariables.brickShakeAmount, GameVariables.brickShakeAmount));
            yield return null;
            count--;
        }

        visualObjects.transform.localPosition = Vector2.zero;
    }

    protected virtual void StartItemFallingFromDestroyedBrick()
    {
    }

    protected virtual IEnumerator DestroyBrickSequence(bool playSound = true)
    {
        StartItemFallingFromDestroyedBrick();
        BrickHasBeenDestroyed = true;

        CoreConnector.GameManager.scoreManager.PointsCollected(Points.brickPointsValue);
        CoreConnector.GameManager.brickManager.BrickDestroyed(this);

        SpawnParticles(ParticleTypes.BrickExplosion, transform.position);
        if (playSound)
        {
            PlaySound(SoundList.brickDestroyed);
        }

        yield return null;

        DisableColliders();

        targetScale = 0.05f;

        var timePassed = 0.0f;
        const float timeToTake = 0.35f;

        while (timePassed < timeToTake)
        {
            var percent = timePassed / timeToTake;
            timePassed += Time.deltaTime;
            var scaleValue = 1 - percent;
            var scaleVal = scaleValue * targetScale;
            SetVisualScale(scaleVal);
            yield return null;
        }

        DisableVisuals();
    }

    public virtual void Shake(float amount)
    {
    }

    public virtual void UpdateLoop()
    {
    }

    protected virtual void DisableVisuals()
    {
        sprite.enabled = false;
    }

    public void EnableVisuals()
    {
        sprite.enabled = true;
    }
}