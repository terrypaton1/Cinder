using UnityEngine;

public class Ball : BaseObject
{
    [SerializeField]
    protected CircleCollider2D triggerCollider;

    public bool activeAndMoving;

    [SerializeField]
    protected CircleCollider2D circleCollider;

    [SerializeField]
    protected GameObject flameBallParticleHolder;

    [SerializeField]
    protected GameObject crazyBallParticleHolder;

    private float addedRepeatedVerticalBounce;

    private float ballMaxSpeed = 5;

    private int bricksHitInARow;

    private bool crazyBallIsActive;

    private float currentBallSpeed;

    private int fireballLayerMovingDown;

    private int fireballLayerMovingUp;

    private bool flameBallIsActive;
    private int hitWallsInARowOnlyCount;
    private int layerMovingDown;
    private int layerMovingUp;
    private float radians;
    private Vector2 speed;

    private Rigidbody2D thisRigidbody;

    protected void Awake()
    {
        thisRigidbody = GetComponentInChildren<Rigidbody2D>();
        layerMovingUp = LayerMask.NameToLayer("BallMovingUp");
        layerMovingDown = LayerMask.NameToLayer("BallMovingDown");
        fireballLayerMovingDown = LayerMask.NameToLayer("FireBallMovingDown");
        fireballLayerMovingUp = LayerMask.NameToLayer("FireBallMovingUp");
        // get the max speed this ball can move for the current level
        DisableFlameBall();
        DisableCrazyBall();
    }

