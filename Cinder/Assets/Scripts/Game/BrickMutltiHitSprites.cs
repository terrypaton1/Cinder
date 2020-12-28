using UnityEngine;

public class BrickMutltiHitSprites : MonoBehaviour
{
    [SerializeField]
    protected Sprite[] hitNumbers;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    public void DisplayHitsLeft(int amountOfHitsToDestroy)
    {
        if (amountOfHitsToDestroy < 2)
        {
            Hide();
            // no renderers get enabled below this value
            return;
        }

        Show();
        // the max number shown is 6
        var value = Mathf.Clamp(amountOfHitsToDestroy, 0, 6);
        var sprite = hitNumbers[value];
        Debug.Log("sprite:"+sprite);
        spriteRenderer.sprite = sprite;
    }

    public void Hide()
    {
        spriteRenderer.enabled = false;
    }

    public void Show()
    {
        spriteRenderer.enabled = true;
    }
}