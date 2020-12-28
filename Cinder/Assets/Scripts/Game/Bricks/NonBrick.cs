using System;
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

    private void DisableCollisions()
    {
        colliderRef.enabled = false;
    }

    private void EnableCollisions()
    {
        colliderRef.enabled = true;
    }

    public virtual void Reset()
    {
    }
}