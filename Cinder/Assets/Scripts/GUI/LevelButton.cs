using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class LevelButton : MonoBehaviour
{
    [SerializeField]
    protected Image lockedSprite;

    [SerializeField]
    protected TextMeshProUGUI levelNumberLabel;

    [SerializeField]
    protected UnityEngine.UI.Button button;

    public int levelNumber = 1;
    public Image levelSprite;

    protected void OnEnable()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var maxLevelNumber = PlayerPrefs.GetInt(DataVariables.maxLevelBeatenPrefix);
        if (maxLevelNumber < 1)
        {
            maxLevelNumber = 1;
        }

        if (levelNumber <= maxLevelNumber)
        {
            // button is unlocked
            levelNumberLabel.enabled = true;
            levelSprite.enabled = true;
            lockedSprite.enabled = false;
            button.enabled = true;
        }
        else
        {
            // button is locked
//			levelSprite.gameObject.SetActive(true);
            levelNumberLabel.enabled = false;
            levelSprite.enabled = false;
            lockedSprite.enabled = true;
            button.enabled = false;
        }

        levelNumberLabel.text = (levelNumber).ToString();
    }

    public void OnClick()
    {
        CoreConnector.UIManager.LoadLevel(levelNumber);
    }
}