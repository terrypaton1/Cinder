public class LaserBatPowerUp : PowerUpBase
{
    public override void Activate()
    {
        base.Activate();

        timer = GameVariables.laserBatLengthOfTime;
        PlaySound(SoundList.PowerUpLaser);
        CoreConnector.GameManager.playersBatManager.ChangeToNewBat(PlayerBatTypes.Laser);
    }

    public override void ManagePowerUpLoop(float deltaTime)
    {
        if (!powerUpActive)
        {
            return;
        }
        timer -= deltaTime;
        // display percent left
        var percentLeft = timer / GameVariables.laserBatLengthOfTime;
        CoreConnector.GameUIManager.powerupRemainingDisplay.DisplayPercent(percentLeft);
        if (timer <= 0)
        {
            DisablePowerUp();
        }
    }

    public override void DisablePowerUp()
    {
        base.DisablePowerUp();
        CoreConnector.GameManager.playersBatManager.ChangeToNewBat(PlayerBatTypes.Normal);
        powerUpActive = false;
        CoreConnector.GameManager.powerUpManager.TestDisablePowerupBar();
    }
}