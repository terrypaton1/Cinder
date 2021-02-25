﻿using UnityEngine;

public class GooglyEye : MonoBehaviour
{
    [SerializeField]
    protected Transform eyeBall;

    [SerializeField]
    protected SpriteRenderer[] eyeRenders;

    private Ball currentBallTracking;

    protected void Update()
    {
        if (currentBallTracking == null)
        {
            // todo change this to use the ball manager to get a ball!!
            currentBallTracking = CoreConnector.GameManager.ballManager.GetFirstActiveBall();
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