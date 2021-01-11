public class CrazyBall : PowerUpBase
{
    public override void Activate()
    {
        base.Activate();

        CoreConnector.GameUIManager.gameMessages.DisplayInGameMessage(Message.CrazyBall);

        CoreConnector.GameManager.ballManager.ActivateCrazyBall();
        CoreConnector.GameUIManager.DisplayPowerUpBar();
        timer = GameVariables.crazyBallLengthOfTime;
        PlaySound(SoundList.PowerUpCrazyBall);
    }

    public override void DisablePowerUp()
    {
        base.DisablePowerUp();
        if (!powerUpActive)
        {
            return;
        }

        CoreConnector.GameManager.ballManager.DisableCrazyBall();
    }
}