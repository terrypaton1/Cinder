using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Serialization;

public class GameMessages : MonoBehaviour
{ 
    [SerializeField]
    protected TextMeshProUGUI messageText;

    [FormerlySerializedAs("_messageBox")]
    [SerializeField]
    protected GameObject messageBox;

    [SerializeField]
    protected Animator messageAnimation;

    private IEnumerator coroutine;

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
//		Debug.Log("ShowMessage");
        messageAnimation.Play("ShowMessage");
        yield return new WaitForSeconds(3f);
//		Debug.Log("HideMessage");
        messageAnimation.Play("HideMessage");
    }

    public void HideInGameMessageInstantly()
    {
        StopAllCoroutines();
        messageAnimation.Play("HideMessage");
    }

    public void HideInGameMessage()
    {
        StopAllCoroutines();
        StartCoroutine(HideInGameMessageSequence());
    }

    private IEnumerator HideInGameMessageSequence()
    {
        messageAnimation.Play("HideMessage");
        yield return new WaitForSeconds(1f);
        messageBox.SetActive(false);
    }

    public void LifeLost()
    {
        StopAllCoroutines();
        messageAnimation.Play("HideMessage");
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        messageAnimation.Play("HideMessage");
    }
}