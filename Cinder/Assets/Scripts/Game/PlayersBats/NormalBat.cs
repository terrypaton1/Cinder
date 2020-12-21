public class NormalBat : PlayersBatBase
{
    public override void MorphToPlayState()
    {
        MorphToPlayingAnimation.Play("NormalBatIntro");
    }

    public override void MorphToNormal()
    {
        MorphToPlayingAnimation.Play("NormalBatToNormal");
    }

    public override void PlayerLosesLife()
    {
        MorphToPlayingAnimation.Play("NormalBatPlayerLosesLife");
    }
}