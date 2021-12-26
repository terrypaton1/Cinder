using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private readonly Dictionary<int, PlayersBatBase> allBats = new Dictionary<int, PlayersBatBase>();

    private PlayersBatBase currentBat;
    private Vector3 currentBatPosition;

    private bool freezePlayerActive;
    private bool isMorphingBat;
    private bool batIsActive;
    private float lastMovementDirection;
    private float lastXPosition;
    private float maximumXPosition;
    private float minimumXPosition;
    private float currentRotation;
    private float targetRotation;

    private PlayerBatTypes currentBatType = PlayerBatTypes.None;
    private PlayerBatTypes nextBatType;
    private PlayerBatTypes previousBatType;

    private const float batYPosition = 2.0f;

    private IEnumerator coroutine;
    private IEnumerator transitionCoroutine;

    private float timer;
    private const float TimerReset = 0.2f;

    protected void Start()
    {
        allBats.Add((int) PlayerBatTypes.Normal, normalBat);
        allBats.Add((int) PlayerBatTypes.Wide, wideBat);
        allBats.Add((int) PlayerBatTypes.Small, smallBat);
        allBats.Add((int) PlayerBatTypes.Split, splitBat);
        allBats.Add((int) PlayerBatTypes.Laser, laserBat);

        currentBatPosition = new Vector3(0, GameVariables.PlayersBatYPosition, 0);

        var cam = CoreConnector.GameManager.touchPosition.GetCamera();

        minimumXPosition = cam.ViewportToWorldPoint(Vector3.zero).x;
        maximumXPosition = cam.ViewportToWorldPoint(Vector3.one).x;
    }

    public void Reset()
    {
        StopTransitionCoroutine();
        StopRunningCoroutine();
        currentBatType = PlayerBatTypes.None;
        HideAllBats();
        batIsActive = false;
    }

    protected void FixedUpdate()
    {
        if (!CoreConnector.GameManager.IsGamePlaying())
        {
            //     return;
        }

        if (currentBatType == PlayerBatTypes.None)
        {
            return;
        }

        // if the player has an active bat
        PositionAndRotateCurrentActiveBat();
        ManageStorePlayersPosition();
    }

    private void ManageStorePlayersPosition()
    {
        timer += Time.deltaTime;
        if (!(timer > TimerReset))
        {
            return;
        }

        timer -= TimerReset;

        lastXPosition = currentBat.rigidRef.transform.position.x;
    }

    public void PositionAndRotateCurrentActiveBat()
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

        currentBatPosition = currentBat.rigidRef.position;
        // lerp towards the mouse position
        var newXPosition = Mathf.Lerp(currentBatPosition.x, mousePosition.x, Time.deltaTime * 60f);
        var dir = currentBatPosition.x - mousePosition.x;

        // if the player has changed direction completely then first aim for a zero rotation
        if (dir > 0.0f && lastMovementDirection < 0.0f || dir < 0.0f && lastMovementDirection > 0.0f)
        {
            // player is changing direction
            targetRotation = 0.0f;
        }

        lastMovementDirection = dir;
        // Calculate the angle based on the amount the bat is moving
        var newDistance = lastXPosition - currentBat.rigidRef.position.x;
        if (Mathf.Abs(newDistance) > 0.02f)
        {
            targetRotation += newDistance * 10;
        }

        targetRotation *= 0.9f;
        targetRotation = Mathf.Clamp(targetRotation, -60f, 60f);
        currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.deltaTime * 16f);

        currentBat.rigidRef.MoveRotation(currentRotation);

        currentBatPosition.x = newXPosition;
        currentBatPosition.y = GameVariables.PlayersBatYPosition;
        // position the current bat
        currentBat.rigidRef.MovePosition(currentBatPosition);
    }

    private IEnumerator StartGameSequence()
    {
        StopTransitionCoroutine();
        HideAllBats();
        currentBatType = PlayerBatTypes.None;
        ChangeToNewBat(PlayerBatTypes.Normal);
        currentBat.EnableBat();
        freezePlayerActive = false;
        batIsActive = true;

        yield return new WaitForSeconds(1.0f);
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
            StopTransitionCoroutine();

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
                currentBat.MorphToPlayingState();
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
        // Transition the current bat to look normal
        isMorphingBat = true;
        if (currentBat != normalBat)
        {
            currentBat.MorphToNormal();
            yield return new WaitForSeconds(1.0f);
        }

        // Swap to the new bat (which starts looking like a normal bat)
        SwitchToNextBat();

        // Morph to the playing state of the next bat
        if (currentBat != normalBat)
        {
            currentBat.MorphToPlayingState();
        }
        else
        {
            // make normal bat
            currentBat.MorphToNormal();
        }

        yield return null;

        isMorphingBat = false;
    }

    private void SwitchToNextBat()
    {
        HideAllBats();
        currentBatType = nextBatType;

        if (nextBatType == PlayerBatTypes.None)
        {
            // no bat
            return;
        }

        currentBat = GetBat((int) nextBatType);

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
        StopRunningCoroutine();
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
        allBats.TryGetValue(batType, out var bat);
        if (bat == null)
        {
            Debug.Log($"batType:{batType} missing");
        }

        return bat;
    }

    private void StopTransitionCoroutine()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
    }

    private void StopRunningCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}