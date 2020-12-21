using UnityEngine;
using System.Collections;

public class PlayerLifeManager : BaseObject
{
    private int playerLives;

    public void Hide()
    {
        CoreConnector.GameUIManager.playerLifeDisplay.Hide();
    }

    public void Show()
    {
        CoreConnector.GameUIManager.playerLifeDisplay.Show();
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        playerLives = GameVariables.playerStartingLives;
        CoreConnector.GameUIManager.playerLifeDisplay.Show();
        CoreConnector.GameUIManager.playerLifeDisplay.UpdateLivesDisplay(playerLives);
    }

    public void RestartLevel()
    {
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