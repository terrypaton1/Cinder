using UnityEngine;
using System.Collections;

public class WanderingObstacle : BrickBase
{
    [SerializeField]
    protected bool useCrazyMovement;

    [SerializeField]
    protected Rigidbody2D thisRigidbody;

    [SerializeField]
    protected Animator _animator;

    [Range(1, 30)]
    public int amountOfBricksBeforeSpawn;

    public bool activeAndMoving;

    [Range(1, 10)]
    public int amountOfHitsToKill = 2;

    private int amountOfHitReceivedFromBall;
    private int obstaclePointsValue;

    private int amountOfBricksDestroyedSoFar;
    private float currentSpeed;
    private float maxSpeed = 3;
    private float currentDirectionTendency;
    private Vector3 startingPosition;

    // if amountOfBricksBeforeSpawn ==0 then the obstacle should start straight away
    // the obstacle should semi randomly move around the screen
    // does not collide with bricks, but does collide with ball
    // Doesn't move below a height of 4
    // after x amount of hits from a ball, this obstacle should be destroyed

    protected void Awake()
    {
        startingPosition = transform.position;
    }

    public override void LevelComplete()
    {
        thisRigidbody.velocity = Vector2.zero;
        activeAndMoving = false;
    }

    public override void BrickWasDestroyed()
    {
        if (BrickHasBeenDestroyed)
        {
            return;
        }

        if (activeAndMoving)
        {
            return;
        }

        amountOfBricksDestroyedSoFar++;
        CheckIfObstacleShouldBeEnabled();
    }

    private void CheckIfObstacleShouldBeEnabled()
    {
        if (amountOfBricksBeforeSpawn == 0)
        {
            EnableObstacle();
        }
        else
        {
            if (amountOfBricksDestroyedSoFar >= amountOfBricksBeforeSpawn)
            {
                EnableObstacle();
            }
        }
    }

    public override void BrickHitByBall()
    {
        PlaySound(SoundList.WanderingObstacleHit);
        amountOfHitReceivedFromBall--;
        if (amountOfHitReceivedFromBall < 1)
        {
            StartCoroutine(DestroyBrickSequence());
        }
        else
        {
// brick has taken a hit, shake its visuals!
            _animator.Play("WanderingObstacleHitWithBall");
        }
    }

    protected override IEnumerator DestroyBrickSequence(bool playSound = true)
    {
        BrickHasBeenDestroyed = true;

        thisRigidbody.velocity = Vector2.zero;
        activeAndMoving = false;

        CoreConnector.GameManager.scoreManager.PointsCollected(obstaclePointsValue);
        SpawnParticles(ParticleTypes.WanderingObstacleExplosion, transform.position);
        if (playSound)
        {
            PlaySound(SoundList.WanderingObstacleDestroyed);
        }

        CoreConnector.GameManager.brickManager.BrickDestroyed();

        yield return null;

        DisableColliders();

        var counter = 0.5f;
        targetScale = 0.1f;

        while (counter > 0)
        {
            counter -= Time.deltaTime;
            yield return null;
        }

        DisableVisuals();
    }

    public override void ResetBrick()
    {
        DisableObstacle();
        transform.position = startingPosition;
        CheckIfObstacleShouldBeEnabled();

        base.ResetBrick();
    }

    public override void Show()
    {
        // Wanderring obstacle doesn't appear at first
        DisableVisuals();
        DisableColliders();
    }

    private void EnableObstacle()
    {
        obstaclePointsValue = Points.WanderingObstaclePointsValue;
        maxSpeed = GameVariables.wanderingObstacleSpeed;
        activeAndMoving = true;

        EnableColliders();
        EnableVisuals();

        SpawnParticles(ParticleTypes.WanderingObstacleSpawn, transform.position);

        visualScale = Vector3.one;
        visualObjects.transform.localScale = visualScale;
        // apply a random direction
        var randomVector = Random.insideUnitCircle;
        randomVector = randomVector.normalized * 10.0f;
        currentDirectionTendency = 0.0f;
        thisRigidbody.AddForce(randomVector);
    }

    private void DisableObstacle()
    {
        thisRigidbody.velocity = Vector2.zero;
        activeAndMoving = false;

        amountOfBricksDestroyedSoFar = 0;
        amountOfHitReceivedFromBall = amountOfHitsToKill;

        DisableColliders();
        DisableVisuals();
    }

    public override void UpdateLoop()
    {
    }

    protected void FixedUpdate()
    {
        if (BrickHasBeenDestroyed)
        {
            return;
        }

        if (!activeAndMoving)
        {
            return;
        }

        currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime);
        var velocity = thisRigidbody.velocity;
        if (velocity.magnitude < currentSpeed)
        {
            thisRigidbody.velocity = velocity.normalized * currentSpeed;
        }

        if (velocity.magnitude > maxSpeed)
        {
//					Debug.Log("slowing down the entity");
            currentSpeed = velocity.magnitude;
            currentSpeed -= Time.deltaTime * 3.0f;
            thisRigidbody.velocity = velocity.normalized * currentSpeed;
        }

        if (useCrazyMovement)
        {
            // crazy ball only gets applied if the ball is away from the bat, don't want it too hard for the player to hit
            if (transform.position.y > 2.0f)
            {
                velocity = thisRigidbody.velocity;
                // modify the balls movement direction, but only if the balls y> 2(?)
                velocity.x += Random.Range(-0.15f, 0.15f);
                velocity.y += Random.Range(-0.15f, 0.15f);
                velocity = velocity.normalized * currentSpeed;
                thisRigidbody.velocity = velocity;
            }
        }

        // depending on left/right direction flip the sprite 
        if (velocity.x < 0.0f)
        {
            currentDirectionTendency -= 0.1f;
        }
        else
        {
            currentDirectionTendency += 0.1f;
        }

        currentDirectionTendency = Mathf.Clamp(currentDirectionTendency, -1.0f, 1.0f);
        sprite.flipX = currentDirectionTendency < 0.0f;
    }

    protected override void ApplyNormalLayers()
    {
        // empty.
    }
}