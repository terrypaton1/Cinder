public class PowerUpSplitBat : PowerupBrick
{
    public override void EvaluateDisplay()
    {
        typeOfPowerUp = PowerupType.SplitBat;
        _spriteRenderer.sprite = CoreConnector.GameManager.gameSettings.powerupSplitBat;
    }
}