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

    readonly WaitForSeconds showMessageSequence = new WaitForSeconds(3.0f);
    readonly WaitForSeconds hideMessageSequence = new WaitForSeconds(1.0f);

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

        yield return showMessageSequence;

        // todo possibly add extra effects during the message

        messageAnimation.Play(Hide);

        yield return hideMessageSequence;
        // Enforce disabled.
        HideInGameMessageInstantly();
    }

    private void HideInGameMessageInstantly()
    {
        messageAnimation.Play(Disabled);
        StopCurrentCoroutine();
    }
}