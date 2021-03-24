public class LaserBatPowerUp : PowerUpBase
{
    public override void Activate()
    {
        base.Activate();

        maxTime = 
            timer = GameVariables.laserBatLengthOfTime;
        PlaySound(SoundList.PowerUpLaser);
        CoreConnector.GameManager.playersBatManager.ChangeToNewBat(PlayerBatTypes.Laser);
    }

    public override void DisablePowerUp()
    {
        base.DisablePowerUp();
        CoreConnector.GameManager.playersBatManager.ChangeToNewBat(PlayerBatTypes.Normal);
        powerUpActive = false;
        CoreConnector.GameManager.powerUpManager.TestDisablePowerUpBar();
    }
    
}