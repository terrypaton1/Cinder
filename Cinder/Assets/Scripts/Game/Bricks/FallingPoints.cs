﻿using UnityEngine;

public class FallingPoints : FallingBase
{
    private int _pointsValue;
    private int category;
    new public static string dashID = "FallingPoints";
    public override void Hide()
    {
        base.Hide();
        spriteRenderer.enabled = false;
        
    }

    public override string GetDashID()
    {
        return dashID;
    }

    // dashID=
    public void Setup(int newPointsValue, int _category)
    {
        category = _category;
        _pointsValue = newPointsValue;
        // disable all pointsDisplay

        if (!Application.isPlaying)
        {
            return;
        }

        isFalling = false;
        spriteRenderer.enabled = true;
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
        CoreConnector.GameManager.scoreManager.PointsCollected(_pointsValue);
    }

    protected override void FellInToDeadZone()
    {
        Disable();
    }
}