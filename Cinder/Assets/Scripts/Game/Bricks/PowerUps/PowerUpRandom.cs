public class PowerUpRandom : PowerupBrick
{
    public override void EvaluateDisplay()
    {
        typeOfPowerUp = PowerupType.Random;
        _spriteRenderer.sprite = CoreConnector.GameSettings.powerupRandom;
    }

    public override void ResetBrick()
    {
        InitializeBrick();
        base.ResetBrick();
    }

    protected override void StartItemFallingFromDestroyedBrick()
    {
        int randomPowerUpNum = UnityEngine.Random.Range(0, randomPowerupChoices.Length);
        PowerupType randomTypeOfPowerUp = randomPowerupChoices[randomPowerUpNum];
        CoreConnector.GameManager.fallingObjectsManager.AddFallingPowerUp(transform.position, randomTypeOfPowerUp);
    }
}