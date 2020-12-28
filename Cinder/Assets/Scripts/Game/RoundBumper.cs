using UnityEngine;

public class RoundBumper : NonBrick
{
    [SerializeField]
    protected Animator bumperAnimation;

    private const string BumperAnimation = "RoundBumper1";

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        //todo move the collision processing to the ball instead of here
        if (!collision.gameObject.CompareTag(CollisionTags.Ball))
        {
            return;
        }

        var _ball = collision.gameObject.GetComponent<Ball>();
        var force = collision.transform.position - transform.position;
        force = force.normalized * GameVariables.bumperPushForce;
        PlaySound(SoundList.RounderBumper);
        _ball.PushFromBumper(force);
        bumperAnimation.Play(BumperAnimation);
    }
}