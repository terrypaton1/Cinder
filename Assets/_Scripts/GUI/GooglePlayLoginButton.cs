using UnityEngine;

public class GooglePlayLoginButton : BaseObject
{
    [SerializeField]
    protected UILabel textLabel;

    [SerializeField]
    protected GameObject loginPromptMessage;

    private Animator _animator;

    private bool playerLoggedIn;

    protected void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected void OnEnable()
    {
        Messenger<bool>.AddListener(GlobalEvents.LoginResult, LoginResult);
        Messenger.AddListener(SocialEvents.ConfirmSignoutOfSocial, CheckLoginState);
        Messenger.AddListener(SocialEvents.AttemptingLogin, AttemptingLogin);
        CheckLoginState();
    }

    protected void OnDisable()
    {
        Messenger<bool>.RemoveListener(GlobalEvents.LoginResult, LoginResult);
        Messenger.RemoveListener(SocialEvents.AttemptingLogin, AttemptingLogin);
        Messenger.RemoveListener(SocialEvents.ConfirmSignoutOfSocial, CheckLoginState);
    }

    protected void OnClick()
    {
        if (_animator != null)
        {
            _animator.Play("ButtonPressAnimation");
        }

        if (!playerLoggedIn)
        {
            Log("Trying to log the player in");

            Messenger.Broadcast(SocialEvents.AttemptingLogin, MessengerMode.DONT_REQUIRE_LISTENER);
            Messenger.Broadcast(SocialEvents.LoginUser);
        }
        else
        {
            Debug.Log("user is logged in");
            // display the confirm sign the player out dialog
            Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.ConfirmLogOut,
                MessengerMode.DONT_REQUIRE_LISTENER);
        }
    }

    private void AttemptingLogin()
    {
        PlayerPrefs.SetInt(DataVariables.playerHasLoggedIntoGooglePlayGames, 1);
        textLabel.text = "Logging in ...";
        CheckLoginState();
    }

    private void LoginResult(bool success)
    {
        Log("GooglePlayLoginButton:" + success);
        if (success)
        {
            // save game has been loaded, update any details we need to.
            CheckLoginState();
        }
        else
        {
            // user is not logged in
            playerLoggedIn = false;
            textLabel.text = "Log in";
            loginPromptMessage.SetActive(true);
        }
    }

    private void CheckLoginState()
    {
        if (PlayerPrefs.GetInt(DataVariables.playerHasLoggedIntoGooglePlayGames) == 0)
        {
            PlayerIsNotLoggedIn();
            return;
        }

//		Debug.Log("CheckLoginState");
        Log("CheckLoginState");

        if (Social.localUser.authenticated)
        {
//		if (PlayGamesPlatform.Instance.IsAuthenticated()) {
            // user is logged in
            GameVariables.instance.playerIsLoggedIn = true;
            playerLoggedIn = true;
            textLabel.text = "Logged in :)";
            loginPromptMessage.SetActive(false);
        }
        else
        {
            // user is not logged in
            PlayerIsNotLoggedIn();
            // start checking if the player is logged in repeqatedly, every few seconds
            if (gameObject.activeInHierarchy)
                Invoke("CheckLoginState", 2);
        }
    }

    private void PlayerIsNotLoggedIn()
    {
        playerLoggedIn = false;
        textLabel.text = "Log in";
        GameVariables.instance.playerIsLoggedIn = false;
        loginPromptMessage.SetActive(true);
    }
}