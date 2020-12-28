﻿public class FallingFreeze : FallingBase
{
    public override void Setup()
    {
        isFalling = false;
    }

    public override void HitPlayersBat()
    {
        CoreConnector.GameManager.powerUpManager.ActivatePowerUp(PowerupType.FreezePlayer);
        base.HitPlayersBat();
    }
    
}