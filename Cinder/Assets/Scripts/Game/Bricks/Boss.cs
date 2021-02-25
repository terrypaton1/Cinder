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

    protected void Awake()
    {
        UpdateAmountOfHitsLeftDisplay();
        // determine if boss can drop freezes
        var levelNumber = PlayerPrefs.GetInt(DataVariables.currentLevel);
        if (levelNumber >= GameVariables.bossesStartDroppingFreezesFromLevel)
        {
            canDropFreezePower = true;
        }
        fallingFreezeReference.Setup();
    }

    public override void BrickHitByBall()
    {
        if (canDropFreezePower)
        {
            ManageDropFreezePower();
        }

        base.BrickHitByBall();
        DisplayBossHealth();
    }

    private void ManageDropFreezePower()
    {
        // judge whether we should drop a freeze
        freezeDropTriggerCount++;
        // every 5 hits, the boss will drop a freeze, if one isn't falling already
        if (fallingFreezeReference.isFalling)
        {
            return;
        }

        // there is no freeze falling, we could drop another!
        if (freezeDropTriggerCount < GameVariables.bossDropFreezeTriggerCount)
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

        CoreConnector.GameManager.fallingObjectsManager.AddFallingPowerUp(position, PowerupType.FreezePlayer);
    }

    private void DisplayBossHealth()
    {
        if (amountOfHitsToDestroy > 0)
        {
            var percent = (float) amountOfHitsToDestroy / resetHitsToDestroyCount;
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
        ShowGooglyEyes();
        fallingFreezeReference.Setup();
        fallingFreezeReference.Disable();

        freezeDropTriggerCount = 0;

        brickPointsValue = Points.BossPointsValue;
        UpdateAmountOfHitsLeftDisplay();
        base.ResetBrick();
        CoreConnector.GameUIManager.bossHealthRemainingDisplay.DisplayBossHealthBar();
    }

    protected override void StartItemFallingFromDestroyedBrick()
    {
    }

    protected override IEnumerator DestroyBrickSequence(bool playSound = true)
    {
        StartItemFallingFromDestroyedBrick();

        BrickHasBeenDestroyed = true;
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
        while (visualScale.x > 0.1f)
        {
            visualScale.x *= 0.8f;
            visualScale.y *= 0.8f;
            visualObjects.transform.localScale = visualScale;
            yield return 0;
        }

        CoreConnector.GameManager.brickManager.BrickDestroyed(this);
        visualObjects.SetActive(false);
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