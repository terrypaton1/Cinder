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
//		Debug.Log("playerScore:"+playerScore);
        if (!bonusLife1Collected)
        {
            if (playerScore >= GameVariables.bonusLife1PointsThreshold)
            {
                // give player an extra life
                bonusLife1Collected = true;
                AwardExtraLife();
            }
        }

        if (!bonusLife2Collected)
        {
            if (playerScore >= GameVariables.bonusLife2PointsThreshold)
            {
                bonusLife2Collected = true;
                AwardExtraLife();
            }
        }

        if (!bonusLife3Collected)
        {
            if (playerScore >= GameVariables.bonusLife3PointsThreshold)
            {
                bonusLife3Collected = true;
                AwardExtraLife();
            }
        }

        if (bonusLife4Collected)
        {
            return;
        }

        if (playerScore >= GameVariables.bonusLife4PointsThreshold)
        {
            bonusLife4Collected = true;
            AwardExtraLife();
        }
    }

    private void AwardExtraLife()
    {
        ShowInGameMessage("Extra life!");
        CoreConnector.GameManager.playerLifeManager.GivePlayerExtraLife();
    }
}