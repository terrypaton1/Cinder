using UnityEngine;

public class RoundBumper : NonBrick
{
    [SerializeField]
    protected Animator bumperAnimation;

    private const string BumperAnimation = "RoundBumper1";


    public void CollideWithBall(Ball ball, Collision2D collision)
    {
        var force = collision.transform.position - transform.position;
        force = force.normalized * GameVariables.BumperPushForce;
        PlaySound(SoundList.RounderBumper);
        ball.PushFromBumper(force);
        bumperAnimation.Play(BumperAnimation, 0, 0.0f);

        Shake(1.0f);
    }
}