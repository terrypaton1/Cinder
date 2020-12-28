using UnityEngine;

public class LaserBullet : BaseObject
{
    private float currentLaserSpeed;
    private float laserMaxSpeed = 1;

    [SerializeField]
    protected Rigidbody2D thisRigidbody;

    public void Launch(Vector3 velocity)
    {
        laserMaxSpeed = GameVariables.laserBulletSpeed;
        thisRigidbody.velocity = velocity;
    }

    private void HitABrick(Vector2 particleSpawnPosition)
    {
        PlaySound(SoundList.LaserBulletHitsBrick);
        RePoolObject();
    }

    protected void FixedUpdate()
    {
        // check if the y velocity ever reaches zero
        // check the speed of the ball and make it always locked

        currentLaserSpeed = Mathf.Lerp(currentLaserSpeed, laserMaxSpeed, Time.deltaTime * 7.5f);
        var velocity = thisRigidbody.velocity;
        if (velocity.magnitude < currentLaserSpeed)
        {
            thisRigidbody.velocity = velocity.normalized * currentLaserSpeed;
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        var position = collision.contacts[0].point;
        SpawnParticles(ParticleTypes.LaserHitsBrick, position);

        if (collision.gameObject.CompareTag(CollisionTags.Brick))
        {
            HitABrick(position);
            var _brick = collision.gameObject.GetComponent<BrickBase>();
            _brick.BrickHitByBall();
            RePoolObject();
        }
        else
        {
            PlaySound(SoundList.LaserBulletHitsWall);
            RePoolObject();
        }
    }

    public override void LevelComplete()
    {
        RePoolObject();
    }

    private void RePoolObject()
    {
        // todo change to repooling the object
        Destroy(gameObject);
    }
}