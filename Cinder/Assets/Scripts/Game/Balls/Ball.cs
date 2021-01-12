using UnityEngine;

public sealed class Ball : BaseObject
{
    [SerializeField]
    private Rigidbody2D thisRigidbody;

    [SerializeField]
    private CircleCollider2D triggerCollider;

    [SerializeField]
    private CircleCollider2D circleCollider;

    [SerializeField]
    private ParticleSystem flameBallParticles;

    [SerializeField]
    private ParticleSystem crazyBallParticles;

    [SerializeField]
    private ParticleSystem ballTrailParticles;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private TrailRenderer trailRenderer;

    private float radians;
    private Vector2 speed;
    private int bricksHitInARow;
    private float addedRepeatedVerticalBounce;
    private int hitWallsInARowOnlyCount;
    private int layerMovingUp;
    private int layerMovingDown;
    private int fireballLayerMovingUp;
    private int fireballLayerMovingDown;
    private float currentBallSpeed;
    private float ballMaxSpeed = 5;
    public bool activeAndMoving;
    private bool flameBallIsActive;
    private bool crazyBallIsActive;
    public bool ballIsEnabled;

    private void Awake()
    {
        // todo move these to the ball manager and reference them from there
        layerMovingUp = LayerMask.NameToLayer("BallMovingUp");
        layerMovingDown = LayerMask.NameToLayer("BallMovingDown");
        fireballLayerMovingDown = LayerMask.NameToLayer("FireBallMovingDown");
        fireballLayerMovingUp = LayerMask.NameToLayer("FireBallMovingUp");

        // get the max speed this ball can move for the current level
        DisableFlameBall();
        DisableCrazyBall();
    }

    public void PushFromBumper(Vector2 _force)
    {
        thisRigidbody.AddForce(_force, ForceMode2D.Force);
    }

    public void ActivateCrazyBall()
    {
        crazyBallIsActive = true;
        crazyBallParticles.Play();
    }

    public void DisableCrazyBall()
    {
        crazyBallIsActive = false;
        crazyBallParticles.Stop();
    }

    public void ActivateFlameBall()
    {
        flameBallIsActive = true;
        triggerCollider.enabled = true;
        flameBallParticles.Play();
    }

    public void DisableFlameBall()
    {
        triggerCollider.enabled = false;
        flameBallIsActive = false;
        flameBallParticles.Stop();
    }

    private void DisableBallTrail()
    {
        ballTrailParticles.Stop();
    }

    private void EnableBallTrail()
    {
        ballTrailParticles.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(CollisionTags.Brick))
        {
            return;
        }

