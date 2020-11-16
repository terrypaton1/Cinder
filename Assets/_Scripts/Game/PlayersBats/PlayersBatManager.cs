#region

using System.Collections;
using UnityEngine;

#endregion

public enum PlayerBatTypes
{
    None,
    Normal,
    Wide,
    Small,
    Split,
    Laser
}

public class PlayersBatManager : BaseObject
{
    public TouchPosition touchPosition;

    [SerializeField]
    protected GameObject playersBatTrail;

    [SerializeField]
    protected PlayersBatBase _normalBat;

    [SerializeField]
    protected PlayersBatBase _wideBat;

    [SerializeField]
    protected PlayersBatBase _smallBat;

    [SerializeField]
    protected PlayersBatBase _splitBat;

    [SerializeField]
    protected PlayersBatBase _laserBat;

    private readonly float batYPosition = 4;
    private PlayersBatBase currentBat;
    private Vector3 currentBatPosition;

    private PlayerBatTypes currentBatType;
    private float currentRotation;
    private bool freezePlayerActive;
    private bool isMorphingBat;
    private float lastMovementDirection;
    private float lastXPosition;
    private float maximumXPosition;
    private float minimumXPosition;
    private PlayerBatTypes nextBatType;
    private bool playIsActive;
    private PlayerBatTypes previousBatType;
    private float targetRotation;

    private void Start()
    {
        currentBatPosition = new Vector3(0, GameVariables.playersBatYPosition, 0);
        minimumXPosition = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        maximumXPosition = Camera.main.ViewportToWorldPoint(Vector3.one).x;
        InvokeRepeating("MonitorPlayersPosition", 1, 0.2f);
        StartCoroutine(StartGameSequence());
    }

    private void FixedUpdate()
    {
        PositionAndRotateCurrentActiveBat();
    }

    private void OnEnable()
    {
        Messenger.AddListener(MenuEvents.RestartGame, RestartLevel);
        Messenger.AddListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.AddListener(MenuEvents.NextLevel, GoingToNextLevel);
        Messenger.AddListener(GlobalEvents.LifeLost, LoseLife);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener(MenuEvents.RestartGame, RestartLevel);
        Messenger.RemoveListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.RemoveListener(MenuEvents.NextLevel, GoingToNextLevel);
        Messenger.RemoveListener(GlobalEvents.LifeLost, LoseLife);
    }

    private void OnDestroy()
    {
        CancelInvoke("MonitorPlayersPosition");
        s_Instance = null;
    }

    private void PositionAndRotateCurrentActiveBat()
    {
        if (freezePlayerActive)
        {
            // emit particles where the player is			
            Messenger<ParticleTypes, Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.FreezePlayer,
                currentBat.rigidRef.position, MessengerMode.DONT_REQUIRE_LISTENER);
            return;
        }

        if (!playIsActive)
        {
            return;
        }

        var mousePosition = touchPosition.GetPlayersPosition();
        mousePosition.y = batYPosition;
        mousePosition.x = Mathf.Clamp(mousePosition.x, minimumXPosition, maximumXPosition);

        currentBatPosition = currentBat.rigidRef.position;
        // lerp towards the mouse position
        var newXPosition = Mathf.Lerp(currentBatPosition.x, mousePosition.x, Time.deltaTime * 60f);
        var dir = currentBatPosition.x - mousePosition.x;
        if (dir > 0 && lastMovementDirection < 0 || dir < 0 && lastMovementDirection > 0)
        {
            targetRotation = 0;
//			Debug.Log("changed direction");
        }

        lastMovementDirection = dir;
        currentBatPosition.x = newXPosition;
        // angle based on the amount moving
        var newDistance = lastXPosition - currentBat.rigidRef.position.x;
        if (Mathf.Abs(newDistance) > .02f)
        {
            targetRotation += (newDistance * 10);
        }

