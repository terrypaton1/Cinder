public class ScoreManager : BaseObject
{
    public int playerScore;

    private bool bonusLife1Collected;
    private bool bonusLife2Collected;
    private bool bonusLife3Collected;
    private bool bonusLife4Collected;

    public void PointsCollected(int _points)
    {
        playerScore += _points;
        CheckForBonusLife();
        // update the points display
        CoreConnector.GameUIManager.pointsDisplay.UpdatePointsDisplay(playerScore);
    }

    private void CheckForBonusLife()
    {
        // todo - this code could be much more efficiently made,
        // its just a bunch of 'a threshold value and a bool'

        EvaluateBonus1();
        EvaluateBonus2();
        EvaluateBonus3();
        EvaluateBonus4();
    }

    private void EvaluateBonus1()
    {
        if (playerScore < GameVariables.bonusLife1PointsThreshold)
        {
            return;
        }

        if (bonusLife1Collected)
        {
            return;
        }

        // give player an extra life
        bonusLife1Collected = true;
        AwardExtraLife();
    }

    private void EvaluateBonus2()
    {
        if (bonusLife2Collected) return;
        if (playerScore < GameVariables.bonusLife2PointsThreshold)
        {
            return;
        }

        bonusLife2Collected = true;
        AwardExtraLife();
    }

    private void EvaluateBonus3()
    {
        if (bonusLife3Collected)
        {
            return;
        }

        if (playerScore < GameVariables.bonusLife3PointsThreshold)
        {
            return;
        }

        bonusLife3Collected = true;
        AwardExtraLife();
    }

    private void EvaluateBonus4()
    {
        if (playerScore < GameVariables.bonusLife4PointsThreshold)
        {
            return;
        }

        if (bonusLife4Collected)
        {
            return;
        }

        bonusLife4Collected = true;
        AwardExtraLife();
    }

    private void AwardExtraLife()
    {
        ShowInGameMessage(Constants.ExtraLife);
        CoreConnector.GameManager.playerLifeManager.GivePlayerExtraLife();
    }
}