using UnityEngine;

public class RoundBumper : NonBrick
{
    [SerializeField]
    protected Animator bumperAnimation;

    private static string BumperAnimation = "RoundBumper1";

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        //topdo move the collision processing to the ball instead of here
        if (collision.gameObject.CompareTag(CollisionTags.Ball))
        {
            var _ball = collision.gameObject.GetComponent<Ball>();
            Vector2 force = collision.transform.position - transform.position;
            force = force.normalized * GameVariables.bumperPushForce;
            PlaySound(SoundList.RounderBumper);
            _ball.PushFromBumper(force);
            bumperAnimation.Play(BumperAnimation);
        }
    }
}