using UnityEngine;

public class GooglyEye : MonoBehaviour
{
    [SerializeField]
    protected Transform eyeBall;

    [SerializeField]
    protected SpriteRenderer[] eyeRenders;

    private Ball currentBallTracking;
    private bool hasTarget;
    private bool isEnabled;

    protected void Update()
    {
        if (!isEnabled)
        {
            return;
        }

        if (!hasTarget)
        {
            // todo change this to use the ball manager to get a ball!!
            currentBallTracking = CoreConnector.GameManager.ballManager.GetFirstActiveBall();
            hasTarget = currentBallTracking != null;
        }

        if (currentBallTracking == null)
        {
            return;
        }

        var differenceVector = currentBallTracking.transform.position - transform.position;
        var targetLocation = differenceVector.normalized * 0.08f;

        eyeBall.localPosition = targetLocation;

        // todo every once and a while the eye should look at the player 
        //  eyeBall.localPosition = Vector3.Lerp(eyeBall.localPosition, targetLocation, Time.deltaTime * 5.0f);
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