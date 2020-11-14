using System.Collections;
using UnityEngine;

public class WanderingObstacle : BaseObject
{
    [SerializeField]
    SpriteRenderer _sprite;

    [SerializeField]
    bool useCrazyMovement;

    [SerializeField]
    GameObject visualObjects;

    public bool activeAndMoving;

    [Range(1, 30)]
    public int amountOfBricksBeforeSpawn;

    [Range(1, 10)]
    public int amountofHitsToKill = 2;

    [SerializeField]
    Animator _animator;

    protected Collider2D _collider;
    private int amountOfBricksDestroyedSoFar;

    private int amountOfHitRecievedFromBall;

    float currentDirectionTendancy;

    private float currentSpeed;
    private float maxSpeed = 3;
    private bool obstacleHasBeenDestroyed;
    private int obstaclePointsValue;

    private Vector3 startingPosition;
    private Rigidbody2D thisRigidbody;

    private Vector3 visualScale = Vector3.one;
    // if amountOfBricksBeforeSpawn ==0 then the obstacle should start straight away
    // the obstacle should semi randomly move around the screen
    // doesnt collide with bricks, but does collide with ball
    // Doesn't move below a height of 4
    // after x amount of hits from a ball, this obstacle should be destroyed

    protected void Awake()
    {
        _collider = GetComponentInChildren<Collider2D>();
        thisRigidbody = GetComponentInChildren<Rigidbody2D>();
        startingPosition = transform.position;
    }

    protected void Start()
    {
        DisableObstacle();
        CheckIfObstacleShouldBeEnabled();
    }

    protected void FixedUpdate()
    {
        if (obstacleHasBeenDestroyed)
        {
            return;
        }

        if (activeAndMoving)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime);
//			Debug.Log("currentSpeed:" + currentSpeed);	
            var velocity = thisRigidbody.velocity;
            if (velocity.magnitude < currentSpeed)
            {
                thisRigidbody.velocity = velocity.normalized * currentSpeed;
            }

            if (velocity.magnitude > maxSpeed)
            {
//					Debug.Log("slowing down the ball");
                currentSpeed = velocity.magnitude;
                currentSpeed -= Time.deltaTime * 3f;
//					currentBallSpeed *= .99f;
                thisRigidbody.velocity = velocity.normalized * currentSpeed;
            }

            if (useCrazyMovement)
            {
                // crazy ball only gets applied if the ball is away from the bat, don't want it too hard for the player to hit
                if (transform.position.y > 2)
                {
                    velocity = thisRigidbody.velocity;
                    // modify the balls movement direction, but only if the balls y> 2(?)
                    velocity.x += Random.Range(-.15f, .15f);
                    velocity.y += Random.Range(-.15f, .15f);
                    velocity = velocity.normalized * currentSpeed;
                    thisRigidbody.velocity = velocity;
                }
            }

            // depending on left/right direction flip the sprite 
            if (velocity.x < 0)
            {
                currentDirectionTendancy -= .1f;
            }
            else
            {
                currentDirectionTendancy += .1f;
            }

