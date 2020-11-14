using UnityEngine;
using System.Collections;

public class RoundBumper : BaseObject {
	/// <summary>
	/// The bumper animation.
	/// </summary>
	Animator bumperAnimation;
	/// <summary>
	/// The power.
	/// </summary>
	public	float bumperPower = 2f;
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		bumperAnimation = GetComponent<Animator>();
		bumperPower = GameVariables.bumperPushForce;
	}


	/// <summary>
	/// Raises the collision enter2D event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnCollisionEnter2D(Collision2D collision) {

//		Debug.Log(collision.gameObject.tag);
		if (collision.gameObject.CompareTag("Ball")) {
			// tell the ball you hit me
			Ball _ball = collision.gameObject.GetComponent<Ball>();
//			_ball.HitABrick();
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
