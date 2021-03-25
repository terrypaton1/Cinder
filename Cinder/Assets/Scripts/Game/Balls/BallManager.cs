using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallManager : BaseObject
{
    [SerializeField]
    protected Ball ballPrefabReference;

    private float ballMaxSpeedForCurrentLevel = 5;
    private Vector3 ballStartPosition;

    public List<Ball> ballList = new List<Ball>();
    public List<Ball> ballPoolList = new List<Ball>();
    private IEnumerator coroutine;

    public void Setup()
    {
        ballList = new List<Ball>();
        ballPoolList = new List<Ball>();

        for (var i = 0; i < 10; i++)
        {
            var ball = Instantiate(ballPrefabReference, ballStartPosition, Quaternion.identity);
            ball.transform.parent = transform;
            ball.Disable();
            ballPoolList.Add(ball);
        }
    }

    private Ball GetBallFromPool()
    {
        foreach (var ball in ballPoolList)
        {
            if (!ball.ballIsEnabled)
            {
                return ball;
            }
        }

        Debug.LogError("No balls left in pool");
        return null;
    }

    public override void LevelComplete()
    {
        StopAllCoroutines();
        DisableFlameBall();

        // tell all balls to complete.

        foreach (var ball in ballPoolList)
        {
            if (ball.activeAndMoving)
            {
                ball.LevelComplete();
            }
        }
    }

    public void GoingToNextLevel()
    {
        // Destroy all the current balls
        StopAllCoroutines();
    }

    public void AddNewBall()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = InstantiateNewBall();
        StartCoroutine(coroutine);
    }

    public void AddMultiBalls(float xPosition = 0)
    {
        StartCoroutine(InstantiateMultiBalls(xPosition));
    }

    public void RestartLevel()
    {
        StopAllCoroutines();
        ResetAllBalls();
    }

    public void ResetAllBalls()
    {
        foreach (var ball in ballPoolList)
        {
            ball.Disable();
        }

        ballList = new List<Ball>();
    }

    private IEnumerator InstantiateNewBall()
    {
        ballStartPosition.y = GameVariables.ballStartHeight;
        ballStartPosition.x = 0;
        SpawnParticles(ParticleTypes.NewBallOne, ballStartPosition);

        // wait a little while
        yield return new WaitForSeconds(0.4f);
        AddBall(0);

        yield return new WaitForSeconds(0.5f);

        LaunchAllBalls();
    }

    private IEnumerator InstantiateMultiBalls(float xPosition)
    {
        ballStartPosition.y = GameVariables.ballStartHeight;

        ballStartPosition.x = xPosition;
        SpawnParticles(ParticleTypes.NewBallOne, ballStartPosition);

        ballStartPosition.x = -xPosition;
        SpawnParticles(ParticleTypes.NewBallTwo, ballStartPosition);

        yield return new WaitForSeconds(0.5f);

        // add ball 1 & 2
        AddBall(xPosition);
        AddBall(-xPosition);

        yield return new WaitForSeconds(0.5f);

        LaunchAllBalls();
    }

    private void LaunchAllBalls()
    {
        ballMaxSpeedForCurrentLevel = CalculateBallMaxSpeed();
        foreach (var ball in ballList)
        {
            ball.LaunchBall(ballMaxSpeedForCurrentLevel);
        }
    }

    private void AddBall(float xPosition)
    {
        ballStartPosition.x = xPosition;
        var ball = GetBallFromPool();
        ball.transform.position = ballStartPosition;
        ball.Enable();
        ballList.Add(ball);
    }

    private static float CalculateBallMaxSpeed()
    {
        var levelNumber = PlayerPrefs.GetInt(Constants.currentLevel);
        if (levelNumber < 6)
        {
            return GameVariables.ballMaxSpeedPhase1;
        }

        if (levelNumber < 21)
        {
            return GameVariables.ballMaxSpeedPhase2;
        }

        if (levelNumber < 41)
        {
            return GameVariables.ballMaxSpeedPhase3;
        }

        return GameVariables.ballMaxSpeedPhase4;
    }

    public void BallDestroyed(Ball ball)
    {
        var position = ball.transform.position;
        // find the bottom of the screen
        position.y = 0.0f;
        SpawnParticles(ParticleTypes.BallLost, position);

        PlaySound(SoundList.ballLost);
        RemoveBall(ball);
        // need to check if there are any balls left in the game
        // if there are no balls in the scene, the player loses a life.
        // and the player has lives left, then
        if (ballList.Count < 1)
        {
            CoreConnector.GameManager.LoseLife();
        }
    }

    public void RemoveBall(Ball ball)
    {
        if (ballList.Contains(ball))
        {
            ballList.Remove(ball);
            // add the ball back on to the normal ball pool
        }

        ball.Disable();
    }

    public void ActivateFlameBall()
    {
        foreach (var ball in ballList)
        {
            ball.ActivateFlameBall();
        }
    }

    public void DisableFlameBall()
    {
        foreach (var ball in ballList)
        {
            ball.DisableFlameBall();
        }
    }

    public void DisableCrazyBall()
    {
        foreach (var ball in ballList)
        {
            ball.DisableCrazyBall();
        }
    }

    public void ActivateCrazyBall()
    {
        foreach (var ball in ballList)
        {
            ball.ActivateCrazyBall();
        }
    }

    public Ball GetFirstActiveBall()
    {
        foreach (var ball in ballList)
        {
            if (ball.activeAndMoving)
            {
                return ball;
            }
        }

        // no balls are currently active
        return null;
    }
}