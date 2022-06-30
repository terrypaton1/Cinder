using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class BrickBase : BaseObject
{
    [SerializeField]
    public BrickType brickType;

    [SerializeField]
    protected Collider2D colliderRef;

    [SerializeField]
    protected List<SpriteRenderer> sprites;

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

    private const float destroyedBrickScale = 0.05f;
    private const float destroyBrickTimeToTake = 0.35f;

    public bool BrickHasBeenDestroyed { get; protected set; }

    protected void OnValidate()
    {
        if (sprites == null || sprites.Count == 0)
        {
            var foundSprite = visualObjects.GetComponentsInChildren<SpriteRenderer>();
            sprites = foundSprite.ToList();
        }

        TestForBackgroundSprite();
    }

    private void TestForBackgroundSprite()
    {
        if (sprites.Count == 1)
        {
            var originalSprite = sprites[0];
            if (originalSprite == null)
            {
                Debug.Log($"Missing sprite for:{gameObject.name}");
                return;
            }

            var newSprite = Instantiate(originalSprite, originalSprite.transform.parent);
            var position = originalSprite.transform.localPosition;
            position.z = 0.5f;
            newSprite.transform.localPosition = position;
            newSprite.transform.localScale = originalSprite.transform.localScale * 1.3f;
            newSprite.color = Color.black;
            sprites.Add(newSprite);
        }
    }

    public virtual void Hide()
    {
        DisableColliders();
        DisableVisuals();
        StopRunningCoroutine();
    }

    public virtual void Show()
    {
        ResetBrick();
    }

    public virtual void ResetBrick()
    {
        visualObjects.transform.localEulerAngles = Vector3.zero;
        BrickHasBeenDestroyed = false;
        amountOfHitsToDestroy = resetHitsToDestroyCount;
        UpdateAmountOfHitsLeftDisplay();

        visualObjects.transform.localPosition = Vector3.zero;

        EnableVisuals();
        if (!Application.isPlaying)
        {
            SetVisualScale(1.0f);
            return;
        }

        SetVisualScale(destroyedBrickScale);
        EnableColliders();

        ApplyNormalLayers();
        delayCounter = Random.Range(0.1f, 0.4f);

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
        DisableVisuals();
        SetAlpha(0.0f);
        LeanTween.cancel(visualObjects.gameObject);
        yield return WaitCache.WaitForSeconds(0.5f);
        var randomScale = Random.Range(0.2f, 2.0f);
        SetVisualScale(randomScale);
        EnableVisuals();

        // scale the brick up from tiny. 

        targetScale = 1.0f;

        var timeToTake = Random.Range(0.3f, 0.5f);
        LeanTween.value(0.0f, 1.0f, timeToTake).setOnUpdate(SetPercent)
            .setEase(LeanTweenType.easeOutBack)
            .setDelay(delayCounter);

        yield return WaitCache.WaitForSeconds(timeToTake);
    }

    private void SetPercent(float percent)
    {
        SetAlpha(percent);
        SetVisualScale(percent);
    }

    private void SetVisualScale(float scalePercent)
    {
        scale.x = scale.y = scalePercent * targetScale;
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

    protected virtual void ApplyNormalLayers()
    {
        if (!Application.isPlaying)
        {
            Debug.Log("ApplyNormalLayers");
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
            StopRunningCoroutine();
            coroutine = DestroyBrickSequence(false);
            StartCoroutine(coroutine);
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
                Random.Range(-GameVariables.BrickShakeAmount, GameVariables.BrickShakeAmount),
                Random.Range(-GameVariables.BrickShakeAmount, GameVariables.BrickShakeAmount));
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
        BrickHasBeenDestroyed = true;
        StartItemFallingFromDestroyedBrick();

        CoreConnector.GameManager.scoreManager.PointsCollected(Points.brickPointsValue);
        CoreConnector.GameManager.brickManager.BrickDestroyed();

        SpawnParticles(ParticleTypes.BrickExplosion, transform.position);

        targetScale = destroyedBrickScale;
        LeanTween.cancel(visualObjects.gameObject);
        LeanTween.scale(visualObjects.gameObject, Vector3.one * destroyedBrickScale, destroyBrickTimeToTake)
            .setEase(LeanTweenType.linear);

        if (playSound)
        {
            PlaySound(SoundList.brickDestroyed);
        }

        yield return null;

        DisableColliders();

        yield return new WaitForSeconds(destroyBrickTimeToTake);

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
        foreach (var sprite in sprites)
        {
            sprite.enabled = false;
        }
    }

    public virtual void EnableVisuals()
    {
        foreach (var sprite in sprites)
        {
            sprite.enabled = true;
        }
    }

    public void ApplyColor(Color color)
    {
        if (sprites == null)
        {
            return;
        }

        var sprite = sprites[0];
        sprite.color = color;
    }

    private void SetAlpha(float value)
    {
        var sprite = sprites[0];

        var color = sprite.color;
        color.a = value;
        ApplyColor(color);
    }
}