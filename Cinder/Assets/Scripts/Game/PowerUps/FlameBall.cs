public class FlameBall : PowerUpBase
{
    public override void Activate()
    {
        base.Activate();

        PlaySound(SoundList.PowerUpFireball);
        CoreConnector.GameManager.brickManager.ActivateFlameBall();
        CoreConnector.GameManager.ballManager.ActivateFlameBall();
        Timer = GameVariables.FlameBallLengthOfTime;
        PlaySound(SoundList.PowerUpLaser);
        CoreConnector.GameUIManager.gameMessages.DisplayInGameMessage(Message.Fireball);
    }

    public override void DisablePowerUp()
    {
        base.DisablePowerUp();

        CoreConnector.GameManager.brickManager.DisableFlameBall();
        CoreConnector.GameManager.ballManager.DisableFlameBall();
    }
}