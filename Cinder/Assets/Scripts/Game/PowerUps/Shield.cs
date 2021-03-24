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

        yield return new WaitForSeconds(1.0f);

        DisableInstantly();
    }

    public override void Activate()
    {
        base.Activate();

        PlaySound(SoundList.PowerUpShield);
        CoreConnector.GameUIManager.powerupRemainingDisplay.DisplayPowerUpBar();

        maxTime =
            timer = GameVariables.powerUpShieldTotalTime;

        shieldCollider.enabled = true;
        sprite.enabled = true;

        shieldAnimator.Play(Constants.ActivateShield);
        shieldPulseShown = false;
    }

    public override void DisableInstantly()
    {
        base.DisableInstantly();

        shieldPulseShown = false;

        shieldCollider.enabled = false;
        sprite.enabled = true;
    }
}