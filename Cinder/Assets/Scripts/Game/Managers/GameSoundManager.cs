using UnityEngine;
using Random = UnityEngine.Random;

public class GameSoundManager : MonoBehaviour
{
    [SerializeField]
    protected AudioClip[] ballHitsBrickSounds;

    [SerializeField]
    protected AudioClip[] ballHitsWall;

    [SerializeField]
    protected AudioClip[] ballHitsBat;

    [SerializeField]
    protected AudioClip[] brickDestroyed;

    [SerializeField]
    protected AudioClip levelComplete;

    [SerializeField]
    protected AudioClip ballLost;

    [SerializeField]
    protected AudioClip multiball;

    [SerializeField]
    protected AudioClip extraLife;

    [SerializeField]
    protected AudioClip multiHitMadness;

    [SerializeField]
    protected AudioClip multiHitAwesome;

    [SerializeField]
    protected AudioClip multiHit;

    [SerializeField]
    protected AudioClip multiHitExcellent;

    [SerializeField]
    protected AudioClip multiHitBrilliant;

    [SerializeField]
    protected AudioClip multiHitInsane;

    [SerializeField]
    protected AudioClip multiHitWild;

    [SerializeField]
    protected AudioClip multiHitUnbelievable;

    [SerializeField]
    protected AudioClip roundBumper;

    [SerializeField]
    protected AudioClip[] pointsCollected;

    [SerializeField]
    protected AudioClip powerupShield;

    [SerializeField]
    protected AudioClip powerupLaser;

    [SerializeField]
    protected AudioClip powerupCrazyBall;

    [SerializeField]
    protected AudioClip powerupFireball;

    [SerializeField]
    protected AudioClip powerupShrinkBat;

    [SerializeField]
    protected AudioClip powerupDoubleBat;

    [SerializeField]
    protected AudioClip powerupLongBat;

    [SerializeField]
    protected AudioClip powerupFreeze;

    [SerializeField]
    protected AudioClip[] tntBricks;

    [SerializeField]
    protected AudioClip[] unbreakableBrick;

    [SerializeField]
    protected AudioClip lifeLost;

    [SerializeField]
    protected AudioClip laserBulletHitsBrickCollision;

    [SerializeField]
    protected AudioClip[] laserBulletFiring;

    [SerializeField]
    protected AudioClip laserBulletHitsWall;

    [SerializeField]
    protected AudioClip ballHitsShield;

    [SerializeField]
    protected AudioClip wanderingObstacleDestroyed;

    [SerializeField]
    protected AudioClip wanderingObstacleHit;

    [SerializeField]
    protected AudioClip allBONUSLettersCollected;

    [SerializeField]
    protected AudioClip bonusLetterCollected;

    [SerializeField]
    protected AudioSource _audioSource1;

    [SerializeField]
    protected AudioSource _audioSource2Balls;

    [SerializeField]
    protected AudioSource _audioSource2Bricks;

    private int randomSound;
    private float randomPitch;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    protected void OnEnable()
    {
        CoreConnector.SoundManager = this;
    }

