using UnityEngine;

public class PowerupBrick : BrickBase
{
    //todo move this to a random power up class
    [SerializeField]
    public PowerupType[] randomPowerupChoices;

    [SerializeField]
    public PowerupType typeOfPowerUp;

    [SerializeField]
    protected SpriteRenderer _spriteRenderer;

    public virtual void EvaluateDisplay()
    {
    }

    public override void UpdateAmountOfHitsLeftDisplay()
    {
        // powerup brick doesn't use this
    }

    public override void ResetBrick()
    {
        base.ResetBrick();
    }

    protected override void StartItemFallingFromDestroyedBrick()
    {
        CoreConnector.GameManager.fallingObjectsManager.AddFallingPowerUp(transform.position, typeOfPowerUp);
    }

    protected virtual void InitializeBrick()
    {
        brickPointsValue = 50;
        UpdateAmountOfHitsLeftDisplay();
        EvaluateDisplay();
    }

    public override void LevelComplete()
    {
        base.LevelComplete();
    }

    protected override void DisableVisuals()
    {
        base.DisableVisuals();
    }
}