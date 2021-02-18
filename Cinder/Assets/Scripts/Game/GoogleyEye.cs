using UnityEngine;

public class GoogleyEye : MonoBehaviour
{
    [SerializeField]
    protected Transform eyeBall;

    [SerializeField]
    protected SpriteRenderer[] eyeRenders;

    private Ball currentBallTracking;

    protected void Update()
    {
        // todo - this is awful!
        // look at the players bat
        if (currentBallTracking == null)
        {
            // todo change this to use the ball manager to get a ball!!
            currentBallTracking = (Ball) FindObjectOfType(typeof(Ball));
        }

        if (currentBallTracking == null)
        {
            return;
        }

        var dir = (currentBallTracking.transform.position - transform.position).normalized * .08f;
        eyeBall.localPosition = Vector3.Lerp(eyeBall.localPosition, dir, Time.deltaTime * 5f);
    }

    public void Hide()
    {
        foreach (var spriteRenderer in eyeRenders)
        {
            spriteRenderer.enabled = false;
        }
    }

    public void Show()
    {
        foreach (var spriteRenderer in eyeRenders)
        {
            spriteRenderer.enabled = true;
        }
    }
}