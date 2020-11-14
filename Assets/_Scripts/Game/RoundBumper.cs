using UnityEngine;

public class RoundBumper : BaseObject
{
    Animator bumperAnimation;
    public float bumperPower = 2f;

    protected void Start()
    {
        //todo replace this with a serialized version
        bumperAnimation = GetComponent<Animator>();
        bumperPower = GameVariables.bumperPushForce;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Ball _ball = collision.gameObject.GetComponent<Ball>();
// work out dirction to push ball
// animate the bumper
// work out the direction
            Vector2 force = collision.transform.position - transform.position;
            force = force.normalized * bumperPower;
            PlaySound(SoundList.RounderBumper);
//			Debug.Log("force:" + force);
            _ball.PushFromBumper(force);
            bumperAnimation.Play("RoundBumper1");
        }
    }
}