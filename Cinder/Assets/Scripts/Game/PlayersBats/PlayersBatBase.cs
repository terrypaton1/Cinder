using UnityEngine;

public class PlayersBatBase : BaseObject
{
    [SerializeField]
    protected Animator batAnimator;

    [SerializeField]
    public Rigidbody2D rigidRef;

    [SerializeField]
    protected Collider2D[] colliders;

    [SerializeField]
    protected SpriteRenderer[] spriteRenderers;

    private bool batIsEnabled;

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(CollisionTags.Ball))
        {
            SpawnParticles(ParticleTypes.BallHitsBat, collision.contacts[0].point);
            BatReactsToBallTouchAnimation();
        }

        if (collision.gameObject.CompareTag(CollisionTags.FallingPowerUp) ||
            collision.gameObject.CompareTag(CollisionTags.FallingPoints))
        {
            BatReactsToObjectTouchAnimation();
            FallingObjectTouched(collision);
        }
    }

    private void FallingObjectTouched(Collision2D collision)
    {
        var falling = collision.gameObject.GetComponent<FallingBase>();
        if (collision.contacts.Length > 0)
        {
            SpawnParticles(ParticleTypes.FallingPointsCollected, collision.contacts[0].point);
        }

        falling.HitPlayersBat();
    }

    private void BatReactsToBallTouchAnimation()
    {
        batAnimator.Play(Constants.BallTouch, 0, 0.0f);
    }

    private void BatReactsToObjectTouchAnimation()
    {
        batAnimator.Play(Constants.ObjectTouch, 0, 0.0f);
    }

    public void MorphToPlayingState()
    {
        batAnimator.Play(Constants.Intro, 0, 0.0f);
    }

    public void MorphToNormal()
    {
        batAnimator.Play(Constants.ToNormal, 0, 0.0f);
    }

    public void PlayerLosesLife()
    {
        batAnimator.Play(Constants.LosesLife, 0, 0.0f);
    }

    public virtual void EnableBat()
    {
        batIsEnabled = true;
        EnablePhysics();
        EnableVisuals();
    }

    public virtual void DisableBat()
    {
        batIsEnabled = false;
        DisablePhysics();
        DisableVisuals();
    }

    private void Update()
    {
        if (!batIsEnabled)
        {
            return;
        }

        UpdateLoop();
    }

    protected virtual void UpdateLoop()
    {
    }

    private void DisablePhysics()
    {
        rigidRef.simulated = false;
        foreach (var colliderRef in colliders)
        {
            colliderRef.enabled = false;
        }
    }

    private void EnablePhysics()
    {
        rigidRef.simulated = true;
        foreach (var colliderRef in colliders)
        {
            colliderRef.enabled = true;
        }
    }

    private void DisableVisuals()
    {
        foreach (var sprite in spriteRenderers)
        {
            sprite.enabled = false;
        }
    }

    private void EnableVisuals()
    {
        foreach (var sprite in spriteRenderers)
        {
            sprite.enabled = true;
        }
    }
}