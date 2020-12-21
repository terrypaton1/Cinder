using UnityEngine;

public class FallingLetter : FallingBase
{
    public string letter = "display0";

    private int _pointsValue;

    public void Setup(int newPointsValue, string _letter)
    {
        _pointsValue = newPointsValue;
        letter = _letter;

        spriteRenderer.enabled = true;
        spriteRenderer.sprite = CoreConnector.GameManager.gameSettings.GetLetter(letter);
        Disable();
        isFalling = false;
    }

    public override void HitPlayersBat()
    {
        CoreConnector.GameManager.scoreManager.PointsCollected(_pointsValue);
        CoreConnector.GameManager.bonusManager.LetterCollected(this);
        base.HitPlayersBat();
    }

    protected override void FellInToDeadZone()
    {
        CoreConnector.GameManager.bonusManager.LetterWasNotCollected(this);
        Disable();
    }
}