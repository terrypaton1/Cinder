using UnityEngine;

public class PowerUpBase : MonoBehaviour
{
    protected float timer;
    protected bool powerUpActive;

    public virtual void ManagePowerUpLoop(float deltaTime)
    {
    }

    public virtual void Activate()
    {
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
    }
}