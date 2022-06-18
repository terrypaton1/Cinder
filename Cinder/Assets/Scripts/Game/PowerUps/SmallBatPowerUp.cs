public class SmallBatPowerUp : PowerUpBase
{
    public override void Activate()
    {
        base.Activate();
        
        CoreConnector.GameManager.powerUpManager.DisableLaserBat();
        PlaySound(SoundList.PowerUpSmallBat);
        CoreConnector.GameManager.playersBatManager.ChangeToNewBat(PlayerBatTypes.Small);
        CoreConnector.GameUIManager.gameMessages.DisplayInGameMessage(Message.SmallBat);
        // power up disables immediately
        DisablePowerUp();
    }
}
