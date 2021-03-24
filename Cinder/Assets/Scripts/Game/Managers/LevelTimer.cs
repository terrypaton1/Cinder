using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField]
    protected FallingPowerup fallingPowerUp;

    [SerializeField]
    public PowerupType[] randomPowerupChoices;

    private const string repeatingFunctionName = "IncrementTime";

    private const float repeatingTimeStep = 1.0f;
    private float timePassed;
    private float timeBeforeFirstPowerUpDrops;
    private bool powerUpDropStarted;
    private float timeBetweenPowerUpsTimer;
    private float timeBetweenPowerUps;

    protected void OnEnable()
    {
        timeBeforeFirstPowerUpDrops = 1.5f * 60;
        timeBetweenPowerUps = 20;
        fallingPowerUp.Setup(PowerupType.MultiBall);
        fallingPowerUp.Disable();
    }

    public void StartTimer()
    {
        StopTimer();
        InvokeRepeating(repeatingFunctionName, repeatingTimeStep, repeatingTimeStep);
    }

    public void IncrementTime()
    {
        if (!CoreConnector.GameManager.IsGamePlaying())
        {
            return;
        }

        if (!powerUpDropStarted)
        {
            timePassed += repeatingTimeStep;
            if (timePassed > timeBeforeFirstPowerUpDrops)
            {
                powerUpDropStarted = true;
                DropPowerUp();
            }
        }

        if (!powerUpDropStarted)
        {
            return;
        }

        timeBetweenPowerUpsTimer += repeatingTimeStep;
        if (!(timeBetweenPowerUpsTimer > timeBetweenPowerUps))
        {
            return;
        }

        timeBetweenPowerUpsTimer = 0;
        DropPowerUp();
    }

    public void ResetTimer()
    {
        fallingPowerUp.Disable();
        powerUpDropStarted = false;
        timePassed = 0;
        timeBetweenPowerUpsTimer = 0;
        StartTimer();
    }

    public void StopTimer()
    {
        CancelInvoke(repeatingFunctionName);
    }

    private void DropPowerUp()
    {
        var randomPowerUpNum = Random.Range(0, randomPowerupChoices.Length);
        var randomTypeOfPowerUp = randomPowerupChoices[randomPowerUpNum];
        fallingPowerUp.Setup(randomTypeOfPowerUp);

        var xRange = Random.Range(-1.3f, 1.3f);
        var upStartPosition = new Vector3(xRange, 8.0f, 0.0f);
        var powerUpStartPosition = upStartPosition;
        fallingPowerUp.StartFalling(powerUpStartPosition);
        
        CoreConnector.GameManager.particleManager.SpawnParticleEffect(
            ParticleTypes.LaserHitsBrick,
            powerUpStartPosition);
    }
}