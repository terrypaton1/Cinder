using UnityEngine;
using System.Collections;

/// <summary>
/// The Sound FX list.
/// </summary>
public enum SoundList {
	ballHitsBrick,
	ballHitsWall,
	ballHitsBat,
	brickDestroyed,
	levelComplete,
	ballLost,
	multiball,
	PointsCollected,
	LifeLost,

	ExtraLife,
	PowerupShield,
	PowerupLaser,
	PowerupCrazyBall,
	PowerupFireball,
	PowerupSmallBat,
	PowerupSplitBat,
	PowerupWideBat,
	PowerupFreeze,
	TNTBrick,
	UnbreakableBrick,

	MultiHitAwesome,
	MultiHitMadness,
	MultiHitExcellent,
	MultiHitBrilliant,
	MultiHitInsane,
	MultiHitWild,
	MultiHitUnbelievable,
	MultiHit,

	RounderBumper,
	LaserBulletHitsBrick,
	LaserBulletFiring,
	LaserBulletHitsWall,
	BallHitsShield,
	WanderingObstacleDestroyed,
	WanderingObstacleHit,
	AllBONUSLettersCollected,
	BONUSLetterCollected

}

public class GameSoundManager : MonoBehaviour {

	[SerializeField]
	AudioClip[] ballHitsBrickSounds;
	[SerializeField]
	AudioClip[] ballHitsWall;
	[SerializeField]
	AudioClip[] ballHitsBat;
	[SerializeField]
	AudioClip[] brickDestroyed;
	[SerializeField]
	AudioClip levelComplete;
	[SerializeField]
	AudioClip ballLost;
	[SerializeField]
	AudioClip multiball;
	[SerializeField]
	AudioClip extraLife;

	[SerializeField]
	AudioClip multiHitMadness;
	[SerializeField]
	AudioClip multiHitAwesome;
	[SerializeField]
	AudioClip multiHit;
	[SerializeField]
	AudioClip multiHitExcellent;
	[SerializeField]
	AudioClip multiHitBrilliant;
	[SerializeField]
	AudioClip multiHitInsane;
	[SerializeField]
	AudioClip multiHitWild;
	[SerializeField]
	AudioClip multiHitUnbelievable;

	[SerializeField]
	AudioClip roundBumper;

	[SerializeField]
	AudioClip[] pointsCollected;
	[SerializeField]
	AudioClip powerupShield;

	[SerializeField]
	AudioClip powerupLaser;
	[SerializeField]
	AudioClip powerupCrazyBall;
	[SerializeField]
	AudioClip powerupFireball;
	[SerializeField]
	AudioClip powerupShrinkBat;
	[SerializeField]
	AudioClip powerupDoubleBat;
	[SerializeField]
	AudioClip powerupLongBat;
	[SerializeField]
	AudioClip powerupFreeze;

	[SerializeField]
	AudioClip[] tntBricks;
	[SerializeField]
	AudioClip[]	unbreakableBrick;
	[SerializeField]
	AudioClip lifeLost;

	[SerializeField]
	AudioClip laserBulletHitsBrickCollision;

	[SerializeField]
	AudioClip[] laserBulletFiring;

	[SerializeField]
	AudioClip laserBulletHitsWall;

	[SerializeField]
	AudioClip ballHitsShield;
	[SerializeField]
	AudioClip wanderingObstacleDestroyed;

	[SerializeField]
	AudioClip wanderingObstacleHit;
	[SerializeField]
	AudioClip allBONUSLettersCollected;

	[SerializeField]
	AudioClip bonusLetterCollected;

	
	[SerializeField]
	AudioSource _audioSource1;
	
	[SerializeField]
	AudioSource _audioSource2Balls;
	
	[SerializeField]
	AudioSource _audioSource2Bricks;


	
	int randomSound;
	
	float randomPitch;
	
	public float lowPitchRange = .95f;
	
	public float highPitchRange = 1.05f;

	void OnEnable() {
		Messenger<SoundList>.AddListener(GlobalEvents.PlaySoundFX, PlaySound);
	}

	void OnDisable() {
		Messenger<SoundList>.RemoveListener(GlobalEvents.PlaySoundFX, PlaySound);
	}
	
	public void PlaySound(SoundList soundID) {
		if (GameVariables.instance.SFXEnabled == 0) {
//			Debug.Log("SFX disabled");
			return;
		}
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);
		switch (soundID) {
			case SoundList.ballHitsBrick:
				randomSound = Random.Range(0, ballHitsBrickSounds.Length);
				_audioSource2Balls.pitch = randomPitch;            
				_audioSource2Balls.PlayOneShot(ballHitsBrickSounds [randomSound]);
				break;
			case SoundList.ballHitsWall:
				randomSound = Random.Range(0, ballHitsWall.Length);
				_audioSource2Balls.pitch = randomPitch;
				_audioSource2Balls.PlayOneShot(ballHitsWall [randomSound]);
				break;
			case SoundList.ballHitsBat:
				randomSound = Random.Range(0, ballHitsBat.Length);
				_audioSource2Balls.pitch = randomPitch;
				_audioSource2Balls.PlayOneShot(ballHitsBat [randomSound]);
				break;
			case SoundList.brickDestroyed:
				randomSound = Random.Range(0, brickDestroyed.Length);
				_audioSource2Bricks.pitch = randomPitch;
				_audioSource2Bricks.PlayOneShot(brickDestroyed [randomSound]);
				break;
			case SoundList.levelComplete:
				_audioSource1.PlayOneShot(levelComplete);
				break;
			case SoundList.ballLost:
				_audioSource1.PlayOneShot(ballLost);
				break;
			case SoundList.multiball:
				_audioSource1.PlayOneShot(multiball);
				break;
			case SoundList.ExtraLife:
				_audioSource1.PlayOneShot(extraLife);
				break;
			case SoundList.PointsCollected:
				randomSound = Random.Range(0, pointsCollected.Length);
				_audioSource2Balls.pitch = randomPitch;            
				_audioSource2Balls.PlayOneShot(pointsCollected [randomSound]);
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
			case SoundList.PowerupWideBat:
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
				_audioSource2Balls.PlayOneShot(tntBricks [randomSound]);
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
				_audioSource1.PlayOneShot(laserBulletFiring [randomSound]);
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