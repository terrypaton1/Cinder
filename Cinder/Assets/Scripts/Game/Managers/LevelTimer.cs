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

        if (!powerUpDropStarted)
        {
            timePassed += repeatingTimeStep;
//			Debug.Log("timePassed:" + timePassed);
            if (timePassed > timeBeforeFirstPowerUpDrops)
            {
                powerUpDropStarted = true;
//				Debug.Log("drop first powerup");
                DropPowerUp();
            }
        }

        if (!powerUpDropStarted)
        {
            return;
        }

        timeBetweenPowerUpsTimer += repeatingTimeStep;
        if (timeBetweenPowerUpsTimer > timeBetweenPowerUps)
        {
            timeBetweenPowerUpsTimer = 0;
            DropPowerUp();
        }
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

        var powerUpStartPosition = new Vector3(Random.Range(-1.3f, 1.3f), 8, 0);
        fallingPowerUp.StartFalling(powerUpStartPosition);
        CoreConnector.GameManager.particleManager.SpawnParticleEffect(
            ParticleTypes.LaserHitsBrick,
            powerUpStartPosition);
    }
}