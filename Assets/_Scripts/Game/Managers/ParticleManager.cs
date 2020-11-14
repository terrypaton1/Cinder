using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using System.Runtime.InteropServices;

public enum ParticleTypes {
	BrickExplosion,
	BallHitsBrick,
	BallHitsBat,
	NewBallOne,
	NewBallTwo,
	BallLost,
	PowerupLost,
	PowerupCollected,
	FallingPointsCollected,
	DestroyFallingItems,
	TNTExplosion,
	LaserHitsBrick,
	WanderingObstacleExplosion,
	WanderingObstacleSpawn,
	BossExplosion,
	FreezePlayer
}

public class ParticleManager : MonoBehaviour {
	/// <summary>
	/// The brick collision particle system.
	/// </summary>
	[SerializeField] ParticleSystem _brickCollisionParticleSystem;
	/// <summary>
	/// The brick explosion particle system.
	/// </summary>
	[SerializeField] ParticleSystem _brickExplosionParticleSystem;
	/// <summary>
	/// The bat collision particle system.
	/// </summary>
	[SerializeField] ParticleSystem _batCollisionParticleSystem;
	/// <summary>
	/// The new ball particle system.
	/// </summary>
	[SerializeField] ParticleSystem _newBallParticleSystem;
	/// <summary>
	/// The new ball two particle system.
	/// </summary>
	[SerializeField] ParticleSystem _newBallTwoParticleSystem;
	/// <summary>
	/// The ball lost particle system.
	/// </summary>
	[SerializeField] ParticleSystem ballLostParticleSystem;
	/// <summary>
	/// The powerup lost particles.
	/// </summary>
	[SerializeField] ParticleSystem PowerupLostParticles;
	/// <summary>
	/// The powerup collected particles.
	/// </summary>
	[SerializeField] ParticleSystem PowerupCollectedParticles;
	/// <summary>
	/// The falling points collected particles.
	/// </summary>
	[SerializeField] ParticleSystem FallingPointsCollectedParticles;
	/// <summary>
	/// The destroy falling items particles.
	/// </summary>
	[SerializeField] ParticleSystem DestroyFallingItemsParticles;
	/// <summary>
	/// The TNT explosion particles.
	/// </summary>
	[SerializeField] ParticleSystem TNTExplosionParticles;
	/// <summary>
	/// The laser bullet particles.
	/// </summary>
	[SerializeField] ParticleSystem LaserBulletParticles;
	/// <summary>
	/// The wandering obstacle explosion particles.
	/// </summary>
	[SerializeField] ParticleSystem WanderingObstacleExplosionParticles;
	/// <summary>
	/// The wandering obstacle spawn particles.
	/// </summary>
	[SerializeField] ParticleSystem WanderingObstacleSpawnParticles;
	/// <summary>
	/// The boss explosion particles.
	/// </summary>
	[SerializeField] ParticleSystem BossExplosionParticles;
	/// <summary>
	/// The freeze player particles.
	/// </summary>
	[SerializeField] ParticleSystem FreezePlayerParticles;



	/// <summary>
	/// Spawns the particle effect.
	/// </summary>
	/// <param name="effect">Effect.</param>
	/// <param name="position">Position.</param>
	void SpawnParticleEffect(ParticleTypes effect, Vector3 position) {
		position.z = -.2f;
		switch (effect) {

			case ParticleTypes.BallHitsBrick:

				_brickCollisionParticleSystem.transform.position = position;
				_brickCollisionParticleSystem.Emit(5);
				break;
			case ParticleTypes.BrickExplosion:
				_brickExplosionParticleSystem.transform.position = position;
				_brickExplosionParticleSystem.Emit(10);
				break;
			case ParticleTypes.BallHitsBat:

				_batCollisionParticleSystem.transform.position = position;
				_batCollisionParticleSystem.Emit(5);
				break;
			case ParticleTypes.NewBallOne:
				_newBallParticleSystem.transform.position = position;
				_newBallParticleSystem.Play();
				break;
			case ParticleTypes.NewBallTwo:
				_newBallTwoParticleSystem.transform.position = position;
				_newBallTwoParticleSystem.Play();
				break;
			case ParticleTypes.BallLost:
				ballLostParticleSystem.transform.position = position;
				ballLostParticleSystem.Emit(10);
				break;
			case ParticleTypes.PowerupLost:
				PowerupLostParticles.transform.position = position;
				PowerupLostParticles.Emit(10);
				break;
			case ParticleTypes.PowerupCollected:
				PowerupCollectedParticles.transform.position = position;
				PowerupCollectedParticles.Emit(10);
				break;
			case ParticleTypes.FallingPointsCollected:
				FallingPointsCollectedParticles.transform.position = position;
				FallingPointsCollectedParticles.Emit(5);
				break;
			case ParticleTypes.DestroyFallingItems:
				DestroyFallingItemsParticles.transform.position = position;
				DestroyFallingItemsParticles.Emit(5);
				break;
			case ParticleTypes.TNTExplosion:
				TNTExplosionParticles.transform.position = position;
				TNTExplosionParticles.Emit(40);
				break;
			case ParticleTypes.LaserHitsBrick:
				LaserBulletParticles.transform.position = position;
				LaserBulletParticles.Emit(5);
				break;
			case ParticleTypes.WanderingObstacleExplosion:
				WanderingObstacleExplosionParticles.transform.position = position;
				WanderingObstacleExplosionParticles.Emit(20);
				break;
			case ParticleTypes.WanderingObstacleSpawn:
				WanderingObstacleSpawnParticles.transform.position = position;
				WanderingObstacleSpawnParticles.Emit(10);
				break;
			case ParticleTypes.BossExplosion:
//				Debug.Log("BossExplosion");
				BossExplosionParticles.transform.position = position;
				BossExplosionParticles.Emit(50);
				break;
			case ParticleTypes.FreezePlayer:
//				Debug.Log("FreezePlayer");
				FreezePlayerParticles.transform.position = position;
				FreezePlayerParticles.Emit(1);
				break;
		}
	}

	void OnEnable() {
		Messenger<ParticleTypes,Vector3>.AddListener(GlobalEvents.SpawnParticleEffect, SpawnParticleEffect);
	}

	/// <summary>
	/// Remove listeners
	/// </summary>
	void OnDisable() {
		Messenger<ParticleTypes,Vector3>.RemoveListener(GlobalEvents.SpawnParticleEffect, SpawnParticleEffect);
	}

}
