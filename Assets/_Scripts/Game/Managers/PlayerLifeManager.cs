using UnityEngine;
using System.Collections;

public class PlayerLifeManager : BaseObject
{
    private int playerLives;

    [SerializeField]
    protected UILabel playerLivesText;

    protected void Awake()
    {
        playerLives = GameVariables.playerStartingLives;
        UpdateLivesDisplay();
    }

    protected void OnEnable()
    {
        Messenger.AddListener(MenuEvents.RestartGame, RestartLevel);
    }

    protected void OnDisable()
    {
        Messenger.RemoveListener(MenuEvents.RestartGame, RestartLevel);
    }

    private void RestartLevel()
    {
        StopAllCoroutines();
    }

    public int PlayerLives
    {
        get { return playerLives; }
    }

    public void GivePlayerExtraLife()
    {
        PlaySound(SoundList.ExtraLife);
        playerLives++;
        UpdateLivesDisplay();
        // show a message
    }

    public void PlayerLosesALife()
    {
        StartCoroutine(PlayerLifeLostSequence());
    }

    private IEnumerator PlayerLifeLostSequence()
    {
//		Debug.Log("Life lost");
        PlaySound(SoundList.LifeLost);

        Messenger.Broadcast(GlobalEvents.LifeLost);
        // show a message that the player has lost a life
        playerLives--;
        UpdateLivesDisplay();
        PlayersBatManager.instance.PlayerLosesLife();
        yield return new WaitForSeconds(.5f);
        // if player loses all lives, game over
        if (playerLives < 1)
        {
            // game over;
            Messenger.Broadcast(GlobalEvents.GameOver);
            yield break;
        }

        // player still has lives left!
        // wait a little while
        yield return new WaitForSeconds(2f);
        // if the player has lives left, spawn a new ball
//		Debug.Log("add a new ball");
        BallManager.instance.AddNewBall();
        Messenger.Broadcast(GlobalEvents.ResumeLevelTimer, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    private void UpdateLivesDisplay()
    {
        // animate the text with a tween?
        playerLivesText.text = playerLives.ToString();
    }

    protected void OnDestroy()
    {
        s_Instance = null;
    }

    private static PlayerLifeManager s_Instance = null;

    public static PlayerLifeManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(PlayerLifeManager)) as PlayerLifeManager;
            }

            if (s_Instance == null)
            {
                Debug.LogError("Could not locate an PlayerLifeManager object!");
                Debug.Break();
            }

            return s_Instance;
        }
    }

    protected void OnApplicationQuit()
    {
        s_Instance = null;
    }
}