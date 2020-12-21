public class PowerUpLaserBat : PowerupBrick
{
    public override void EvaluateDisplay()
    {
        typeOfPowerUp = PowerupType.LaserBat;
        _spriteRenderer.sprite = CoreConnector.GameManager.gameSettings.powerupLaser;
    }
}