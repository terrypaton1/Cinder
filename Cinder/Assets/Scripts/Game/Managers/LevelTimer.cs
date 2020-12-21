using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField]
    protected FallingPowerup fallingPowerUp;

    [SerializeField]
    public PowerupType[] randomPowerupChoices;

    private string repeatingFunctionName = "IncrementTime";

    private float repeatingTimeStep = 1f;
    private float timePassed;
    private float timeBeforeFirstPowerupDrops;
    private bool powerupDropStarted;
    private float timeBetweenPowerupsTimer;
    private float timeBetweenPowerups;

    protected void OnEnable()
    {
        timeBeforeFirstPowerupDrops = 1.5f * 60;
        timeBetweenPowerups = 20;
        fallingPowerUp.Setup(PowerupType.MultiBall);
        fallingPowerUp.Disable();
    }

    public void StartTimer()
    {
//		Debug.Log("start timer");
        StopTimer();
        InvokeRepeating(repeatingFunctionName, repeatingTimeStep, repeatingTimeStep);
    }

    public void IncrementTime()
    {
        // check how much time has passed;
        // check on the game state
        if (!CoreConnector.GameManager.IsGamePlaying())
        {
            return;
        }

        if (!powerupDropStarted)
        {
            timePassed += repeatingTimeStep;
//			Debug.Log("timePassed:" + timePassed);
            if (timePassed > timeBeforeFirstPowerupDrops)
            {
                powerupDropStarted = true;
//				Debug.Log("drop first powerup");
                DropPowerup();
            }
        }

        if (powerupDropStarted)
        {
            timeBetweenPowerupsTimer += repeatingTimeStep;
//			Debug.Log("timeBetweenPowerupsTimer:" + timeBetweenPowerupsTimer);
            if (timeBetweenPowerupsTimer > timeBetweenPowerups)
            {
                timeBetweenPowerupsTimer = 0;
                DropPowerup();
            }
        }
    }

    public void ResetTimer()
    {
        fallingPowerUp.Disable();
        powerupDropStarted = false;
        timePassed = 0;
        timeBetweenPowerupsTimer = 0;
        StartTimer();
    }

    public void StopTimer()
    {
//		Debug.Log("StopRepeating");
        CancelInvoke(repeatingFunctionName);
    }

    public void DropPowerup()
    {
        int randomPowerUpNum = Random.Range(0, randomPowerupChoices.Length);
        PowerupType randomTypeOfPowerUp = randomPowerupChoices[randomPowerUpNum];
        fallingPowerUp.Setup(randomTypeOfPowerUp);

        Vector3 powerupStartPosition = new Vector3(Random.Range(-1.3f, 1.3f), 8, 0);
        fallingPowerUp.StartFalling(powerupStartPosition);
        CoreConnector.GameManager.particleManager.SpawnParticleEffect(ParticleTypes.LaserHitsBrick,
            powerupStartPosition);
    }
}