using UnityEngine;

public class PointsDisplay : MonoBehaviour
{
    [SerializeField]
    protected UILabel pointsDisplayText;

    Animator _pointsCollectedAnimator;

    protected void OnEnable()
    {
        UpdatePointsDisplay(0);
        Messenger<int>.AddListener(MenuEvents.UpdatePointsDisplay, UpdatePointsDisplay);
        _pointsCollectedAnimator = GetComponent<Animator>();
    }

    protected void OnDisable()
    {
        Messenger<int>.RemoveListener(MenuEvents.UpdatePointsDisplay, UpdatePointsDisplay);
    }

    private void UpdatePointsDisplay(int value)
    {
        pointsDisplayText.text = value.ToString("n0");
        if (value > 0)
        {
            _pointsCollectedAnimator.Play("PointsCollected", 0, 0);
//			_pointsCollectedAnimator.SetTime(0);
        }
    }
}