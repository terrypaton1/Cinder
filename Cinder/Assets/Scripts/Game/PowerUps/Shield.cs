using System.Collections;
using UnityEngine;

public class Shield : PowerUpBase
{
    [SerializeField]
    protected Collider2D shieldCollider;

    [SerializeField]
    protected SpriteRenderer sprite;

    [SerializeField]
    protected Animator shieldAnimator;

    [SerializeField]
    protected ParticleSystem shieldParticles;

    private bool shieldPulseShown;

    private IEnumerator coroutine;

    private void StopRunningCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    public override void ManagePowerUpTime()
    {
        base.ManagePowerUpTime();
        if (!powerUpActive)
        {
            return;
        }

        EvaluatePulseShield();
    }

    private void EvaluatePulseShield()
    {
        if (timer > 1.5f)
        {
            return;
        }

        if (shieldPulseShown)
        {
            return;
        }

        shieldPulseShown = true;
        shieldAnimator.Play(Constants.PulseShield);
    }

    public override void DisablePowerUp()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        StopRunningCoroutine();

        coroutine = DisableShieldSequence();
        StartCoroutine(coroutine);
    }

    private IEnumerator DisableShieldSequence()
    {
        shieldAnimator.Play(Constants.DisableShield);

        powerUpActive = false;
        shieldCollider.enabled = false;
        CoreConnector.GameManager.powerUpManager.TestDisablePowerUpBar();
        shieldParticles.Stop();

        yield return new WaitForSeconds(1.0f);

        DisableInstantly();
    }

    public override void Activate()
    {
        base.Activate();

        PlaySound(SoundList.PowerUpShield);
        CoreConnector.GameUIManager.powerupRemainingDisplay.DisplayPowerUpBar();

        maxTime =
            timer = GameVariables.PowerUpShieldTotalTime;

        shieldCollider.enabled = true;
        sprite.enabled = true;

        shieldAnimator.Play(Constants.ActivateShield);
        shieldPulseShown = false;
        shieldParticles.Play();
    }

    public override void DisableInstantly()
    {
        base.DisableInstantly();

        shieldPulseShown = false;

        shieldParticles.Stop();
        shieldParticles.Clear();
        shieldCollider.enabled = false;
        sprite.enabled = false;
    }
}