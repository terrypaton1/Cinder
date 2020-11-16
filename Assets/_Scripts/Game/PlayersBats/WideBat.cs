public class WideBat :  PlayersBatBase {
	public override void MorphToPlayState() {
		MorphToPlayingAnimation.Play("WideBatIntro");
	}
	public override void MorphToNormal() {
		MorphToPlayingAnimation.Play("WideBatToNormal");
	}
	public override void PlayerLosesLife() {
		MorphToPlayingAnimation.Play("WideBatPlayerLosesLife");
	}
}
