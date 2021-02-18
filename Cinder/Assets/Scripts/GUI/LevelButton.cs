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

    public int levelNumber = 1;
    public Image levelSprite;

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