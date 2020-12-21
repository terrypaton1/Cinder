public class FlameBall : PowerUpBase
{
    public override void Activate()
    {
        base.Activate();

        PlaySound(SoundList.PowerupFireball);
        // tell all current balls to go into flame mode
        CoreConnector.GameManager.ActivateFlameBall();
        timer = GameVariables.flameBallLengthOfTime;
        PlaySound(SoundList.PowerupLaser);
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