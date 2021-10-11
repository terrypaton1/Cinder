using UnityEngine;

public class NonBrick : BaseObject
{
    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    protected Collider2D colliderRef;

    private int resetCounter;
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
    public virtual void Shake(float amount)
    {
        var randomRadius = Random.Range(0.05f, 0.1f) * amount;

        var newLocal = Random.insideUnitSphere * randomRadius;
        newLocal.z = 0;
        spriteRenderer.transform.localPosition = newLocal;

        newLocal = spriteRenderer.transform.localEulerAngles;
        newLocal.z = Random.Range(-15.0f, 15.0f) * amount;
        spriteRenderer.transform.localEulerAngles = newLocal;

        resetCounter += 2;
    }
    
    public virtual void UpdateLoop()
    {
       
        var newPosition =
            Vector3.Lerp(spriteRenderer.transform.localPosition, Vector3.zero, Time.deltaTime * 10);
        spriteRenderer.transform.localPosition = newPosition;

        if (resetCounter <= 0)
        {
            return;
        }

        resetCounter--;
        if (resetCounter == 0)
        {
            spriteRenderer.transform.localEulerAngles = Vector3.zero;
        }
    }
}