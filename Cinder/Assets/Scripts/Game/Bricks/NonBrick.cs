using System;
using UnityEngine;

public class NonBrick : BaseObject
{
    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    protected Collider2D colliderRef;

    public virtual void Show()
    {
        EnableVisuals();
        EnableCollisions();
    }

    public virtual void Hide()
    {
        DisableCollisions();
        DisableVisuals();
    }

    protected virtual void DisableVisuals()
    {
        if (spriteRenderer == null)
        {
            Debug.Log("HER#!",gameObject);
        }
        spriteRenderer.enabled = false;
    }

    protected virtual void EnableVisuals()
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

    public virtual void Reset()
    {
    }
}