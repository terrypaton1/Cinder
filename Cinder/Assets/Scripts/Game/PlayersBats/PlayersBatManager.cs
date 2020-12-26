using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayersBatManager : BaseObject
{
    [SerializeField]
    protected PlayersBatBase normalBat;

    [SerializeField]
    protected PlayersBatBase wideBat;

    [SerializeField]
    protected PlayersBatBase smallBat;

    [SerializeField]
    protected PlayersBatBase splitBat;

    [SerializeField]
    protected PlayersBatBase laserBat;

    private Dictionary<int, PlayersBatBase> allBats = new Dictionary<int, PlayersBatBase>();

    private PlayersBatBase currentBat;
    private Vector3 currentBatPosition;
    private PlayerBatTypes currentBatType = PlayerBatTypes.None;
    private float currentRotation;
    private bool freezePlayerActive;
    private bool isMorphingBat;
    private float lastMovementDirection;
    private float lastXPosition;
    private float maximumXPosition;
    private float minimumXPosition;
    private PlayerBatTypes nextBatType;
    private bool batIsActive;
    private PlayerBatTypes previousBatType;
    private float targetRotation;
    private readonly float batYPosition = 2.0f;
    private IEnumerator coroutine;
    private IEnumerator transitionCoroutine;

    private float timer;
    private readonly float TimerReset = 0.2f;

    protected void Start()
    {
        allBats.Add((int) PlayerBatTypes.Normal, normalBat);
        allBats.Add((int) PlayerBatTypes.Wide, wideBat);
        allBats.Add((int) PlayerBatTypes.Small, smallBat);
        allBats.Add((int) PlayerBatTypes.Split, splitBat);
        allBats.Add((int) PlayerBatTypes.Laser, laserBat);

        currentBatPosition = new Vector3(0, GameVariables.playersBatYPosition, 0);

        var cam = CoreConnector.GameManager.touchPosition.GetCamera();

        minimumXPosition = cam.ViewportToWorldPoint(Vector3.zero).x;
        maximumXPosition = cam.ViewportToWorldPoint(Vector3.one).x;
    }

    public void Reset()
    {
        StopTransitionCoroutine();
        currentBatType = PlayerBatTypes.None;
        HideAllBats();
        batIsActive = false;
    }

    protected void FixedUpdate()
    {
        if (!CoreConnector.GameManager.IsGamePlaying())
        {
            return;
        }

        if (currentBatType == PlayerBatTypes.None)
        {
            return;
        }

        // if th eplayer has an active bat
        PositionAndRotateCurrentActiveBat();

        ManageStorePlayersPosition();
    }

    private void ManageStorePlayersPosition()
    {
        timer += Time.deltaTime;
        if (timer > TimerReset)
        {
            timer -= TimerReset;

            lastXPosition = currentBat.rigidRef.transform.position.x;
        }
    }

    private void PositionAndRotateCurrentActiveBat()
    {
        if (freezePlayerActive)
        {
            // emit particles where the player is			
            SpawnParticles(ParticleTypes.FreezePlayer, currentBat.rigidRef.position);
            return;
        }

        if (!batIsActive)
        {
            return;
        }

        var mousePosition = CoreConnector.GameManager.touchPosition.GetPlayersPosition();
        mousePosition.y = batYPosition;
        mousePosition.x = Mathf.Clamp(mousePosition.x, minimumXPosition, maximumXPosition);

//		Log("mousePosition:"+mousePosition);
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

        currentBatPosition.y = GameVariables.playersBatYPosition;
        // position the current bat
        currentBat.rigidRef.MovePosition(currentBatPosition);
        currentBat.rigidRef.MoveRotation(currentRotation);
    }

    private void StopTransitionCoroutine()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
    }

    private IEnumerator StartGameSequence()
    {
        freezePlayerActive = false;
        HideAllBats();
        currentBatType = PlayerBatTypes.None;

        StopTransitionCoroutine();

        ChangeToNewBat(PlayerBatTypes.Normal);

        yield return new WaitForSeconds(1.0f);

        batIsActive = true;
    }

    public void ChangeToNewBat(PlayerBatTypes _newBatType)
    {
        if (_newBatType == currentBatType)
        {
            // this power up is already active
            // ensure that the bat is enabled, as in first level or restart
            return;
        }

        if (isMorphingBat)
        {
            StopTransitionCoroutine();
        }

        previousBatType = currentBatType;
        nextBatType = _newBatType;
        // depending on the previous bat type, we need to transition to the next
        if (previousBatType != PlayerBatTypes.None)
        {
            
            StopCoroutine(transitionCoroutine);

            transitionCoroutine = TransitionToNextBatType();
            StartCoroutine(transitionCoroutine);
        }
        else
        {
            // no bat was previously enabled, switch the bat type instantly
            SwitchToNextBat();
            // might want a first reveal of the players bat, growing out of a small dot
            if (currentBat != null)
            {
                currentBat.MorphToPlayState();
            }
        }
    }

    public void HideAllBats()
    {
        foreach (var batPair in allBats)
        {
            var bat = batPair.Value;
            bat.DisableBat();
        }
    }

    private IEnumerator TransitionToNextBatType()
    {
        // first we transition the current bat to look normal
        isMorphingBat = true;
        if (currentBat != normalBat)
        {
            currentBat.MorphToNormal();
            yield return new WaitForSeconds(1.0f);
        }

        // then we swap to the new bat (which starts looking like a normal bat)
        SwitchToNextBat();
        // then we morph to the playing state of the next bat

        if (currentBat != normalBat)
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

        if (nextBatType == PlayerBatTypes.None)
        {
            // no bat
        }
        else
        {
            currentBat = GetBat((int) nextBatType);
        }

        if (currentBat == null)
        {
            return;
        }

        currentBat.transform.position = currentBatPosition;
        currentBat.EnableBat();
    }

    public void PlayerLosesLife()
    {
        batIsActive = false;
        freezePlayerActive = false;
        // stop the player from moving onto a new bat type
        StopTransitionCoroutine();
        currentBat.PlayerLosesLife();
    }

    public void RestartLevel()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartGameSequence();
        StartCoroutine(coroutine);
    }

    public override void LevelComplete()
    {
        foreach (var batPair in allBats)
        {
            var bat = batPair.Value;
            bat.LevelComplete();
        }

        DisableFreezePlayer();

        currentBatType = PlayerBatTypes.None;
        batIsActive = true;
        ChangeToNewBat(PlayerBatTypes.Normal);
    }

    public void GoingToNextLevel()
    {
        // reset the players bat
        DisableFreezePlayer();
    }

    public void EnableFreezePlayer()
    {
        freezePlayerActive = true;
    }

    public void DisableFreezePlayer()
    {
        freezePlayerActive = false;
    }

    private PlayersBatBase GetBat(int batType)
    {
        PlayersBatBase bat;
        allBats.TryGetValue(batType, out bat);
        return bat;
    }
}