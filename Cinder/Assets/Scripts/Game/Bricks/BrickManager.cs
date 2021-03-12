using UnityEngine;
using System.Collections.Generic;

public class BrickManager : MonoBehaviour
{
    private Vector3 brickTestPosition = Vector3.zero;

    private float distance;

    private List<BrickBase> activeBrickList = new List<BrickBase>();
    private List<NonBrick> nonBrickList = new List<NonBrick>();

    public int bricksLayerDuringFlameBall;
    public int bricksLayerNormal;

    protected void Awake()
    {
        activeBrickList = new List<BrickBase>();
        bricksLayerDuringFlameBall = LayerMask.NameToLayer("BricksDuringFlameBall");
        bricksLayerNormal = LayerMask.NameToLayer("Bricks");
    }

    public void LoadLevelsBricks()
    {
        activeBrickList = CoreConnector.LevelsManager.GetBricksForCurrentLevel();
    }

    public void NextLevel()
    {
        activeBrickList = new List<BrickBase>();
        nonBrickList = new List<NonBrick>();
    }

    public void RestartLevel()
    {
        activeBrickList = CoreConnector.LevelsManager.GetBricksForCurrentLevel();
        nonBrickList = CoreConnector.LevelsManager.GetNonBricksForCurrentLevel();

        foreach (var brickBase in activeBrickList)
        {
            brickBase.ResetBrick();
        }
    }

    public void BrickDestroyed()
    {
        // check if brick destroyed is the same as the last one
        BrickWasDestroyed();
        CoreConnector.GameManager.powerUpManager.BrickWasDestroyed();
        CoreConnector.GameManager.gameVariables.IncreaseBricksBroken();

        if (BricksAreStillActive())
        {
            return;
        }

        // No bricks are active, level complete!
        CoreConnector.GameManager.LevelComplete();
    }

    private bool BricksAreStillActive()
    {
        // how many bricks are still active
        var activeBricks = CalculateActiveBricks();
        return activeBricks > 0;
    }

    public Vector3 GetLastBrickLocation()
    {
        foreach (var brick in activeBrickList)
        {
            if (!brick.BrickHasBeenDestroyed)
            {
                return brick.transform.position;
            }
        }

        return activeBrickList[0].transform.position;
    }

    public void TestDestroyBricksAroundTNT(Vector3 position, float range, BrickBase TheTNTBrick)
    {
        // find all bricks within radius of this one.
#if UNITY_EDITOR
//		Debug.DrawRay(position, Vector2.up * range, Color.blue, 10);
#endif
        // scan through all active bricks and explode them
        foreach (var brick in activeBrickList)
        {
            if (brick != TheTNTBrick && !brick.BrickHasBeenDestroyed)
            {
                TestTNTExplosion(brick, position, range);
            }
        }
    }

    private void TestTNTExplosion(BrickBase brick, Vector3 position, float range)
    {
        brickTestPosition = brick.transform.position;
        distance = Vector3.Distance(position, brickTestPosition);
        if (distance <= range)
        {
            // affect the brick
            brick.AffectedByTNTExplosion();
        }
    }

    public void DisableFlameBall()
    {
        foreach (var brickBase in activeBrickList)
        {
            brickBase.DisableFlameBall();
        }
    }

    public void ActivateFlameBall()
    {
        foreach (var brickBase in activeBrickList)
        {
            brickBase.ActivateFlameBall();
        }
    }

    public bool IsOneBrickLeft()
    {
        var activeBrickCount = CalculateActiveBricks();
        return activeBrickCount == 1;
    }

    public void ShakeGame(float intensity)
    {
        foreach (var brickBase in activeBrickList)
        {
            brickBase.Shake(intensity);
        }
    }

    public void LevelComplete()
    {
        DisableFlameBall();
        foreach (var brickBase in activeBrickList)
        {
            brickBase.LevelComplete();
        }
    }

    public void LifeLost()
    {
        foreach (var brickBase in activeBrickList)
        {
            brickBase.LifeLost();
        }
    }

    private void BrickWasDestroyed()
    {
        foreach (var brickBase in activeBrickList)
        {
            brickBase.BrickWasDestroyed();
        }
    }

    public void UpdateLoop()
    {
        foreach (var brickBase in activeBrickList)
        {
            brickBase.UpdateLoop();
        }

        // give the non bricks an update loop
    }

    private int CalculateActiveBricks()
    {
        var activeBrickCount = 0;
        foreach (var brick in activeBrickList)
        {
            if (!brick.BrickHasBeenDestroyed)
            {
                activeBrickCount++;
            }
        }

        return activeBrickCount;
    }
}