using UnityEngine;

public class NonBrick : BaseObject
{
    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    protected Collider2D colliderRef;

    public void Show()
    {
        EnableVisuals();
        EnableCollisions();
    }

    public void Hide()
    {
        DisableCollisions();
        DisableVisuals();
    }

    private void DisableVisuals()
    {
        spriteRenderer.enabled = false;
    }

    private void EnableVisuals()
    {
        spriteRenderer.enabled = true;
    }

    protected virtual void DisableCollisions()
    {
        colliderRef.enabled = false;
    }

    protected virtual void EnableCollisions()
    {
        colliderRef.enabled = true;
    }
}