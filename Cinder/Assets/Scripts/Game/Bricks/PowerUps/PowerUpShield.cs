public class PowerUpShield : PowerupBrick
{
    public override void EvaluateDisplay()
    {
        typeOfPowerUp = PowerupType.Shield;
        _spriteRenderer.sprite = CoreConnector.GameManager.gameSettings.powerUpShield;
    }
}