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
        SpawnParticles(ParticleTypes.PowerupCollected, transform.position);
        CoreConnector.GameManager.powerupManager.ActivatePowerup(powerupType);
    }

    protected override void FellInToDeadZone()
    {
        base.FellInToDeadZone();

        var powerupLostEffectPosition = transform.position;
        powerupLostEffectPosition.y = 0;
        SpawnParticles(ParticleTypes.PowerupLost, powerupLostEffectPosition);
    }
}