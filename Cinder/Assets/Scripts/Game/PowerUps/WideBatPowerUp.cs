public class WideBatPowerUp : PowerUpBase
{
   public override void Activate()
   {
      base.Activate();
      CoreConnector.GameManager.powerUpManager.DisableLaserBat();
      PlaySound(SoundList.PowerUpWideBat);
      CoreConnector.GameManager.playersBatManager.ChangeToNewBat(PlayerBatTypes.Wide);
      CoreConnector.GameUIManager.gameMessages.DisplayInGameMessage(Message.WideBat);
      // power up disables immediately
      DisablePowerUp();
   }
}
