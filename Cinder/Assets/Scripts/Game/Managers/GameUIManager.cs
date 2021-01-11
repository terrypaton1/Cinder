using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField]
    protected GameObject buttonsHolder;

    [SerializeField]
    public PointsDisplay pointsDisplay;

    [SerializeField]
    public PlayerLifeDisplay playerLifeDisplay;

    [Space(10)]
    [SerializeField]
    public PowerupRemainingDisplay powerupRemainingDisplay;

    [SerializeField]
    public BossHealthRemainingDisplay bossHealthRemainingDisplay;

    [SerializeField]
    public GameMessages gameMessages;

    protected void OnEnable()
    {
        CoreConnector.GameUIManager = this;
        DisplayInGameButtons(false);
    }

    public void DisplayInGameButtons(bool displayButton)
    {
        if (displayButton)
        {
            buttonsHolder.SetActive(true);
            pointsDisplay.Show();
        }
        else
        {
            HidePowerUpBarInstantly();
            buttonsHolder.SetActive(false);
            pointsDisplay.Hide();
        }
    }

    public void PressPauseGame()
    {
        CoreConnector.UIManager.PressedPauseButton();
    }

    public void DisplayPowerUpBar()
    {
        powerupRemainingDisplay.DisplayPowerUpBar();
    }

    public void HidePowerUpBarInstantly()
    {
        powerupRemainingDisplay.HidePowerUpBarInstantly();
    }

    public void HidePowerUpBar()
    {
        powerupRemainingDisplay.HidePowerUpBar();
    }

    public void LifeLost()
    {
        bossHealthRemainingDisplay.Hide();
        HidePowerUpBar();
        gameMessages.LifeLost();
    }
}