using UnityEngine;
using System.Collections;
using TMPro;

public class GameMessages : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI messageText;

    [SerializeField]
    protected Animator messageAnimation;

    private IEnumerator coroutine;

    private const string Hide = "Hide";
    private const string Show = "Show";
    private const string Disabled = "Disabled";


    public void DisplayInGameMessage(string message)
    {
        messageText.text = message;

        StopCurrentCoroutine();
        coroutine = ShowMessageSequence();
        StartCoroutine(coroutine);
    }

    public void LifeLost()
    {
        HideInGameMessageInstantly();
    }

    private void StopCurrentCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator ShowMessageSequence()
    {
        messageAnimation.Play(Show);

        yield return WaitCache.WaitForSeconds(3.0f);

        // todo possibly add extra effects during the message

        messageAnimation.Play(Hide);

        yield return WaitCache.WaitForSeconds(1.0f);
        // Enforce disabled.
        HideInGameMessageInstantly();
    }

    private void HideInGameMessageInstantly()
    {
        messageAnimation.Play(Disabled);
        StopCurrentCoroutine();
    }
}