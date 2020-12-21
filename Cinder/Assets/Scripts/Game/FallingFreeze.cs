using UnityEngine;

public class FallingFreeze : FallingBase
{
    public override void Setup()
    {
        isFalling = false;
    }

    public override void HitPlayersBat()
    {
        CoreConnector.GameManager.powerupManager.ActivatePowerup(PowerupType.FreezePlayer);
        base.HitPlayersBat();
    }
    
}