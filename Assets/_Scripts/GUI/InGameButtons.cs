using UnityEngine;
using UnityEngine.Serialization;

public class InGameButtons : MonoBehaviour
{
    [SerializeField]
    [FormerlySerializedAs("this_button")]
    public Buttons
        thisButtonID;

    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnClick()
    {
//		Debug.Log("oh hai"+thisButtonID);
        switch (thisButtonID)
        {
            case Buttons.PauseGame:
                TouchPosition.instance.PauseGame();
                Messenger.Broadcast(GlobalEvents.PauseGame);
                break;
            case Buttons.MainMenu:
                Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.MainMenu,
                    MessengerMode.DONT_REQUIRE_LISTENER);
                break;
            case Buttons.LevelChooser:
                Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.LevelChooser,
                    MessengerMode.DONT_REQUIRE_LISTENER);
                break;
            case Buttons.RestartLevel:
                // only allow restarting of the level if the player has more than 1 life left
                if (PlayerLifeManager.instance.PlayerLives > 0)
                    Messenger.Broadcast(MenuEvents.RestartGame);

                break;
            case Buttons.ResumeGame:
                Messenger.Broadcast(GlobalEvents.ResumeGame);
                break;
            case Buttons.QuitGame:
                Messenger<bool>.Broadcast(MenuEvents.DisplayPauseMenu, false);
                Messenger.Broadcast(GlobalEvents.QuitGame);
                break;
            case Buttons.NextLevel:
//				Debug.Log("NextLevel!");
                Messenger.Broadcast(MenuEvents.NextLevel, MessengerMode.DONT_REQUIRE_LISTENER);
                break;
        }
    }
}