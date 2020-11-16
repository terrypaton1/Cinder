using UnityEngine;
using System.Collections;

public class BossHealthRemainingDisplay : MonoBehaviour
{
    public UIWidget _uiWidget;

    [SerializeField]
    protected GameObject _messageBox;

    [SerializeField]
    protected UISprite _progressBar;

    private Animator messageAnimation;

    protected void Awake()
    {
        messageAnimation = GetComponent<Animator>();
        _messageBox.SetActive(false);
    }

    public void DisplayPercent(float percent)
    {
        _progressBar.transform.localScale = new Vector3(percent, 1, 1);
    }

    private void DisplayBossHealthBar()
    {
        _messageBox.SetActive(true);
        messageAnimation.Play("BossHealthRemainingShow");
    }

    private void HideBossHealthInstantly()
    {
        StopAllCoroutines();
        messageAnimation.Play("BossHealthRemainingShow");
    }

    private void HideBossHealthBar()
    {
        StopAllCoroutines();
        StartCoroutine(HideInGameMessageSequence());
    }

    private IEnumerator HideInGameMessageSequence()
    {
        messageAnimation.Play("BossHealthRemainingHide");
        yield return new WaitForSeconds(1f);
        _messageBox.SetActive(false);
    }

    private void LifeLost()
    {
        StopAllCoroutines();
        messageAnimation.Play("BossHealthRemainingHide");
    }

    protected void OnEnable()
    {
        Messenger.AddListener(GlobalEvents.DisplayBossHealthBar, DisplayBossHealthBar);
        Messenger.AddListener(GlobalEvents.HideBossHealthInstantly, HideBossHealthInstantly);
        Messenger.AddListener(GlobalEvents.HideBossHealthBar, HideBossHealthBar);
        Messenger.AddListener(GlobalEvents.LifeLost, LifeLost);
    }

    protected void OnDisable()
    {
        Messenger.RemoveListener(GlobalEvents.DisplayBossHealthBar, DisplayBossHealthBar);
        Messenger.RemoveListener(GlobalEvents.HideBossHealthInstantly, HideBossHealthInstantly);
        Messenger.RemoveListener(GlobalEvents.HideBossHealthBar, HideBossHealthBar);
        Messenger.RemoveListener(GlobalEvents.LifeLost, LifeLost);
    }

    void OnDestroy()
    {
        s_Instance = null;
    }

    private static BossHealthRemainingDisplay s_Instance = null;

    public static BossHealthRemainingDisplay instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(BossHealthRemainingDisplay)) as BossHealthRemainingDisplay;
            }

            if (s_Instance == null)
            {
                Debug.Log("Could not locate an BossHealthRemainingDisplay object!");
            }

            return s_Instance;
        }
    }

    void OnApplicationQuit()
    {
        s_Instance = null;
    }
}