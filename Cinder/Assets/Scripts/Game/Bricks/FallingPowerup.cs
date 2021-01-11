public class FallingPowerup : FallingBase
{
    private PowerupType powerupType;

    public override void LevelComplete()
    {
        isFalling = false;
        SpawnParticles(ParticleTypes.DestroyFallingItems, transform.position);
    }

    public void Setup(PowerupType newPowerupType)
    {
        powerupType = newPowerupType;
        isFalling = false;
    }

    public override void HitPlayersBat()
    {
        base.HitPlayersBat();
        SpawnParticles(ParticleTypes.PowerUpCollected, transform.position);
        CoreConnector.GameManager.powerUpManager.ActivatePowerUp(powerupType);
    }

    protected override void FellInToDeadZone()
    {
        base.FellInToDeadZone();

        var powerupLostEffectPosition = transform.position;
        powerupLostEffectPosition.y = 0;
        SpawnParticles(ParticleTypes.PowerUpLost, powerupLostEffectPosition);
    }
}