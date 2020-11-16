using System.Collections;
using UnityEngine;

public class InitialLoadScreen : BaseObject
{
    [SerializeField]
    protected UISprite loadingSprite;

    private readonly Vector3 rotationAmount = new Vector3(0, 0, -180f);

    protected void Start()
    {
        StartCoroutine(InitialLoadSequence());
//		Log("LoginUser");
    }

    protected void Update()
    {
        loadingSprite.transform.Rotate(rotationAmount * Time.deltaTime);
    }

    protected void OnEnable()
    {
        Messenger<bool>.AddListener(GlobalEvents.LoginResult, LoginResult);
    }

    protected void OnDisable()
    {
        Messenger<bool>.RemoveListener(GlobalEvents.LoginResult, LoginResult);
    }

    private IEnumerator InitialLoadSequence()
    {
        yield return new WaitForSeconds(.25f);

        if (PlayerPrefs.GetInt(DataVariables.playerHasLoggedIntoGooglePlayGames) == 1)
        {
            Messenger.Broadcast(SocialEvents.LoginUser);
        }

        Messenger<UIScreens>.Broadcast(GlobalEvents.DisplayUIScreen, UIScreens.MainMenu);
    }

    private void LoginResult(bool success)
    {
        if (success)
        {
            Debug.Log("Authenticated");
        }
        else
        {
            Debug.Log("Failed to authenticate");
        }

        // go to the main menu regardless of result
    }
}