public class FlameBall : PowerUpBase
{
    public override void Activate()
    {
        base.Activate();

        PlaySound(SoundList.PowerUpFireball);
        // tell all current balls to go into flame mode
        CoreConnector.GameManager.ActivateFlameBall();
        timer = GameVariables.flameBallLengthOfTime;
        PlaySound(SoundList.PowerUpLaser);
    }

    public override void ManagePowerUpLoop(float deltaTime)
    {
        if (!powerUpActive)
        {
            return;
        }

        timer -= deltaTime;
        if (timer <= 0)
        {
            CoreConnector.GameManager.DisableFlameBall();
        }
    }
}