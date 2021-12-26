using UnityEngine;

public class PowerUpRandom : PowerupBrick
{
    public override void EvaluateDisplay()
    {
        typeOfPowerUp = PowerupType.Random;
        if (!Application.isPlaying)
        {
            return;
        }
        _spriteRenderer.sprite = CoreConnector.GameManager.gameSettings.powerUpRandom;
    }

    public override void ResetBrick()
    {
        InitializeBrick();
        base.ResetBrick();
    }

    protected override void StartItemFallingFromDestroyedBrick()
    {
        var randomPowerUpNum = Random.Range(0, randomPowerupChoices.Length);
        var randomTypeOfPowerUp = randomPowerupChoices[randomPowerUpNum];
        var position = transform.position;
        position.z += 0.5f;
        CoreConnector.GameManager.fallingObjectsManager.AddFallingPowerUp(position, randomTypeOfPowerUp);
    }
}