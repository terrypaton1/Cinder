using UnityEngine;
using System.Collections;

public class TNTBrick : BrickBase
{
    private float explosionRange = 8.0f;

    protected override IEnumerator DestroyBrickSequence(bool playSound = true)
    {
        var position = transform.position;
        BrickHasBeenDestroyed = true;

        CoreConnector.GameManager.scoreManager.PointsCollected(brickPointsValue);
        CoreConnector.GameManager.brickManager.BrickDestroyed();

        SpawnParticles(ParticleTypes.TNTExplosion, position);
        if (playSound)
        {
            PlaySound(SoundList.TNTBrick);
        }

        yield return null;

        DisableVisuals();
        DisableColliders();

        CoreConnector.GameManager.brickManager.TestDestroyBricksAroundTNT(position, explosionRange, this);

        var counter = 0.5f;
        targetScale = 1.1f;

        while (counter > 0)
        {
            counter -= Time.deltaTime;
            yield return null;
        }
    }

    public override void UpdateHitsRemainingDisplay()
    {
        // TNT brick doesn't use this
    }

    public override void ResetBrick(float delay = -1)
    {
        explosionRange = GameVariables.TntExplosionRange;
        resetHitsToDestroyCount = amountOfHitsToDestroy;
        brickPointsValue = 50;

        base.ResetBrick(delay);
    }
}