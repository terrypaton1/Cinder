using UnityEngine;

public class MultiHitBrick : Brick
{
    [SerializeField]
    protected BrickMultiHitSprites multiHitSpritesReference;

    public override void UpdateHitsRemainingDisplay()
    {
        multiHitSpritesReference.DisplayHitsLeft(amountOfHitsToDestroy);
    }

    protected override void DisableVisuals()
    {
        base.DisableVisuals();
        multiHitSpritesReference.Show();
    }

    public override void Hide()
    {
        base.Hide();
        multiHitSpritesReference.Hide();
    }

    public override void ResetBrick(float delay = -1.0f)
    {
        base.ResetBrick(delay);

        multiHitSpritesReference.DisplayHitsLeft(amountOfHitsToDestroy);
    }
}