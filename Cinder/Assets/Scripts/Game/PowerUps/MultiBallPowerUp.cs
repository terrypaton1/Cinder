public class MultiBallPowerUp : PowerUpBase
{
   public override void Activate()
   {
      base.Activate();
      CoreConnector.GameUIManager.gameMessages.DisplayInGameMessage(Message.MultiBall);
      CoreConnector.GameManager.ballManager.AddMultiBalls(-.3f);
      PlaySound(SoundList.multiBall);
      // power up disables immediately
      DisablePowerUp();
   }
}
