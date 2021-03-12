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

    private const string HideMessage = "HideMessage";
    private const string ShowMessage = "ShowMessage";

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

        yield return new WaitForSeconds(1.0f);
        messageBox.SetActive(false);
    }

    private void HideInGameMessageInstantly()
    {
        messageBox.SetActive(false);
        StopCurrentCoroutine();
    }

    public void LifeLost()
    {
        HideInGameMessageInstantly();
    }
}