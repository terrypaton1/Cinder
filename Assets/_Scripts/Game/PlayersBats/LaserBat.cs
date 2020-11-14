using UnityEngine;
using System.Collections;

public class LaserBat :  PlayersBatBase {

	float firingFrequency = 1;

	public override void MorphToPlayState() {
		MorphToPlayingAnimation.Play("LaserBatIntro");
	}

	public override void MorphToNormal() {
//		Debug.Log("transition to normal bat");
		MorphToPlayingAnimation.Play("LaserBatToNormal");
	}

	public override void PlayerLosesLife() {
		MorphToPlayingAnimation.Play("LaserBatPlayerLosesLife");
	}

	void OnEnable() {

		Messenger.AddListener(GlobalEvents.LifeLost, OnDisable);
		Messenger.AddListener(MenuEvents.LevelComplete, OnDisable);
		firingFrequency = GameVariables.laserBatFiringFrequency;
		InvokeRepeating("FireBullet", 0, firingFrequency);
	}

	void OnDisable() {

		Messenger.RemoveListener(GlobalEvents.LifeLost, OnDisable);
		Messenger.RemoveListener(MenuEvents.LevelComplete, OnDisable);
		CancelInvoke("FireBullet");
	}

	void FireBullet() {
//		Debug.Log("FireBullet");
		PlaySound(SoundList.LaserBulletFiring);
//		float rad = Mathf.Deg2Rad * (rigidRef.rotation + 90);			
		Vector2 firingVelocity = new Vector2(0, .1f);
		Messenger<Vector3,Vector3>.Broadcast(GlobalEvents.FireLaser, transform.position, firingVelocity);
	}

}
