﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator)), SelectionBase]
public class PlayersBatBase : BaseObject
{
    [SerializeField]
    protected Animator MorphToPlayingAnimation;

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
        }

        if (collision.gameObject.CompareTag(CollisionTags.FallingPowerUp))
        {
            FallingObjectTouched(collision);
        }

        if (collision.gameObject.CompareTag(CollisionTags.FallingPoints))
        {
            FallingObjectTouched(collision);
        }
    }

    private static void FallingObjectTouched(Collision2D collision)
    {
        var falling = collision.gameObject.GetComponent<FallingBase>();
        if (collision.contacts.Length > 0)
        {
            SpawnParticles(ParticleTypes.FallingPointsCollected, collision.contacts[0].point);
        }

        falling.HitPlayersBat();
    }

    public void MorphToPlayingState()
    {
        MorphToPlayingAnimation.Play(Constants.Intro);
    }

    public void MorphToNormal()
    {
        MorphToPlayingAnimation.Play(Constants.ToNormal);
    }

    public void PlayerLosesLife()
    {
        MorphToPlayingAnimation.Play(Constants.LosesLife);
    }

    public virtual void EnableBat()
    {
        batIsEnabled = true;
        EnableColliders();
        EnableVisuals();
    }

    public virtual void DisableBat()
    {
        batIsEnabled = false;
        DisableColliders();
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

    private void DisableColliders()
    {
        foreach (var colliderRef in colliders)
        {
            colliderRef.enabled = false;
        }
    }

    private void EnableColliders()
    {
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