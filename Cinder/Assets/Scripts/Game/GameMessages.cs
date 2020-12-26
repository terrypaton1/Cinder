using UnityEngine;
using System.Collections;
using TMPro;

public class GameMessages : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI messageText;

    [SerializeField]
    protected GameObject messageBox;

    [SerializeField]
    protected Animator messageAnimation;

    private IEnumerator coroutine;

    private readonly string HideMessage = "HideMessage";
    private readonly string ShowMessage = "ShowMessage";

    protected void Awake()
    {
        HideMessageInstantly();
    }

    private void HideMessageInstantly()
    {
        // instead change this to make the animator play disabled.
        messageBox.SetActive(false);
    }

    public void DisplayInGameMessage(string message)
    {
        messageText.text = message;

        StopCurrentCoroutine();
        coroutine = ShowMessageSequence();
        StartCoroutine(coroutine);
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
        messageBox.SetActive(true);
        messageAnimation.Play(ShowMessage);
        yield return new WaitForSeconds(3.0f);
        messageAnimation.Play(HideMessage);
    }

    public void HideInGameMessageInstantly()
    {
        StopAllCoroutines();
        messageAnimation.Play(HideMessage);
    }

    public void HideInGameMessage()
    {
        StopAllCoroutines();
        StartCoroutine(HideInGameMessageSequence());
    }

    private IEnumerator HideInGameMessageSequence()
    {
        messageAnimation.Play(HideMessage);
        yield return new WaitForSeconds(1.0f);
        messageBox.SetActive(false);
    }

    public void LifeLost()
    {
        StopAllCoroutines();
        messageAnimation.Play(HideMessage);
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        messageAnimation.Play(HideMessage);
    }
}