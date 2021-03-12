using UnityEngine;
using UnityEngine.Serialization;

public class MultiHitBrick : Brick
{
    [FormerlySerializedAs("mutltiHitSpritesReference")]
    [SerializeField]
    protected BrickMultiHitSprites multiHitSpritesReference;

    public override void UpdateAmountOfHitsLeftDisplay()
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

    public override void ResetBrick()
    {
        base.ResetBrick();

        multiHitSpritesReference.DisplayHitsLeft(amountOfHitsToDestroy);
    }
}