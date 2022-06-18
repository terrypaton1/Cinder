using UnityEngine;

public class PowerUpBase : MonoBehaviour
{
    protected float MAXTime = 1.0f;
    protected float Timer;
    protected bool PowerUpActive;
    
    public virtual void Activate()
    {
        ResetTimer();
        PowerUpActive = true;
    }

    public virtual void DisablePowerUp()
    {
        PowerUpActive = false;
        ResetTimer();
    }

    private void ResetTimer()
    {
        Timer = 0.0f;
    }

    public bool IsPowerUpActive()
    {
        return PowerUpActive;
    }

    protected void PlaySound(SoundList sound)
    {
        CoreConnector.SoundManager.PlaySound(sound);
    }

    public virtual void DisableInstantly()
    {
        PowerUpActive = false;
        ResetTimer();
    }

    private void PowerUpTimeOver()
    {
        DisablePowerUp();
        CoreConnector.GameManager.powerUpManager.TestDisablePowerUpBar();
    }

    public virtual void ManagePowerUpTime()
    {
        if (!PowerUpActive)
        {
            return;
        }

        Timer -= Time.deltaTime;
        // display percent left
        var percentLeft = Timer / MAXTime;
        percentLeft = Mathf.Clamp01(percentLeft);
        CoreConnector.GameUIManager.powerupRemainingDisplay.DisplayPercent(percentLeft);
        if (Timer <= 0)
        {
            PowerUpTimeOver();
        }
    }
}