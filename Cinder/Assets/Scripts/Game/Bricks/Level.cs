﻿using UnityEngine;

[ExecuteInEditMode]
public class Level : MonoBehaviour
{
    [SerializeField]
    public BrickBase[] bricks;

    [SerializeField]
    public NonBrick[] nonBricks;

    protected void OnEnable()
    {
        bricks = GetComponentsInChildren<BrickBase>();
        nonBricks = GetComponentsInChildren<NonBrick>();
    }

    public virtual void Show()
    {
        foreach (var brick in bricks)
        {
            brick.ResetBrick();
        }

        foreach (var nonBrick in nonBricks)
        {
            nonBrick.Show();
        }
    }

    public virtual void Hide()
    {
        foreach (var brick in bricks)
        {
            brick.Hide();
        }

        foreach (var nonBrick in nonBricks)
        {
            nonBrick.Hide();
        }
    }
}