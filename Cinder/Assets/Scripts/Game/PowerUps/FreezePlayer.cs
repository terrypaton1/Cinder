using UnityEngine;

public class FreezePlayer : PowerUpBase
{
    public override void Activate()
    {
        base.Activate();
        CoreConnector.GameManager.playersBatManager.EnableFreezePlayer();
        PlaySound(SoundList.PowerUpFreeze);
        timer = GameVariables.freezePlayerLengthOfTime;
    }

    public override void ManagePowerUpLoop(float deltaTime)
    {
        if (!powerUpActive)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            DisablePowerUp();
        }
    }

    public override void DisablePowerUp()
    {
        base.DisablePowerUp();
        CoreConnector.GameManager.playersBatManager.DisableFreezePlayer();
    }

    public override void DisableInstantly()
    {
        base.DisableInstantly();
        CoreConnector.GameManager.playersBatManager.DisableFreezePlayer();
    }
}