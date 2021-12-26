public class FreezePlayer : PowerUpBase
{
    public override void Activate()
    {
        base.Activate();
        CoreConnector.GameManager.playersBatManager.EnableFreezePlayer();
        PlaySound(SoundList.PowerUpFreeze);
        timer = GameVariables.FreezePlayerLengthOfTime;
    }

    public override void DisablePowerUp()
    {
        CoreConnector.GameManager.playersBatManager.DisableFreezePlayer();
        base.DisablePowerUp();
    }

    public override void DisableInstantly()
    {
        CoreConnector.GameManager.playersBatManager.DisableFreezePlayer();
        base.DisableInstantly();
    }
}