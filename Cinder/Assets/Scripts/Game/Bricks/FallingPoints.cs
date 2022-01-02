using System.Collections;
using UnityEngine;

public class FallingPoints : FallingBase
{
    [SerializeField]
    private TrailRenderer trail;

    private int pointsValue;
    private int category;
    public const string DashID = "FallingPoints";
    IEnumerator coroutine;

    public override void Hide()
    {
        base.Hide();
        spriteRenderer.enabled = false;
        trail.emitting = false;
        trail.Clear();
        StopRunningCoroutine(coroutine);
    }

    public override string GetDashID()
    {
        return DashID;
    }

    public void Setup(int newPointsValue, int _category)
    {
        category = _category;
        pointsValue = newPointsValue;
        // disable all pointsDisplay

        if (!Application.isPlaying)
        {
            return;
        }

        isFalling = false;
        spriteRenderer.enabled = true;
        trail.emitting = false;
        switch (category)
        {
            case 1:
                spriteRenderer.sprite = CoreConnector.GameManager.gameSettings.pointsDisplay10;
                break;
            case 2:
                spriteRenderer.sprite = CoreConnector.GameManager.gameSettings.pointsDisplay50;
                break;
            case 3:
                spriteRenderer.sprite = CoreConnector.GameManager.gameSettings.pointsDisplay100;
                break;
            default:
                spriteRenderer.sprite = CoreConnector.GameManager.gameSettings.pointsDisplay10;
                break;
        }
    }

    public override void HitPlayersBat()
    {
        base.HitPlayersBat();
        PlaySound(SoundList.PointsCollected);
        CoreConnector.GameManager.scoreManager.PointsCollected(pointsValue);
    }

    protected override void FellInToDeadZone()
    {
        Disable();
    }

    public override void Disable()
    {
        base.Disable();
        trail.emitting = false;
    }

    public override void StartFalling(Vector3 position)
    {
        base.StartFalling(position);

        StopRunningCoroutine(coroutine);
        coroutine = EnableTrail();
        StartCoroutine(coroutine);
    }

    private IEnumerator EnableTrail()
    {
        yield return null;
        trail.emitting = true;
    }
}