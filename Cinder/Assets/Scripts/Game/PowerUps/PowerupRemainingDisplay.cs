using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerupRemainingDisplay : MonoBehaviour
{
    [SerializeField]
    protected Transform bar;

    [SerializeField]
    protected Image[] images;

    [SerializeField]
    protected Animator messageAnimation;

    private const string PowerupRemainingShow = "Show";
    private const string PowerupRemainingHide = "Hide";
    private bool isShowing;

    protected void Awake()
    {
        DisableVisuals();
    }

    public void DisplayPercent(float percent)
    {
        bar.transform.localScale = new Vector3(percent, 1, 1);
    }

    public void DisplayPowerUpBar()
    {
        EnableVisuals();
        messageAnimation.Play(PowerupRemainingShow);
    }

    public void HidePowerUpBarInstantly()
    {
        DisableVisuals();
        StopAllCoroutines();
        messageAnimation.Play(PowerupRemainingHide);
    }

    public void HidePowerUpBar()
    {
        if (!isShowing)
        {
            return;
        }

        isShowing = false;
        StopAllCoroutines();
        StartCoroutine(HideInGameMessageSequence());
    }

    private IEnumerator HideInGameMessageSequence()
    {
        messageAnimation.Play(PowerupRemainingHide);
        yield return new WaitForSeconds(1.0f);
        DisableVisuals();
    }

    private void DisableVisuals()
    {
        foreach (var image in images)
        {
            image.enabled = false;
        }

        isShowing = false;
    }

    private void EnableVisuals()
    {
        foreach (var image in images)
        {
            image.enabled = true;
        }

        isShowing = true;
    }
}