public class CrazyBall : PowerUpBase
{
    public override void ManagePowerUpLoop(float deltaTime)
    {
        if (!powerUpActive)
        {
            return;
        }

        timer -= deltaTime;
        // display percent left
        var percentLeft = timer / GameVariables.crazyBallLengthOfTime;
        CoreConnector.GameUIManager.powerupRemainingDisplay.DisplayPercent(percentLeft);

        if (timer > 0.0f)
        {
            return;
        }

        CoreConnector.GameManager.ballManager.DisableCrazyBall();
        DisablePowerUp();
    }

    public override void Activate()
    {
        base.Activate();

        CoreConnector.GameUIManager.gameMessages.DisplayInGameMessage(Message.CrazyBall);

        CoreConnector.GameManager.ballManager.ActivateCrazyBall();
        timer = GameVariables.crazyBallLengthOfTime;
        PlaySound(SoundList.PowerupCrazyBall);
    }
}