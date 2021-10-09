using UnityEngine;

public class FallingPowerup : FallingBase
{
    [SerializeField]
    public PowerupType powerUpType;

    public override void LevelComplete()
    {
        isFalling = false;
        SpawnParticles(ParticleTypes.DestroyFallingItems, transform.position);
    }

    public void Setup(PowerupType newPowerupType)
    {
        powerUpType = newPowerupType;
        isFalling = false;
        // evaluate it's look.
    }

    public override void HitPlayersBat()
    {
        base.HitPlayersBat();
        SpawnParticles(ParticleTypes.PowerUpCollected, transform.position);
        CoreConnector.GameManager.powerUpManager.ActivatePowerUp(powerUpType);
    }

    public override string GetDashID()
    {
        return powerUpType.ToString();
    }
}