    public void PlaySound(SoundList soundID)
    {
        if (CoreConnector.GameManager.gameVariables.SFXEnabled == 0)
        {
//			Debug.Log("SFX disabled");
            return;
        }

//todo change this to a dictionary look up for the sounds
         randomPitch = Random.Range(lowPitchRange, highPitchRange);
        switch (soundID)
        {
            case SoundList.ballHitsBrick:
                randomSound = Random.Range(0, ballHitsBrickSounds.Length);
                _audioSource2Balls.pitch = randomPitch;
                _audioSource2Balls.PlayOneShot(ballHitsBrickSounds[randomSound]);
                break;
            case SoundList.ballHitsWall:
                randomSound = Random.Range(0, ballHitsWall.Length);
                _audioSource2Balls.pitch = randomPitch;
                _audioSource2Balls.PlayOneShot(ballHitsWall[randomSound]);
                break;
            case SoundList.ballHitsBat:
                randomSound = Random.Range(0, ballHitsBat.Length);
                _audioSource2Balls.pitch = randomPitch;
                _audioSource2Balls.PlayOneShot(ballHitsBat[randomSound]);
                break;
            case SoundList.brickDestroyed:
                randomSound = Random.Range(0, brickDestroyed.Length);
                _audioSource2Bricks.pitch = randomPitch;
                _audioSource2Bricks.PlayOneShot(brickDestroyed[randomSound]);
                break;
            case SoundList.levelComplete:
                _audioSource1.PlayOneShot(levelComplete);
                break;
            case SoundList.ballLost:
                _audioSource1.PlayOneShot(ballLost);
                break;
            case SoundList.multiBall:
                _audioSource1.PlayOneShot(multiball);
                break;
            case SoundList.ExtraLife:
                _audioSource1.PlayOneShot(extraLife);
                break;
            case SoundList.PointsCollected:
                randomSound = Random.Range(0, pointsCollected.Length);
                _audioSource2Balls.pitch = randomPitch;
                _audioSource2Balls.PlayOneShot(pointsCollected[randomSound]);
                break;
            case SoundList.PowerupShield:
                _audioSource1.PlayOneShot(powerupShield);
                break;
            case SoundList.PowerupLaser:
                _audioSource1.PlayOneShot(powerupLaser);
                break;
            case SoundList.PowerupCrazyBall:
                _audioSource1.PlayOneShot(powerupCrazyBall);
                break;
            case SoundList.PowerupFireball:
                _audioSource1.PlayOneShot(powerupFireball);
                break;
            case SoundList.PowerupSmallBat:
                _audioSource1.PlayOneShot(powerupShrinkBat);
                break;
            case SoundList.PowerupSplitBat:
                _audioSource1.PlayOneShot(powerupDoubleBat);
                break;
            case SoundList.PowerUpWideBat:
                _audioSource1.PlayOneShot(powerupLongBat);
                break;
            case SoundList.PowerupFreeze:
                _audioSource1.PlayOneShot(powerupFreeze);
                break;

            case SoundList.LifeLost:
                _audioSource1.PlayOneShot(lifeLost);
                break;
            case SoundList.TNTBrick:
                randomSound = Random.Range(0, tntBricks.Length);
                _audioSource2Balls.pitch = randomPitch;
                _audioSource2Balls.PlayOneShot(tntBricks[randomSound]);
                break;
            case SoundList.MultiHit:
                _audioSource1.PlayOneShot(multiHit);
                break;
            case SoundList.MultiHitAwesome:
                _audioSource1.PlayOneShot(multiHitAwesome);
                break;
            case SoundList.MultiHitMadness:
                _audioSource1.PlayOneShot(multiHitMadness);
                break;
            case SoundList.MultiHitExcellent:
                _audioSource1.PlayOneShot(multiHitExcellent);
                break;
            case SoundList.MultiHitBrilliant:
                _audioSource1.PlayOneShot(multiHitBrilliant);
                break;
            case SoundList.MultiHitInsane:
                _audioSource1.PlayOneShot(multiHitInsane);
                break;
            case SoundList.MultiHitWild:
                _audioSource1.PlayOneShot(multiHitWild);
                break;
            case SoundList.MultiHitUnbelievable:
                _audioSource1.PlayOneShot(multiHitUnbelievable);
                break;
            case SoundList.RounderBumper:
//				Debug.Log("add round bumper sound");
                _audioSource1.pitch = randomPitch;
                _audioSource1.PlayOneShot(roundBumper);
                break;
            case SoundList.LaserBulletHitsBrick:
//				Debug.Log("laser collision sound");
                _audioSource1.pitch = randomPitch;
                _audioSource1.PlayOneShot(laserBulletHitsBrickCollision);
                break;
            case SoundList.LaserBulletFiring:
//				Debug.Log("laser firing sound");
                randomSound = Random.Range(0, laserBulletFiring.Length);
                _audioSource1.pitch = randomPitch;
                _audioSource1.PlayOneShot(laserBulletFiring[randomSound]);
                break;
            case SoundList.LaserBulletHitsWall:
//				Debug.Log("laser hitting wall sound");
                _audioSource1.pitch = randomPitch;
                _audioSource1.PlayOneShot(laserBulletHitsWall);
                break;
            case SoundList.BallHitsShield:
//				Debug.Log("laser hitting wall sound");
                _audioSource1.pitch = randomPitch;
                _audioSource1.PlayOneShot(ballHitsShield);
                break;
            case SoundList.WanderingObstacleDestroyed:
//				Debug.Log("laser hitting wall sound");
                _audioSource1.pitch = randomPitch;
                _audioSource1.PlayOneShot(wanderingObstacleDestroyed);
                break;
            case SoundList.WanderingObstacleHit:
//				Debug.Log("laser hitting wall sound");
                _audioSource1.pitch = randomPitch;
                _audioSource1.PlayOneShot(wanderingObstacleHit);
                break;

            case SoundList.BONUSLetterCollected:
//				Debug.Log("laser hitting wall sound");
                _audioSource1.pitch = randomPitch;
                _audioSource1.PlayOneShot(bonusLetterCollected);
                break;
            case SoundList.AllBONUSLettersCollected:
//				Debug.Log("laser hitting wall sound");
                _audioSource1.pitch = randomPitch;
                _audioSource1.PlayOneShot(allBONUSLettersCollected);
                break;
        }
    }
}