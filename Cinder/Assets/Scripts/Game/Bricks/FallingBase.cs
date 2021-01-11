using UnityEngine;

public class FallingBase : BaseObject
{
    [SerializeField]
    protected Collider2D colliderRef;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    protected Rigidbody2D rigid2D;

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

    public void StartFalling(Vector3 position)
    {
        // start falling, slowly at first, then faster
        var transform1 = transform;
        transform1.position = position;
        transform1.localEulerAngles = Vector3.zero;

        EnableVisuals();

        colliderRef.enabled = true;
        colliderRef.gameObject.isStatic = false;

        rigid2D.isKinematic = false;
        rigid2D.simulated = true;
        rigid2D.angularVelocity = 0.0f;
        rigid2D.velocity = Vector3.zero;

        isFalling = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(CollisionTags.DeadZone) ||
            collision.gameObject.CompareTag(CollisionTags.Shield))
        {
            FellInToDeadZone();
        }
    }

    public void Disable()
    {
        rigid2D.isKinematic = true;
        isFalling = false;
        colliderRef.enabled = false;
        DisableVisuals();
        rigid2D.simulated = false;
        rigid2D.angularVelocity = 0.0f;
    }

    private void DisableVisuals()
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
        var powerUpLostEffectPosition = transform.position;
        powerUpLostEffectPosition.y = 0;
        SpawnParticles(ParticleTypes.PowerUpLost, powerUpLostEffectPosition);
        Disable();
    }
}