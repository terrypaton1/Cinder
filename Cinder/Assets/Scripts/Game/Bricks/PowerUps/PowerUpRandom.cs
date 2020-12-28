public class PowerUpRandom : PowerupBrick
{
    public override void EvaluateDisplay()
    {
        typeOfPowerUp = PowerupType.Random;
        _spriteRenderer.sprite = CoreConnector.GameSettings.powerUpRandom;
    }

    public override void ResetBrick()
    {
        InitializeBrick();
        base.ResetBrick();
    }

    protected override void StartItemFallingFromDestroyedBrick()
    {
        var randomPowerUpNum = UnityEngine.Random.Range(0, randomPowerupChoices.Length);
        var randomTypeOfPowerUp = randomPowerupChoices[randomPowerUpNum];
        CoreConnector.GameManager.fallingObjectsManager.AddFallingPowerUp(transform.position, randomTypeOfPowerUp);
    }
}