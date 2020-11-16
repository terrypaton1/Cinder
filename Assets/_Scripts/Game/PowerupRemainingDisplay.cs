using UnityEngine;
using System.Collections;

public class PowerupRemainingDisplay : MonoBehaviour
{
    public UIWidget _uiWidget;

    [SerializeField]
    protected GameObject _messageBox;

    [SerializeField]
    UISprite _progressBar;

    protected Animator messageAnimation;

    protected void Awake()
    {
        messageAnimation = GetComponent<Animator>();
        _messageBox.SetActive(false);
    }

    public void DisplayPercent(float percent)
    {
        _progressBar.transform.localScale = new Vector3(percent, 1, 1);
    }

    private void DisplayPowerupBar()
    {
        _messageBox.SetActive(true);
        messageAnimation.Play("PowerupRemainingShow");
    }

    private void HidePowerupBarInstantly()
    {
        StopAllCoroutines();
        messageAnimation.Play("PowerupRemainingShow");
    }

    private void HidePowerupBar()
    {
        StopAllCoroutines();
        StartCoroutine(HideInGameMessageSequence());
    }

    private IEnumerator HideInGameMessageSequence()
    {
        messageAnimation.Play("PowerupRemainingHide");
        yield return new WaitForSeconds(1f);
        _messageBox.SetActive(false);
    }

    private void LifeLost()
    {
        StopAllCoroutines();
        messageAnimation.Play("PowerupRemainingHide");
    }

    protected void OnEnable()
    {
        Messenger.AddListener(GlobalEvents.DisplayPowerupBar, DisplayPowerupBar);
        Messenger.AddListener(GlobalEvents.HidePowerupBarInstantly, HidePowerupBarInstantly);
        Messenger.AddListener(GlobalEvents.HidePowerupBar, HidePowerupBar);
        Messenger.AddListener(GlobalEvents.LifeLost, LifeLost);
    }

    protected void OnDisable()
    {
        Messenger.RemoveListener(GlobalEvents.DisplayPowerupBar, DisplayPowerupBar);
        Messenger.RemoveListener(GlobalEvents.HidePowerupBarInstantly, HidePowerupBarInstantly);
        Messenger.RemoveListener(GlobalEvents.HidePowerupBar, HidePowerupBar);
        Messenger.RemoveListener(GlobalEvents.LifeLost, LifeLost);
    }

    protected void OnDestroy()
    {
        s_Instance = null;
    }

    private static PowerupRemainingDisplay s_Instance = null;

    public static PowerupRemainingDisplay instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(PowerupRemainingDisplay)) as PowerupRemainingDisplay;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                Debug.Log("Could not locate an PowerupRemainingDisplay object!");
//				UnityEngine.Debug.Break();
            }

            return s_Instance;
        }
    }

    protected void OnApplicationQuit()
    {
        s_Instance = null;
    }
}