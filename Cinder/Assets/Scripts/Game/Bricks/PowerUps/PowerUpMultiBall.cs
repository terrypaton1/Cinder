public class PowerUpMultiBall : PowerupBrick
{
    public override void EvaluateDisplay()
    {
        typeOfPowerUp = PowerupType.MultiBall;
        _spriteRenderer.sprite = CoreConnector.GameManager.gameSettings.powerupMultiball;
    }
}