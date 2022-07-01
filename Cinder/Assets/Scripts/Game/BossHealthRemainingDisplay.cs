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

    private IEnumerator _coroutine;

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
        StopRoutine();
        _coroutine = HideInGameMessageSequence();
        StartCoroutine(_coroutine);
    }

    private IEnumerator HideInGameMessageSequence()
    {
        messageAnimation.Play("BossHealthRemainingHide");
        yield return WaitCache.WaitForSeconds(1.0f);
        _messageBox.SetActive(false);
    }

    public void Hide()
    {
        StopRoutine();
        messageAnimation.Play("BossHealthRemainingHide");
    }

    private void StopRoutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }
}