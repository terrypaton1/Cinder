using UnityEngine;

public class BonusCharacter : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Color color = Color.white;
    private const float fadeOutTime = 0.5f;
    private const float fadeInTime = 0.5f;

    private void SetAlpha(float value)
    {
        color.a = value;
        spriteRenderer.color = color;
    }

    public void ShowCollected()
    {
        Debug.Log("Show collected");
        // scale changes, alphas in
        spriteRenderer.enabled = true;
        
        LeanTween.value(spriteRenderer.gameObject, 0.0f, 1.0f, fadeInTime)
            .setOnUpdate(SetAlpha);
    }

    public void Hide()
    {
        LeanTween.cancel(spriteRenderer.gameObject);
        LeanTween.value(spriteRenderer.gameObject, color.a, 0.0f, fadeOutTime)
            .setOnUpdate(SetAlpha)
            .setOnComplete(HideInstantly);
    }

    private void HideInstantly()
    {
        LeanTween.cancel(spriteRenderer.gameObject);
        spriteRenderer.enabled = false;
    }

    public void PlayCollected()
    {
        //  letterAnimator.Play(LetterBonusCollectedAnimation, 0, 0.0f);
        LeanTween.cancel(spriteRenderer.gameObject);
    }
}