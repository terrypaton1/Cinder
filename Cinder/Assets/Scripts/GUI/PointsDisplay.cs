using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PointsDisplay : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI pointsDisplayText;

    [FormerlySerializedAs("_pointsCollectedAnimator")]
    [SerializeField]
    protected Animator animator;

    protected void OnEnable()
    {
        UpdatePointsDisplay(0);
    }

    public void UpdatePointsDisplay(int value)
    {
        pointsDisplayText.text = value.ToString("n0");
        if (value > 0)
        {
            animator.Play("PointsCollected", 0, 0);
//			_pointsCollectedAnimator.SetTime(0);
        }
    }

    public void Show()
    {
        pointsDisplayText.enabled = true;
    }

    public void Hide()
    {
        pointsDisplayText.enabled = false;
    }
}