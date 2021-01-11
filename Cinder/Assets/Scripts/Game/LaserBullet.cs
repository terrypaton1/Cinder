﻿using UnityEngine;

public class LaserBullet : BaseObject
{
    private float currentLaserSpeed;
    private float laserMaxSpeed = 1;
    public bool isUsed;

    [SerializeField]
    protected Rigidbody2D thisRigidbody;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private BoxCollider2D boxCollider;

    public void Launch(Vector3 velocity)
    {
        isUsed = true;

        EnableVisuals();
        EnableCollider();

        laserMaxSpeed = GameVariables.laserBulletSpeed;
        thisRigidbody.velocity = velocity;
    }

    private void EnableCollider()
    {
        boxCollider.enabled = true;
    }

    private void DisableCollider()
    {
        boxCollider.enabled = true;
    }

    private void DisableVisuals()
    {
        spriteRenderer.enabled = false;
    }

    private void EnableVisuals()
    {
        spriteRenderer.enabled = true;
    }

    private void HitABrick()
    {
        PlaySound(SoundList.LaserBulletHitsBrick);
        RePoolObject();
    }

    protected void FixedUpdate()
    {
        // check if the y velocity ever reaches zero
        // check the speed of the ball and make it always locked

        currentLaserSpeed = Mathf.Lerp(currentLaserSpeed, laserMaxSpeed, Time.deltaTime * 7.5f);
        var velocity = thisRigidbody.velocity;
        if (velocity.magnitude < currentLaserSpeed)
        {
            thisRigidbody.velocity = velocity.normalized * currentLaserSpeed;
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        var position = collision.contacts[0].point;
        SpawnParticles(ParticleTypes.LaserHitsBrick, position);

        if (collision.gameObject.CompareTag(CollisionTags.Brick))
        {
            HitABrick();
            // todo this could do with optimizing
            var _brick = collision.gameObject.GetComponent<BrickBase>();
            _brick.BrickHitByBall();
            RePoolObject();
        }
        else
        {
            PlaySound(SoundList.LaserBulletHitsWall);
            RePoolObject();
        }
    }

    public void Disable()
    {
        DisableVisuals();
        DisableCollider();
        isUsed = false;
    }

    private void RePoolObject()
    {
        Disable();
    }
}