public class PowerUpCrazyBall : PowerupBrick
{
    public override void EvaluateDisplay()
    {
        typeOfPowerUp = PowerupType.CrazyBall;
        _spriteRenderer.sprite = CoreConnector.GameManager.gameSettings.powerupCrazyBall;
    }
}