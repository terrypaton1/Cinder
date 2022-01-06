using UnityEngine;
using System.Collections;

public class Boss : BrickBase
{
    [SerializeField]
    protected FallingFreeze fallingFreezeReference;

    [SerializeField]
    protected GooglyEye[] googlyEyes;

    private int freezeDropTriggerCount;
    private bool canDropFreezePower;

    public override void Show()
    {
        amountOfHitsToDestroy = resetHitsToDestroyCount;
        UpdateAmountOfHitsLeftDisplay();
        // determine if boss can drop freezes
        var levelNumber = PlayerPrefs.GetInt(Constants.CurrentLevel);
        if (levelNumber >= GameVariables.BossesStartDroppingFreezesFromLevel)
        {
            canDropFreezePower = true;
        }

        fallingFreezeReference.Setup();

        ResetBrick();
    }

    public override void BrickHitByBall()
    {
        if (canDropFreezePower)
        {
            EvaluateDropNewFreezePowerUp();
        }

        base.BrickHitByBall();
        DisplayBossHealth();
    }

    private void EvaluateDropNewFreezePowerUp()
    {
        // judge whether we should drop a freeze
        freezeDropTriggerCount++;
        // every 5 hits, the boss will drop a freeze, if one isn't falling already
        if (fallingFreezeReference.isFalling)
        {
            return;
        }

        // there is no freeze falling, we could drop another!
        if (freezeDropTriggerCount < GameVariables.BossDropFreezeTriggerCount)
        {
            return;
        }

        // drop a freeze! 
        StartFallingFreeze();
        freezeDropTriggerCount = 0;
    }

    private void StartFallingFreeze()
    {
        var position = transform.position;
        position.x += Random.Range(-1.4f, 1.4f);
        position.z += 0.5f;
        CoreConnector.GameManager.fallingObjectsManager.AddFallingPowerUp(position, PowerupType.FreezePlayer);
    }

    private void DisplayBossHealth()
    {
        if (amountOfHitsToDestroy > 0)
        {
            var percent = (float) amountOfHitsToDestroy / resetHitsToDestroyCount;
            CoreConnector.GameUIManager.bossHealthRemainingDisplay.DisplayBossHealthBar();
            CoreConnector.GameUIManager.bossHealthRemainingDisplay.DisplayPercent(percent);
        }
        else
        {
            // hide the health bar
            CoreConnector.GameUIManager.bossHealthRemainingDisplay.HideBossHealthBar();
        }
    }

    public override void ResetBrick()
    {
        fallingFreezeReference.Setup();
        fallingFreezeReference.Disable();
        visualObjects.SetActive(true);

        freezeDropTriggerCount = 0;

        brickPointsValue = Points.BossPointsValue;
        base.ResetBrick();
        ShowGooglyEyes();
        CoreConnector.GameUIManager.bossHealthRemainingDisplay.HideBossHealthBar();
    }

    protected override void StartItemFallingFromDestroyedBrick()
    {
    }

    protected override IEnumerator DestroyBrickSequence(bool playSound = true)
    {
        BrickHasBeenDestroyed = true;
        StartItemFallingFromDestroyedBrick();

        // based on the boss starting health, award points
        var pointsForBoss = Points.BossPointsValue * resetHitsToDestroyCount;

        CoreConnector.GameManager.scoreManager.PointsCollected(pointsForBoss);
        SpawnParticles(ParticleTypes.BossExplosion, transform.position);

        if (playSound)
        {
            PlaySound(SoundList.brickDestroyed);
        }

        yield return 0;

        DisableColliders();

        LeanTween.cancel(visualObjects.gameObject);
        LeanTween.scale(visualObjects.gameObject, Vector3.one * 0.1f, 0.35f);
        yield return WaitCache.WaitForSeconds(0.35f);
        DisableVisuals();
        HideGooglyEyes();
        CoreConnector.GameManager.brickManager.BrickDestroyed();
    }

    public override void Hide()
    {
        base.Hide();
        fallingFreezeReference.Disable();
        HideGooglyEyes();
    }

    private void ShowGooglyEyes()
    {
        foreach (var eye in googlyEyes)
        {
            eye.Show();
        }
    }

    private void HideGooglyEyes()
    {
        foreach (var eye in googlyEyes)
        {
            eye.Hide();
        }
    }
}