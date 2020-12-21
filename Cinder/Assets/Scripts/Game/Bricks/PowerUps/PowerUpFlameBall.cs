public class PowerUpFlameBall : PowerupBrick
{
    public override void EvaluateDisplay()
    {
        typeOfPowerUp = PowerupType.FlameBall;
        _spriteRenderer.sprite = CoreConnector.GameManager.gameSettings.powerupFlameball;
    }
}