        targetRotation *= .9f;
        targetRotation = Mathf.Clamp(targetRotation, -60f, 60f);
        currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.deltaTime * 16f);
        // position the current bat
        currentBat.rigidRef.MovePosition(currentBatPosition);
        currentBat.rigidRef.MoveRotation(currentRotation);
        playersBatTrail.transform.position = currentBatPosition;
    }

    private IEnumerator StartGameSequence()
    {
        HideAllBats();
        ChangeToNewBat(PlayerBatTypes.Normal);
        yield return new WaitForSeconds(1);
        playIsActive = true;
    }

    public void ChangeToNewBat(PlayerBatTypes _newBatType)
    {
        if (_newBatType == currentBatType)
        {
            // this power up is already active
            return;
        }

        if (isMorphingBat)
        {
            StopCoroutine(TransitionToNextBatType());
        }

        previousBatType = currentBatType;
        nextBatType = _newBatType;
        // depending on the previous bat type, we need to transition to the next
        if (previousBatType != PlayerBatTypes.None)
        {
            Debug.Log("transition to next bat type:" + nextBatType);

            StopCoroutine(PlayerLosesLifeSequence());
            StartCoroutine(TransitionToNextBatType());
        }
        else
        {
            // no bat was previously enabled, switch the bat type instantly
            SwitchToNextBat();
            // might want a first reavel of the players bat, growing out of a small dot
            currentBat.MorphToPlayState();
        }
    }

    private void HideAllBats()
    {
        _normalBat.gameObject.SetActive(false);
        _wideBat.gameObject.SetActive(false);
        _smallBat.gameObject.SetActive(false);
        _splitBat.gameObject.SetActive(false);
        _laserBat.gameObject.SetActive(false);
    }

    private IEnumerator TransitionToNextBatType()
    {
        // first we transition the current bat to look normal
        isMorphingBat = true;
        if (currentBat != _normalBat)
        {
            currentBat.MorphToNormal();
            yield return new WaitForSeconds(1f);
        }

        // then we swap to the new bat (which starts looking like a normal bat)
        SwitchToNextBat();
        // then we morph to the playing state of the next bat

        if (currentBat != _normalBat)
        {
            currentBat.MorphToPlayState();
        }
        else
        {
            // make normal bat
            currentBat.MorphToNormal();
        }

        yield return 0;

        Debug.Log("Bat transition finished");
        isMorphingBat = false;
    }

    private void SwitchToNextBat()
    {
        HideAllBats();
        currentBatType = nextBatType;
        switch (nextBatType)
        {
            case PlayerBatTypes.Normal:
                currentBat = _normalBat;
                break;
            case PlayerBatTypes.Wide:
                currentBat = _wideBat;
                break;
            case PlayerBatTypes.Small:
                currentBat = _smallBat;
                break;
            case PlayerBatTypes.Split:
                currentBat = _splitBat;
                break;
            case PlayerBatTypes.Laser:
                currentBat = _laserBat;
                break;
        }

        currentBat.transform.position = currentBatPosition;
        currentBat.gameObject.SetActive(true);
    }

    private void MonitorPlayersPosition()
    {
        lastXPosition = currentBat.rigidRef.transform.position.x;
    }

    public void PlayerLosesLife()
    {
        playIsActive = false;
        freezePlayerActive = false;
        StartCoroutine(PlayerLosesLifeSequence());
    }

    private IEnumerator PlayerLosesLifeSequence()
    {
//		Debug.Log("PlayerLosesLifeSequence");
        Messenger.Broadcast(GlobalEvents.HideInGameMessage);
        currentBat.PlayerLosesLife();
        yield return new WaitForSeconds(1f);
        playIsActive = true;
        currentBatType = PlayerBatTypes.None;
        ChangeToNewBat(PlayerBatTypes.Normal);
    }

    private void LoseLife()
    {
        StopCoroutine(TransitionToNextBatType());
    }

    private void RestartLevel()
    {
        freezePlayerActive = false;
        StopCoroutine(TransitionToNextBatType());
    }

    private void LevelComplete()
    {
        freezePlayerActive = false;
    }

    private void GoingToNextLevel()
    {
        // reset the players bat
        freezePlayerActive = false;
    }

    public void ActivateFreezePlayer()
    {
        freezePlayerActive = true;
    }

    public void DisableFreezePlayer()
    {
        freezePlayerActive = false;
    }

    #region instance

    private static PlayersBatManager s_Instance;

    public static PlayersBatManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(PlayersBatManager)) as PlayersBatManager;
            }

            if (s_Instance == null)
            {
                Debug.Log("Could not locate an PlayersBatManager object!");
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