public class PowerUpSmallBat : PowerupBrick
{
    public override void EvaluateDisplay()
    {
        typeOfPowerUp = PowerupType.SmallBat;
        _spriteRenderer.sprite =CoreConnector.GameManager.gameSettings.powerUpSmallBat;
    }
}