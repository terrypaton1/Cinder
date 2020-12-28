using UnityEngine;

public class PowerupBrick : BrickBase
{
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
}