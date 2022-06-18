public class SplitBatPowerUp : PowerUpBase
{
   public override void Activate()
   {
      base.Activate();
      CoreConnector.GameManager.powerUpManager.DisableLaserBat();
      PlaySound(SoundList.PowerUpSplitBat);
      CoreConnector.GameManager.playersBatManager.ChangeToNewBat(PlayerBatTypes.Split);
      CoreConnector.GameUIManager.gameMessages.DisplayInGameMessage(Message.SplitBat);
      // power up disables immediately
      DisablePowerUp();
   }
}
