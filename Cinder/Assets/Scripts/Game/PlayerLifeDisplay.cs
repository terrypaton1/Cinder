using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeDisplay : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI playerLivesText;

    [SerializeField]
    protected Image heartSprite;

    public void Show()
    {
        playerLivesText.enabled = true;
        heartSprite.enabled = true;
    }

    public void Hide()
    {
        playerLivesText.enabled = false;
        heartSprite.enabled = false;
    }

    public void UpdateLivesDisplay(int lives)
    {
        playerLivesText.text = lives.ToString();
    }
}