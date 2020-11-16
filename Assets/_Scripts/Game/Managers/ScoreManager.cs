using System;
using UnityEngine;

public class ScoreManager : BaseObject
{
    private bool bonusLife1Collected;
    private bool bonusLife2Collected;
    private bool bonusLife3Collected;
    private bool bonusLife4Collected;

    [NonSerialized]
    public int playerScore;

    protected void OnEnable()
    {
        Messenger.AddListener(MenuEvents.RestartGame, RestartLevel);
        Messenger<int>.AddListener(GlobalEvents.PointsCollected, PointsCollected);
    }

    protected void OnDisable()
    {
        Messenger.RemoveListener(MenuEvents.RestartGame, RestartLevel);
        Messenger<int>.RemoveListener(GlobalEvents.PointsCollected, PointsCollected);
    }

    private void RestartLevel()
    {
//		Debug.Log("RestartLevel");
        playerScore = 0;
        Messenger<int>.Broadcast(MenuEvents.UpdatePointsDisplay, playerScore, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    private void PointsCollected(int _points)
    {
        playerScore += _points;
        CheckForBonusLife();
        // update the ppoints display
        Messenger<int>.Broadcast(MenuEvents.UpdatePointsDisplay, playerScore, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    private void CheckForBonusLife()
    {
//		Debug.Log("playerScore:"+playerScore);
//		Debug.Log("GameVariables.instance.bonusLife1PointsThreshold:"+GameVariables.instance.bonusLife1PointsThreshold);
        if (!bonusLife1Collected)
        {
            if (playerScore >= GameVariables.bonusLife1PointsThreshold)
            {
                // give player an extra life
                bonusLife1Collected = true;
                ShowInGameMessage("Extra life!");
                PlayerLifeManager.instance.GivePlayerExtraLife();
            }
        }

        if (!bonusLife2Collected)
        {
            if (playerScore >= GameVariables.bonusLife2PointsThreshold)
            {
                bonusLife2Collected = true;
                ShowInGameMessage("Extra life!");
                PlayerLifeManager.instance.GivePlayerExtraLife();
            }
        }

        if (!bonusLife3Collected)
        {
            if (playerScore >= GameVariables.bonusLife3PointsThreshold)
            {
                bonusLife3Collected = true;
                ShowInGameMessage("Extra life!");
                PlayerLifeManager.instance.GivePlayerExtraLife();
            }
        }

        if (!bonusLife4Collected)
        {
            if (playerScore >= GameVariables.bonusLife4PointsThreshold)
            {
                bonusLife4Collected = true;
                ShowInGameMessage("Extra life!");
                PlayerLifeManager.instance.GivePlayerExtraLife();
            }
        }
    }

    #region instance

    protected void OnDestroy()
    {
        s_Instance = null;
    }

    private static ScoreManager s_Instance;

    public static ScoreManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(ScoreManager)) as ScoreManager;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                Debug.LogError("Could not locate an ScoreManager object!");
                Debug.Break();
            }

            return s_Instance;
        }
    }

    void OnApplicationQuit()
    {
        s_Instance = null;
    }

    #endregion
}