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
        var maxLevelBeaten = PlayerPrefs.GetInt(Constants.maxLevelBeatenPrefix);
#if UNITY_EDITOR
        Debug.Log("LEVEL SET TO 99");
        maxLevelBeaten = 99;
#endif
        var currentLevel = PlayerPrefs.GetInt(Constants.currentLevel);

        var buttonScale = new Vector3(1.0f, 1.0f, 1.0f);

        if (currentLevel == levelNumber)
        {
            buttonScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        transform.localScale = buttonScale;

        if (maxLevelBeaten < 1)
        {
            maxLevelBeaten = 1;
        }

        if (levelNumber <= maxLevelBeaten)
        {
            DisplayLevelUnLocked();
        }
        else
        {
            DisplayLevelLocked();
        }

        levelNumberLabel.text = levelNumber.ToString();
    }

    private void DisplayLevelLocked()
    {
        levelNumberLabel.enabled = false;
        levelSprite.enabled = false;
        lockedSprite.enabled = true;
        button.enabled = false;
    }

    private void DisplayLevelUnLocked()
    {
        // button is unlocked
        levelNumberLabel.enabled = true;
        levelSprite.enabled = true;
        lockedSprite.enabled = false;
        button.enabled = true;

        EvaluateIsBossLevel();
    }

    private void EvaluateIsBossLevel()
    {
        var isBossLevel = levelNumber % 5;
        if (isBossLevel == 0)
        {
            levelSprite.sprite = specialLevel;
            return;
        }

        levelSprite.sprite = normalLevel;
    }

    public void OnClick()
    {
        UIManager.LoadLevel(levelNumber);
    }
}