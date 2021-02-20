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
    protected Button button;

    [SerializeField]
    protected Image levelSprite;

    [SerializeField]
    protected Sprite normalLevel;

    [SerializeField]
    protected Sprite specialLevel;

    public int levelNumber = 1;

    protected void OnEnable()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var maxLevelNumber = PlayerPrefs.GetInt(DataVariables.maxLevelBeatenPrefix);

        var currentLevel = PlayerPrefs.GetInt(DataVariables.currentLevel);

        var buttonScale = new Vector3(1.0f, 1.0f, 1.0f);
        if (currentLevel == levelNumber)
        {
            buttonScale = new Vector3(1.4f, 1.4f, 1.4f);
        }

        transform.localScale = buttonScale;

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

            var isBossLevel = levelNumber % 5;

            if (isBossLevel == 0)
            {
                levelSprite.sprite = specialLevel;
            }
            else
            {
                levelSprite.sprite = normalLevel;
            }
        }
        else
        {
            // button is locked
            levelNumberLabel.enabled = false;
            levelSprite.enabled = false;
            lockedSprite.enabled = true;
            button.enabled = false;
        }

        levelNumberLabel.text = levelNumber.ToString();
    }

    public void OnClick()
    {
        UIManager.LoadLevel(levelNumber);
    }
}