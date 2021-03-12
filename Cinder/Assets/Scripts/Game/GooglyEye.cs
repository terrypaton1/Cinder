using UnityEngine;

public class GooglyEye : MonoBehaviour
{
    [SerializeField]
    protected Transform eyeBall;

    [SerializeField]
    protected SpriteRenderer[] eyeRenders;

    private Ball currentBallTracking;
    private bool hasTarget;
    private bool isEnabled = false;

    protected void Update()
    {
        if (!isEnabled)
        {
            return;
        }

        if (currentBallTracking == null)
        {
            // todo change this to use the ball manager to get a ball!!
            currentBallTracking = CoreConnector.GameManager.ballManager.GetFirstActiveBall();
            hasTarget = (currentBallTracking != null);
            Debug.Log("hasTarget:" + hasTarget);
        }

        if (!hasTarget)
        {
            return;
        }

        var dir = (currentBallTracking.transform.position - transform.position).normalized * 0.08f;
        eyeBall.localPosition = Vector3.Lerp(eyeBall.localPosition, dir, Time.deltaTime * 5.0f);
    }

    public void Hide()
    {
        isEnabled = false;
        foreach (var spriteRenderer in eyeRenders)
        {
            spriteRenderer.enabled = false;
        }
    }

    public void Show()
    {
        isEnabled = true;
        foreach (var spriteRenderer in eyeRenders)
        {
            spriteRenderer.enabled = true;
        }
    }
}