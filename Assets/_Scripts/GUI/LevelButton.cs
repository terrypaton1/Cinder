using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
[SelectionBase]
#endif
public class LevelButton : MonoBehaviour
{
    [SerializeField]
    public int levelNumber = 1;

    [SerializeField]
    public UISprite levelSprite;

    [SerializeField]
    protected UISprite lockedSprite;

    [SerializeField]
    protected UILabel levelNumberLabel;

    [SerializeField]
    protected BoxCollider _collider;

    protected void OnEnable()
    {
        UpdateDisplay();
        _collider = GetComponent<BoxCollider>();
        Messenger<bool>.AddListener(GlobalEvents.LoginResult, LoginResult);
    }

    protected void OnDisable()
    {
        Messenger<bool>.RemoveListener(GlobalEvents.LoginResult, LoginResult);
    }

#if UNITY_EDITOR
    protected void OnGUI()
    {
        var nameShouldBe = DataVariables.levelNamePrefix + levelNumber;
        if (levelNumber < 10)
        {
            nameShouldBe = DataVariables.levelNamePrefix + "0" + levelNumber;
        }

        if (gameObject.name != nameShouldBe)
        {
            gameObject.name = nameShouldBe;
        }

        UpdateDisplay();
    }
#endif

    protected void LoginResult(bool success)
    {
        if (success)
        {
            // save game has been loaded, update any details we need to.
            UpdateDisplay();
        }
    }

    protected void UpdateDisplay()
    {
        var maxLevelNumber = PlayerPrefs.GetInt(DataVariables.maxLevelBeatenPrefix);
        if (maxLevelNumber < 1)
        {
            maxLevelNumber = 1;
        }
#if UNITY_EDITOR
        maxLevelNumber = 99;
#endif
        if (levelNumber <= maxLevelNumber)
        {
            // button is unlocked
            levelNumberLabel.gameObject.SetActive(true);
            levelSprite.gameObject.SetActive(true);
            lockedSprite.gameObject.SetActive(false);
            if (_collider)
                _collider.enabled = true;
        }
        else
        {
            // button is locked
            levelNumberLabel.gameObject.SetActive(false);
            levelSprite.gameObject.SetActive(false);
            lockedSprite.gameObject.SetActive(true);
            if (_collider)
                _collider.enabled = false;
        }

        levelNumberLabel.text = (levelNumber).ToString();
    }

    public void OnClick()
    {
        Messenger.Broadcast(GlobalEvents.StopLevelScroller, MessengerMode.DONT_REQUIRE_LISTENER);
        // set the current level to load
        PlayerPrefs.SetInt(DataVariables.currentLevel, levelNumber);
        Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.Game,
            MessengerMode.DONT_REQUIRE_LISTENER);
    }
}