    protected void FixedUpdate()
    {
        // check if the y velocity ever reaches zero
        // check the speed of the ball and make it always locked
        if (GameVariables.instance)
        {
//			Debug.Log(GameVariables.instance.ballMaxSpeed);
            currentBallSpeed = Mathf.Lerp(currentBallSpeed, ballMaxSpeed, Time.fixedDeltaTime);
            if (activeAndMoving)
            {
                var velocity = thisRigidbody.velocity;
                if (velocity.magnitude < currentBallSpeed)
                {
                    thisRigidbody.velocity = velocity.normalized * currentBallSpeed;
                }

                // gradually slow down the ball as its moving too fast
                if (velocity.magnitude > ballMaxSpeed)
                {
//					Debug.Log("slowing down the ball");
                    currentBallSpeed = velocity.magnitude;
                    currentBallSpeed -= Time.fixedDeltaTime;
//					currentBallSpeed *= .99f;
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

                if (flameBallIsActive)
                {
                    // fire ball is active

                    if (velocity.y > 0)
                    {
                        circleCollider.gameObject.layer = fireballLayerMovingUp;
                    }
                    else
                    {
                        circleCollider.gameObject.layer = fireballLayerMovingDown;
                    }
                }
                else
                {
                    // normal ball mode
                    if (velocity.y > 0)
                    {
                        circleCollider.gameObject.layer = layerMovingUp;
                    }
                    else
                    {
                        circleCollider.gameObject.layer = layerMovingDown;
                    }
                }
            }
        }
    }

    protected void OnEnable()
    {
        Messenger.AddListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.AddListener(GlobalEvents.ActivateFlameBall, ActivateFlameBall);
        Messenger.AddListener(GlobalEvents.DisableFlameBall, DisableFlameBall);
        Messenger.AddListener(GlobalEvents.DisableCrazyBall, DisableCrazyBall);
        Messenger.AddListener(GlobalEvents.ActivateCrazyBall, ActivateCrazyBall);
    }

    protected void OnDisable()
    {
//		Debug.Log("OnDisable");
        Messenger.RemoveListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.RemoveListener(GlobalEvents.ActivateFlameBall, ActivateFlameBall);
        Messenger.RemoveListener(GlobalEvents.DisableFlameBall, DisableFlameBall);
        Messenger.RemoveListener(GlobalEvents.DisableCrazyBall, DisableCrazyBall);
        Messenger.RemoveListener(GlobalEvents.ActivateCrazyBall, ActivateCrazyBall);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
//		Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("deadzone"))
        {
            BallManager.instance.BallDestroyed(this);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            CheckBounceOffWallAngle();
            PlaySound(SoundList.ballHitsWall);
        }

        if (collision.gameObject.CompareTag("Shield"))
        {
            PlaySound(SoundList.BallHitsShield);
        }

//		if (collision.gameObject.CompareTag("brick")) {
//			HitABrick();
//		}
        if (collision.gameObject.CompareTag("playersbat"))
        {
            //only allowed to hit balls during playing state
            if (GameManager.instance.gameState != GameState.Playing)
                return;
//			Debug.Log("<color=yellow>ball hit players bat</color>");
            resetBounceCheck();
            PlaySound(SoundList.ballHitsBat);
            circleCollider.gameObject.layer = layerMovingUp;
//			Debug.Log(collision.rigidbody.rotation);

// if the players bat is not rotated much, then we deflect the ball based on the side of the bat the player is hitting
//Time.timeScale=.3f;
            speed = Vector2.zero;
//			Debug.Log(collision.rigidbody.rotation);	
            var positionDifference = transform.position - (Vector3) collision.rigidbody.position;
//			Debug.Log("positionDifference.x:"+positionDifference.x);
            if (Mathf.Abs(collision.rigidbody.rotation) < 3 && Mathf.Abs(positionDifference.x) > .2f)
            {
//				Debug.Log("Players bat is not rotated much");
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
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
//		Debug.Log("other:" + other.gameObject.name, other.gameObject);
        if (other.gameObject.CompareTag("brick"))
        {
            var _brick = other.gameObject.GetComponentInParent<BrickBase>();
            _brick.BrickHitByBall();
        }
    }

    public void PushFromBumper(Vector2 _force)
    {
        thisRigidbody.AddForce(_force, ForceMode2D.Force);
    }

    private void ActivateCrazyBall()
    {
        crazyBallIsActive = true;
        crazyBallParticleHolder.SetActive(true);
    }

    private void DisableCrazyBall()
    {
        crazyBallIsActive = false;
        crazyBallParticleHolder.SetActive(false);
    }

    private void ActivateFlameBall()
    {
        flameBallIsActive = true;
        triggerCollider.enabled = true;
        flameBallParticleHolder.SetActive(true);
    }

    private void DisableFlameBall()
    {
        triggerCollider.enabled = false;
        flameBallIsActive = false;
        flameBallParticleHolder.SetActive(false);
    }

    private void LevelComplete()
    {
//		StopBall();
        BallManager.instance.RemoveBall(this);
//		Destroy(gameObject);
    }

    public void HitABrick()
    {
        resetBounceCheck();
        PlaySound(SoundList.ballHitsBrick);
        bricksHitInARow++;

        // Depending on the brick tpe, shake the game
        var intensity = bricksHitInARow / 6f;
        intensity = Mathf.Clamp(intensity, 0.1f, 1);
        Messenger<float>.Broadcast(GlobalEvents.ShakeGame, intensity);

        // give player bonus points
        if (bricksHitInARow == 7)
        {
            PlaySound(SoundList.MultiHit);
            Messenger<int>.Broadcast(GlobalEvents.PointsCollected, GameVariables.multihit1Points,
                MessengerMode.DONT_REQUIRE_LISTENER);
            ShowInGameMessage("MultiHit!");
        }

        if (bricksHitInARow == 12)
        {
            PlaySound(SoundList.MultiHitBrilliant);
            Messenger<int>.Broadcast(GlobalEvents.PointsCollected, GameVariables.multihit2Points,
                MessengerMode.DONT_REQUIRE_LISTENER);
            ShowInGameMessage("Brilliant!");
        }

        if (bricksHitInARow == 16)
        {
            PlaySound(SoundList.MultiHitExcellent);
            Messenger<int>.Broadcast(GlobalEvents.PointsCollected, GameVariables.multihit3Points,
                MessengerMode.DONT_REQUIRE_LISTENER);
            ShowInGameMessage("Excellent!");
        }

        if (bricksHitInARow == 20)
        {
            PlaySound(SoundList.MultiHitAwesome);
            Messenger<int>.Broadcast(GlobalEvents.PointsCollected, GameVariables.multihit3Points,
                MessengerMode.DONT_REQUIRE_LISTENER);
            ShowInGameMessage("Awesome!");
        }

        if (bricksHitInARow == 25)
        {
            PlaySound(SoundList.MultiHitWild);
            Messenger<int>.Broadcast(GlobalEvents.PointsCollected, GameVariables.multihit5Points,
                MessengerMode.DONT_REQUIRE_LISTENER);
            ShowInGameMessage("Wild!");
        }

        if (bricksHitInARow == 30)
        {
            PlaySound(SoundList.MultiHitMadness);
            Messenger<int>.Broadcast(GlobalEvents.PointsCollected, GameVariables.multihit6Points,
                MessengerMode.DONT_REQUIRE_LISTENER);
            ShowInGameMessage("Madness!");
        }

        if (bricksHitInARow == 40)
        {
            PlaySound(SoundList.MultiHitInsane);
            Messenger<int>.Broadcast(GlobalEvents.PointsCollected, GameVariables.multihit7Points,
                MessengerMode.DONT_REQUIRE_LISTENER);
            ShowInGameMessage("Insane!");
        }

        if (bricksHitInARow == 60)
        {
            PlaySound(SoundList.MultiHitUnbelievable);
            Messenger<int>.Broadcast(GlobalEvents.PointsCollected, GameVariables.multihit8Points,
                MessengerMode.DONT_REQUIRE_LISTENER);
            ShowInGameMessage("Unbelievable!");
        }
    }

    private void CheckBounceOffWallAngle()
    {
        hitWallsInARowOnlyCount++;
        addedRepeatedVerticalBounce += thisRigidbody.velocity.y;
        if (hitWallsInARowOnlyCount > 3)
        {
//			Debug.Log("hitWallsInARowOnlyCount:" + hitWallsInARowOnlyCount + " / " + thisRigidbody.velocity);
            var averageBounce = addedRepeatedVerticalBounce / hitWallsInARowOnlyCount;
//			Debug.Log("averageBounce:" + averageBounce);
            if (Mathf.Abs(averageBounce) < .5)
            {
//				Debug.Log("the ball is getting stuck in a shallow bounce");
                var velocity = thisRigidbody.velocity;
                velocity.Normalize();
                velocity *= currentBallSpeed;
                if (velocity.y < 0)
                {
                    velocity.y -= .6f;
                }
                else
                {
                    velocity.y += .6f;
                }

                thisRigidbody.velocity = velocity;
            }
        }
    }

    private void resetBounceCheck()
    {
        addedRepeatedVerticalBounce = 0;
        hitWallsInARowOnlyCount = 0;
    }

    public void LaunchBall(float _ballMaxSpeed)
    {
        ballMaxSpeed = _ballMaxSpeed;
        var tempSpeed = new Vector2();

        // if there is only 1 brick left, then aim the ball at that brick
        if (BrickManager.instance.activeBrickCount == 1)
        {
            // get the last brick location
            var lastBrickLocation = BrickManager.instance.GetLastBrickLocation();
//			Debug.Log("Aim the ball at the last brick");
            tempSpeed = lastBrickLocation - transform.position;
            tempSpeed.Normalize();
            tempSpeed *= .1f;
        }
        else
        {
            tempSpeed.y = .1f;
            tempSpeed.x = Random.Range(-.05f, .05f);
        }

        thisRigidbody.AddForce(tempSpeed);
        activeAndMoving = true;
        hitWallsInARowOnlyCount = 0;
    }
}