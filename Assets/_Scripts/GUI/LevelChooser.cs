using UnityEngine;

public class LevelChooser : MonoBehaviour
{
    [SerializeField]
    UIScrollView _scrollView;

    protected void OnEnable()
    {
        Messenger.AddListener(GlobalEvents.StopLevelScroller, StopLevelScroller);
//	Debug.Log("enabled the scroll view");
        _scrollView.enabled = true;
    }

    protected void OnDisable()
    {
        Messenger.RemoveListener(GlobalEvents.StopLevelScroller, StopLevelScroller);
    }

    private void StopLevelScroller()
    {
//	Debug.Log("stop level scroller");
        _scrollView.enabled = false;
    }
}