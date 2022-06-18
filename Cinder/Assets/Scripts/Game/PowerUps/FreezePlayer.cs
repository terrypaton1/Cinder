public class FreezePlayer : PowerUpBase
{
    public override void Activate()
    {
        base.Activate();
        CoreConnector.GameManager.playersBatManager.EnableFreezePlayer();
        PlaySound(SoundList.PowerUpFreeze);
        Timer = GameVariables.FreezePlayerLengthOfTime;
        CoreConnector.GameUIManager.gameMessages.DisplayInGameMessage(Message.Freeze);
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