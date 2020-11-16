using UnityEngine;
using System.Collections;

public class GameMessages : MonoBehaviour
{
    public UIWidget _uiWidget;

    [SerializeField]
    protected UILabel _messageText;

    [SerializeField]
    protected GameObject _messageBox;

    private Animator messageAnimation;

    protected void Awake()
    {
        messageAnimation = GetComponent<Animator>();
        _messageBox.SetActive(false);
    }

    private void DisplayInGameMessage(string _message)
    {
        _messageText.text = _message;
        StopCoroutine(ShowMessageSequence());
        StartCoroutine(ShowMessageSequence());
    }

    private IEnumerator ShowMessageSequence()
    {
        _messageBox.SetActive(true);
//		Debug.Log("ShowMessage");
        messageAnimation.Play("ShowMessage");
        yield return new WaitForSeconds(3f);
//		Debug.Log("HideMessage");
        messageAnimation.Play("HideMessage");
    }

    private void HideInGameMessageInstantly()
    {
        StopAllCoroutines();
        messageAnimation.Play("HideMessage");
    }

    private void HideInGameMessage()
    {
        StopAllCoroutines();
        StartCoroutine(HideInGameMessageSequence());
    }

    private IEnumerator HideInGameMessageSequence()
    {
        messageAnimation.Play("HideMessage");
        yield return new WaitForSeconds(1f);
        _messageBox.SetActive(false);
    }

    private void LifeLost()
    {
        StopAllCoroutines();
        messageAnimation.Play("HideMessage");
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        messageAnimation.Play("HideMessage");
    }

    protected void OnEnable()
    {
        Messenger<string>.AddListener(GlobalEvents.DisplayInGameMessage, DisplayInGameMessage);
        Messenger.AddListener(GlobalEvents.HideInGameMessageInstantly, HideInGameMessageInstantly);
        Messenger.AddListener(GlobalEvents.HideInGameMessage, HideInGameMessage);
        Messenger.AddListener(GlobalEvents.LifeLost, LifeLost);
        Messenger.AddListener(MenuEvents.RestartGame, RestartGame);
    }

    protected void OnDisable()
    {
        Messenger<string>.RemoveListener(GlobalEvents.DisplayInGameMessage, DisplayInGameMessage);
        Messenger.RemoveListener(GlobalEvents.HideInGameMessageInstantly, HideInGameMessageInstantly);
        Messenger.RemoveListener(GlobalEvents.HideInGameMessage, HideInGameMessage);
        Messenger.RemoveListener(GlobalEvents.LifeLost, LifeLost);
        Messenger.RemoveListener(MenuEvents.RestartGame, RestartGame);
    }
}