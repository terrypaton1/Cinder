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
    private const string PulseShield = "PulseShield";
    private const string DisableShield = "DisableShield";
    private const string ActivateShield = "ActivateShield";

    private IEnumerator coroutine;

    private void StopRunningCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    public override void ManagePowerUpLoop(float deltaTime)
    {
        if (!powerUpActive)
        {
            return;
        }

        timer -= deltaTime;
        
        var percentLeft = timer / GameVariables.crazyBallLengthOfTime;
        CoreConnector.GameUIManager.powerupRemainingDisplay.DisplayPercent(percentLeft);

        EvaluatePulseShield();

        if (timer <= 0)
        {
            DisablePowerUp();
        }
    }

    private void EvaluatePulseShield()
    {
        if (shieldPulseShown)
        {
            return;
        }

        if (timer < 1.5f)
        {
            shieldPulseShown = true;
            shieldAnimator.Play(PulseShield);
        }
    }

    public override void DisablePowerUp()
    {
        StopRunningCoroutine();

        coroutine = DisableShieldSequence();
        StartCoroutine(coroutine);
        shieldAnimator.Play(DisableShield);
    }

    private IEnumerator DisableShieldSequence()
    {
        powerUpActive = false;
        shieldCollider.enabled = false;

        yield return new WaitForSeconds(1.0f);

        DisableInstantly();
    }

    public override void Activate()
    {
        base.Activate();

        PlaySound(SoundList.PowerupShield);

        CoreConnector.GameUIManager.powerupRemainingDisplay.DisplayPowerUpBar();

        shieldPulseShown = false;
        timer = GameVariables.powerUpShieldTotalTime;
        shieldCollider.enabled = true;

        sprite.enabled = true;

        powerUpActive = true;
        shieldAnimator.Play(ActivateShield);
    }

    public override void DisableInstantly()
    {
        base.DisableInstantly();

        powerUpActive = false;
        timer = 0;
        shieldPulseShown = false;
        shieldCollider.enabled = false;

        sprite.enabled = true;
    }
}