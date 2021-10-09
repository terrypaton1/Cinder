using UnityEngine;
using System.Collections;

public class BossHealthRemainingDisplay : MonoBehaviour
{
    [SerializeField]
    protected GameObject _messageBox;

    [SerializeField]
    protected Transform progressBar;

    [SerializeField]
    protected Animator messageAnimation;

    protected void Awake()
    {
        _messageBox.SetActive(false);
    }

    public void DisplayPercent(float percent)
    {
        // if not showing, then show
        progressBar.localScale = new Vector3(percent, 1, 1);
    }

    public void DisplayBossHealthBar()
    {
        _messageBox.SetActive(true);
        messageAnimation.Play("BossHealthRemainingShow");
    }

    public void HideBossHealthBar()
    {
        StopAllCoroutines();
        StartCoroutine(HideInGameMessageSequence());
    }

    private IEnumerator HideInGameMessageSequence()
    {
        messageAnimation.Play("BossHealthRemainingHide");
        yield return new WaitForSeconds(1.0f);
        _messageBox.SetActive(false);
    }

    public void Hide()
    {
        StopAllCoroutines();
        messageAnimation.Play("BossHealthRemainingHide");
    }
}