            currentDirectionTendancy = Mathf.Clamp(currentDirectionTendancy, -1f, 1f);
            if (currentDirectionTendancy < 0)
            {
                _sprite.flipX = true;
            }
            else
            {
                _sprite.flipX = false;
            }
        }
    }

    protected void OnEnable()
    {
        Messenger.AddListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.AddListener(GlobalEvents.BrickWasDestroyed, BrickWasDestroyed);
        Messenger.AddListener(MenuEvents.RestartGame, RestartGame);
    }

    protected void OnDisable()
    {
        Messenger.RemoveListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.RemoveListener(GlobalEvents.BrickWasDestroyed, BrickWasDestroyed);
        Messenger.RemoveListener(MenuEvents.RestartGame, RestartGame);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (obstacleHasBeenDestroyed)
            return;
//		Debug.Log(coll.gameObject.tag);
        if (collision.gameObject.CompareTag("Ball"))
        {
            Messenger<ParticleTypes, Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.BallHitsBrick,
                collision.contacts[0].point, MessengerMode.DONT_REQUIRE_LISTENER);
            WanderingObstacleHitByBall();
            // tell the ball you hit me
            var _ball = collision.gameObject.GetComponent<Ball>();
            _ball.HitABrick();
        }

        if (collision.gameObject.CompareTag("LaserBullet"))
        {
            Messenger<ParticleTypes, Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.LaserHitsBrick,
                collision.contacts[0].point, MessengerMode.DONT_REQUIRE_LISTENER);

            WanderingObstacleHitByBall();
            // tell the ball you hit me
            var _laserBullet = collision.gameObject.GetComponent<LaserBullet>();
            _laserBullet.HitABrick(collision.contacts[0].point);
        }
    }

    private void LevelComplete()
    {
        thisRigidbody.velocity = Vector2.zero;
        activeAndMoving = false;
    }

    private void BrickWasDestroyed()
    {
        if (obstacleHasBeenDestroyed)
            return;
        if (!activeAndMoving)
        {
            amountOfBricksDestroyedSoFar++;
            CheckIfObstacleShouldBeEnabled();
        }
    }

    private void RestartGame()
    {
        DisableObstacle();
        transform.position = startingPosition;
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

    public virtual void WanderingObstacleHitByBall()
    {
        PlaySound(SoundList.WanderingObstacleHit);
        amountOfHitRecievedFromBall--;
        if (amountOfHitRecievedFromBall < 1)
        {
            StartCoroutine(DestroyObstacleSequence());
        }
        else
        {
// brick has taken a hit, shake its visuals!
            _animator.Play("WanderingObstacleHitWithBall");
        }
    }

    public virtual IEnumerator DestroyObstacleSequence()
    {
        obstacleHasBeenDestroyed = true;

        thisRigidbody.velocity = Vector2.zero;
        activeAndMoving = false;
        Messenger<int>.Broadcast(GlobalEvents.PointsCollected, obstaclePointsValue,
            MessengerMode.DONT_REQUIRE_LISTENER);
        Messenger<ParticleTypes, Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect,
            ParticleTypes.WanderingObstacleExplosion, transform.position, MessengerMode.DONT_REQUIRE_LISTENER);
        PlaySound(SoundList.WanderingObstacleDestroyed);
        yield return 0;
        _collider.enabled = false;
        while (visualScale.x > 0.1f)
        {
            visualScale.x *= .8f;
            visualScale.y *= .8f;
            visualObjects.transform.localScale = visualScale;
            yield return 0;
        }

        visualObjects.SetActive(false);
    }

    private void EnableObstacle()
    {
        obstacleHasBeenDestroyed = false;
        obstaclePointsValue = GameVariables.wanderingObstaclePointsValue;
        maxSpeed = GameVariables.wanderingObstacleSpeed;
        activeAndMoving = true;
        _collider.enabled = true;
        visualObjects.SetActive(true);
        Messenger<ParticleTypes, Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect,
            ParticleTypes.WanderingObstacleSpawn, transform.position, MessengerMode.DONT_REQUIRE_LISTENER);

        visualScale = Vector3.one;
        visualObjects.transform.localScale = visualScale;
        // apply a random direction
        var randomVector = Random.insideUnitCircle;
//		thisRigidbody.velocity = randomVector.normalized * GameVariables.instance.wanderingObstacleSpeed;
        randomVector = randomVector.normalized * 10f; //* GameVariables.instance.wanderingObstacleSpeed;
        currentDirectionTendancy = 0;
        thisRigidbody.AddForce(randomVector);
//		Debug.Log(randomVector);
    }

    private void DisableObstacle()
    {
        thisRigidbody.velocity = Vector2.zero;
        activeAndMoving = false;
        amountOfBricksDestroyedSoFar = 0;
        amountOfHitRecievedFromBall = amountofHitsToKill;
        visualObjects.SetActive(false);
        _collider.enabled = false;
    }
}