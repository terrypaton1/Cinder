using UnityEngine;

public class SmallBat : PlayersBatBase
{
    public override void MorphToPlayState()
    {
        MorphToPlayingAnimation.Play("SmallBatIntro");
    }

    public override void MorphToNormal()
    {
        MorphToPlayingAnimation.Play("SmallBatToNormal");
    }

    public override void PlayerLosesLife()
    {
        Debug.Log("small PlayerLosesLife");
        MorphToPlayingAnimation.Play("SmallBatPlayerLosesLife");
    }
}