//using UnityEngine;
using System.Collections;

public class NormalBat :  PlayersBatBase {
	public override void MorphToPlayState() {
//		Debug.Log("MorphToPlayState");
		MorphToPlayingAnimation.Play("NormalBatIntro");
	}
	public override void MorphToNormal() {
//		Debug.Log("MorphToNormal");
		MorphToPlayingAnimation.Play("NormalBatToNormal");
	}
	public override void PlayerLosesLife() {
//		Debug.Log("PlayerLosesLife");
		MorphToPlayingAnimation.Play("NormalBatPlayerLosesLife");
	}	
}
