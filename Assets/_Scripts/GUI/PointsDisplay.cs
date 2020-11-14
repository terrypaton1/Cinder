#region

using UnityEngine;

#endregion

public class PointsDisplay : MonoBehaviour {
	/// <summary>
	/// The points display text.
	/// </summary>
	[SerializeField] UILabel pointsDisplayText;

	/// <summary>
	/// The points collected animator.
	/// </summary>
	Animator _pointsCollectedAnimator;

	void OnEnable() {
		UpdatePointsDisplay(0);
		Messenger<int>.AddListener(MenuEvents.UpdatePointsDisplay, UpdatePointsDisplay);
		_pointsCollectedAnimator = GetComponent<Animator>();
	}

	void OnDisable() {
		Messenger<int>.RemoveListener(MenuEvents.UpdatePointsDisplay, UpdatePointsDisplay);
	}

	/// <summary>
	/// Updates the points display.
	/// </summary>
	/// <param name="value">Value.</param>
	void UpdatePointsDisplay(int value) {
		pointsDisplayText.text = value.ToString("n0");
		if (value > 0) {
			_pointsCollectedAnimator.Play("PointsCollected",0,0);
//			_pointsCollectedAnimator.SetTime(0);
		}
	}
}
