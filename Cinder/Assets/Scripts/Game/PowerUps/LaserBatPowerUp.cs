public class LaserBatPowerUp : PowerUpBase
{
    public override void Activate()
    {
        base.Activate();
        CoreConnector.GameUIManager.DisplayPowerUpBar();

        MAXTime = 
            Timer = GameVariables.LaserBatLengthOfTime;
        PlaySound(SoundList.PowerUpLaser);
        CoreConnector.GameManager.playersBatManager.ChangeToNewBat(PlayerBatTypes.Laser);
        CoreConnector.GameUIManager.gameMessages.DisplayInGameMessage(Message.LaserBat);
    }

    public override void DisablePowerUp()
    {
        base.DisablePowerUp();
        
        CoreConnector.GameManager.playersBatManager.ChangeToNewBat(PlayerBatTypes.Normal);
        PowerUpActive = false;
        CoreConnector.GameManager.powerUpManager.TestDisablePowerUpBar();
    }
}