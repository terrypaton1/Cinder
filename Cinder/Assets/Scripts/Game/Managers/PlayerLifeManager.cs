public class PlayerLifeManager : BaseObject
{
    private int playerLives;

    public void ResetPlayerLives()
    {
        playerLives = GameVariables.playerStartingLives;
    }

    public void RestartLevel()
    {
        ResetPlayerLives();
        StopAllCoroutines();
        CoreConnector.GameUIManager.playerLifeDisplay.Show();
        CoreConnector.GameUIManager.playerLifeDisplay.UpdateLivesDisplay(playerLives);
    }

    public int PlayerLives
    {
        get { return playerLives; }
    }

    public void GivePlayerExtraLife()
    {
        PlaySound(SoundList.ExtraLife);
        playerLives++;
        CoreConnector.GameUIManager.playerLifeDisplay.UpdateLivesDisplay(playerLives);
    }

    public void PlayerLosesALife()
    {
        playerLives--;
        CoreConnector.GameUIManager.playerLifeDisplay.UpdateLivesDisplay(playerLives);
    }
}