using UnityEngine;
using System.Collections;

public class SplitBat :  PlayersBatBase {
	public override void MorphToPlayState() {
		MorphToPlayingAnimation.Play("SplitBatIntro");
	}
	public override void MorphToNormal() {
		MorphToPlayingAnimation.Play("SplitBatToNormal");
	}
	public override void PlayerLosesLife() {
		MorphToPlayingAnimation.Play("SplitBatPlayerLosesLife");
	}
}
