public class CrazyBall : PowerUpBase
{
    public override void Activate()
    {
        base.Activate();

        CoreConnector.GameUIManager.gameMessages.DisplayInGameMessage(Message.CrazyBall);

        CoreConnector.GameManager.ballManager.ActivateCrazyBall();
        CoreConnector.GameUIManager.DisplayPowerUpBar();
        maxTime =
            timer = GameVariables.CrazyBallLengthOfTime;
        PlaySound(SoundList.PowerUpCrazyBall);
    }

    public override void DisablePowerUp()
    {
        CoreConnector.GameManager.ballManager.DisableCrazyBall();
        base.DisablePowerUp();
    }
}