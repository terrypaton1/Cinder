public class PlayerLifeManager : BaseObject
{
    public int PlayerLives
    {
        get;
        private set;
    }

    public void ResetPlayerLives()
    {
        PlayerLives = GameVariables.PlayerStartingLives;
    }

    public void RestartLevel()
    {
        StopAllCoroutines();
        CoreConnector.GameUIManager.playerLifeDisplay.Show();
        UpdateLivesDisplay(PlayerLives);
    }

    public void GivePlayerExtraLife()
    {
        PlayerLives++;
        UpdateLivesDisplay(PlayerLives);
    }

    public void PlayerLosesALife()
    {
        PlayerLives--;
        UpdateLivesDisplay(PlayerLives);
    }

    private static void UpdateLivesDisplay(int value)
    {
        CoreConnector.GameUIManager.playerLifeDisplay.UpdateLivesDisplay(value);
    }
}