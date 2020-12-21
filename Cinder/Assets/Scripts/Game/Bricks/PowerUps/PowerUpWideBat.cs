public class PowerUpWideBat : PowerupBrick
{
    public override void EvaluateDisplay()
    {
        typeOfPowerUp = PowerupType.WideBat;
        _spriteRenderer.sprite = CoreConnector.GameManager.gameSettings.powerupWide;
    }
}