        var _brick = other.gameObject.GetComponentInParent<BrickBase>();
        _brick.BrickHitByBall();
    }

    public override void LevelComplete()
    {
        CoreConnector.GameManager.ballManager.RemoveBall(this);
    }

    private void FixedUpdate()
    {
        // check if the y velocity ever reaches zero
        // check the speed of the ball and make it always locked
        currentBallSpeed = Mathf.Lerp(currentBallSpeed, ballMaxSpeed, Time.fixedDeltaTime);
        if (!activeAndMoving)
        {
            return;
        }

        var velocity = thisRigidbody.velocity;
        if (velocity.magnitude < currentBallSpeed)
        {
            thisRigidbody.velocity = velocity.normalized * currentBallSpeed;
        }

        // gradually slow down the ball as its moving too fast
        if (velocity.magnitude > ballMaxSpeed)
        {
            currentBallSpeed = velocity.magnitude;
            currentBallSpeed -= Time.fixedDeltaTime;
            thisRigidbody.velocity = velocity.normalized * currentBallSpeed;
        }

        if (crazyBallIsActive)
        {
            // crazy ball only gets applied if the ball is away from the bat, don't want it too hard for the player to hit
            if (transform.position.y > 2)
            {
                velocity = thisRigidbody.velocity;
                // modify the balls movement direction, but only if the balls y> 2(?)
                velocity.x += Random.Range(-.5f, .5f);
                velocity.y += Random.Range(-.5f, .5f);
                velocity = velocity.normalized * currentBallSpeed;
                thisRigidbody.velocity = velocity;
            }
        }

        SetBallLayer(velocity);
    }

    private void SetBallLayer(Vector2 velocity)
    {
        if (flameBallIsActive)
        {
            // fire ball is active
            circleCollider.gameObject.layer = velocity.y > 0 ? fireballLayerMovingUp : fireballLayerMovingDown;
        }
        else
        {
            // normal ball mode
            circleCollider.gameObject.layer = velocity.y > 0 ? layerMovingUp : layerMovingDown;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CoreConnector.GameManager.IsGamePlaying())
        {
            return;
        }

        if (collision.gameObject.CompareTag(CollisionTags.DeadZone))
        {
            CoreConnector.GameManager.ballManager.BallDestroyed(this);
        }

        if (collision.gameObject.CompareTag(CollisionTags.Wall))
        {
            CheckBounceOffWallAngle();
            PlaySound(SoundList.ballHitsWall);
        }

        if (collision.gameObject.CompareTag(CollisionTags.Shield))
        {
            PlaySound(SoundList.BallHitsShield);
        }

        if (collision.gameObject.CompareTag(CollisionTags.PlayersBat))
        {
            ProcessHitPlayersBat(collision);
        }

        if (collision.gameObject.CompareTag(CollisionTags.Brick))
        {
            ProcessHitABrick(collision);
        }
    }

    private void ProcessHitABrick(Collision2D collision)
    {
        var go = collision.gameObject;
        var brickBase = go.GetComponentInParent<BrickBase>();

        if (brickBase.BrickHasBeenDestroyed)
        {
            return;
        }

        resetBounceCheck();
        PlaySound(SoundList.ballHitsBrick);
        SpawnParticles(ParticleTypes.BallHitsBrick, collision.contacts[0].point);

        bricksHitInARow++;

        var intensity = bricksHitInARow / 6.0f;
        intensity = Mathf.Clamp(intensity, 0.1f, 1.0f);
        CoreConnector.GameManager.brickManager.ShakeGame(intensity);

        MultiBrickHitAward.EvaluateBrickHits(bricksHitInARow);
        brickBase.BrickHitByBall();
    }

    private void ProcessHitPlayersBat(Collision2D collision)
    {
        resetBounceCheck();
        PlaySound(SoundList.ballHitsBat);
        circleCollider.gameObject.layer = layerMovingUp;

        // if the players bat is not rotated much, then we deflect the ball based on the side of the bat the player is hitting
        speed = Vector2.zero;
        var positionDifference = transform.position - (Vector3) collision.rigidbody.position;
        if (Mathf.Abs(collision.rigidbody.rotation) < 3 && Mathf.Abs(positionDifference.x) > .2f)
        {
            // find the distance from the center of the bat
            speed.x = positionDifference.x;
            speed.y = 1;
            speed.Normalize();
            speed *= ballMaxSpeed;
        }
        else
        {
            radians = Mathf.Deg2Rad * (collision.rigidbody.rotation + 90);
            speed.x = Mathf.Cos(radians);
            speed.y = Mathf.Sin(radians);
            speed.Normalize();
            speed *= ballMaxSpeed;
        }

        thisRigidbody.velocity = speed;
        bricksHitInARow = 0;
    }

    private void CheckBounceOffWallAngle()
    {
        hitWallsInARowOnlyCount++;
        addedRepeatedVerticalBounce += thisRigidbody.velocity.y;

        if (hitWallsInARowOnlyCount < 4)
        {
            return;
        }

        var averageBounce = addedRepeatedVerticalBounce / hitWallsInARowOnlyCount;
        if (Mathf.Abs(averageBounce) > 0.5f)
        {
            return;
        }

// Debug.Log("the ball is getting stuck in a shallow bounce");
        var velocity = thisRigidbody.velocity;
        velocity.Normalize();
        velocity *= currentBallSpeed;
        if (velocity.y < 0)
        {
            velocity.y -= 0.6f;
        }
        else
        {
            velocity.y += 0.6f;
        }

        thisRigidbody.velocity = velocity;
    }

    private void resetBounceCheck()
    {
        addedRepeatedVerticalBounce = 0;
        hitWallsInARowOnlyCount = 0;
    }

    public void LaunchBall(float _ballMaxSpeed)
    {
        DisableCrazyBall();
        DisableFlameBall();
        DisableBallTrail();
        
        thisRigidbody.isKinematic = false;
        ballMaxSpeed = _ballMaxSpeed;

        // if there is only 1 brick left, then aim the ball at that brick
        if (CoreConnector.GameManager.brickManager.IsOneBrickLeft())
        {
            // get the last brick location
            var lastBrickLocation = CoreConnector.GameManager.brickManager.GetLastBrickLocation();
//			Debug.Log("Aim the ball at the last brick");
            speed = lastBrickLocation - transform.position;
            speed.Normalize();
            speed *= .1f;
        }
        else
        {
            speed.y = .1f;
            speed.x = Random.Range(-.05f, .05f);
        }

        thisRigidbody.AddForce(speed);
        activeAndMoving = true;
        hitWallsInARowOnlyCount = 0;

        trailRenderer.emitting = true;
        EnableBallTrail();
    }

    public void Enable()
    {
        ballIsEnabled = true;
        circleCollider.enabled = true;
        spriteRenderer.enabled = true;
    }

    public void Disable()
    {
        DisableBallTrail();
        hitWallsInARowOnlyCount = 0;
        ballIsEnabled = false;
        circleCollider.enabled = false;
        spriteRenderer.enabled = false;
        thisRigidbody.velocity = Vector2.zero;
        activeAndMoving = false;
        thisRigidbody.isKinematic = true;
        trailRenderer.emitting = false;
    }
}