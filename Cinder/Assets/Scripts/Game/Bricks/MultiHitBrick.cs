using UnityEngine;

public class MultiHitBrick : Brick
{
    [SerializeField]
    protected BrickMutltiHitSprites mutltiHitSpritesReference;

    public override void UpdateAmountOfHitsLeftDisplay()
    {
        mutltiHitSpritesReference.DisplayHitsLeft(amountOfHitsToDestroy);
    }

    protected override void DisableVisuals()
    {
        base.DisableVisuals();
        mutltiHitSpritesReference.Show();
    }

    public override void Hide()
    {
        base.Hide();
        mutltiHitSpritesReference.Hide();
        
    }

    public override void ResetBrick()
    {
        base.ResetBrick();
        
        mutltiHitSpritesReference.DisplayHitsLeft(amountOfHitsToDestroy);
    }
}