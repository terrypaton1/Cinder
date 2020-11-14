#region

using UnityEngine;

#endregion

public class LevelChooser : MonoBehaviour {
	/// <summary>
	/// The scroll view.
	/// </summary>
	[SerializeField]
	UIScrollView _scrollView;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable() {
		Messenger.AddListener(GlobalEvents.StopLevelScroller, StopLevelScroller);
//	Debug.Log("enabled the scroll view");
				_scrollView.enabled = true;
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable() {
		Messenger.RemoveListener(GlobalEvents.StopLevelScroller, StopLevelScroller);
	}

	/// <summary>
	/// Stops the level scroller.
	/// </summary>
	void StopLevelScroller() {
//	Debug.Log("stop level scroller");
		_scrollView.enabled = false;
	}
}
