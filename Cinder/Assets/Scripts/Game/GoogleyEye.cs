using UnityEngine;

public class GoogleyEye : MonoBehaviour
{
    private Ball currentBallTracking;

    [SerializeField]
    protected Transform eyeBall;

    [SerializeField]
    private SpriteRenderer[] eyeRenders;

    protected void Update()
    {
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

        Vector3 dir = (currentBallTracking.transform.position - this.transform.position).normalized * .08f;
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