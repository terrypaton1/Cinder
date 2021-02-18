using UnityEngine;

public class PowerUpBase : MonoBehaviour
{
    protected float maxTime = 1.0f;
    protected float timer;
    protected bool powerUpActive;

    public virtual void Activate()
    {
        ResetTimer();
        powerUpActive = true;
    }

    public virtual void DisablePowerUp()
    {
        powerUpActive = false;
        ResetTimer();
    }

    private void ResetTimer()
    {
        timer = 0.0f;
    }

    public bool IsPowerUpActive()
    {
        return powerUpActive;
    }

    protected void PlaySound(SoundList sound)
    {
        CoreConnector.SoundManager.PlaySound(sound);
    }

    public virtual void DisableInstantly()
    {
        powerUpActive = false;
        ResetTimer();
    }

    private void PowerUpTimeOver()
    {
        DisablePowerUp();
        CoreConnector.GameManager.powerUpManager.TestDisablePowerupBar();
    }

    public virtual void ManagePowerUpTime()
    {
        if (!powerUpActive)
        {
            return;
        }

        timer -= Time.deltaTime;
        // display percent left
        var percentLeft = timer / maxTime;
        percentLeft = Mathf.Clamp01(percentLeft);
        CoreConnector.GameUIManager.powerupRemainingDisplay.DisplayPercent(percentLeft);
        if (timer <= 0)
        {
            PowerUpTimeOver();
        }
    }
}