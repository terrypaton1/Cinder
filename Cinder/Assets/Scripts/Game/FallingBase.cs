using UnityEngine;

public class FallingBase : BaseObject
{
    [SerializeField]
    protected Collider2D colliderRef;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    protected Rigidbody2D rigid2D;

    private Transform objectTransform;
    public bool isFalling;

    public override void LifeLost()
    {
        if (!isFalling)
        {
            return;
        }

        SpawnParticles(ParticleTypes.DestroyFallingItems, transform.position);
        Disable();
    }

    public override void LevelComplete()
    {
        SpawnParticles(ParticleTypes.DestroyFallingItems, transform.position);
        Disable();
    }

    public virtual void Setup()
    {
        objectTransform = transform;
    }

    public virtual void StartFalling(Vector3 position)
    {
        // start falling, slowly at first, then faster
        objectTransform.position = position;
        objectTransform.localEulerAngles = Vector3.zero;
        rigid2D.isKinematic = false;
        EnableVisuals();
        colliderRef.enabled = true;
        rigid2D.simulated = true;
        rigid2D.angularVelocity = 0.0f;
        rigid2D.velocity = Vector3.zero;
        isFalling = true;
        colliderRef.gameObject.isStatic = false;
        // use a coroutine to move the object
        // rigid2D.AddForce(Vector2.down * 4.0f, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(CollisionTags.DeadZone) ||
            collision.gameObject.CompareTag(CollisionTags.Shield))
        {
            FellInToDeadZone();
        }
    }

    public virtual void Disable()
    {
        rigid2D.isKinematic = true;
        isFalling = false;
        colliderRef.enabled = false;
        DisableVisuals();
        rigid2D.simulated = false;
        rigid2D.angularVelocity = 0.0f;
    }

    public void DisableVisuals()
    {
        spriteRenderer.enabled = false;
    }

    private void EnableVisuals()
    {
        spriteRenderer.enabled = true;
    }

    public virtual void HitPlayersBat()
    {
        Disable();
    }

    protected virtual void FellInToDeadZone()
    {
        var powerUpLostEffectPosition = objectTransform.position;
        powerUpLostEffectPosition.y = 0;
        SpawnParticles(ParticleTypes.PowerupLost, powerUpLostEffectPosition);
        Disable();
    }
}