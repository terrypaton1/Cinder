using UnityEngine;

public enum ParticleTypes
{
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

public class ParticleManager : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _brickCollisionParticleSystem;

    [SerializeField]
    ParticleSystem _brickExplosionParticleSystem;

    [SerializeField]
    ParticleSystem _batCollisionParticleSystem;

    [SerializeField]
    ParticleSystem _newBallParticleSystem;

    [SerializeField]
    ParticleSystem _newBallTwoParticleSystem;

    [SerializeField]
    ParticleSystem ballLostParticleSystem;

    [SerializeField]
    ParticleSystem PowerupLostParticles;

    [SerializeField]
    ParticleSystem PowerupCollectedParticles;

    [SerializeField]
    ParticleSystem FallingPointsCollectedParticles;

    [SerializeField]
    ParticleSystem DestroyFallingItemsParticles;

    [SerializeField]
    ParticleSystem TNTExplosionParticles;

    [SerializeField]
    ParticleSystem LaserBulletParticles;

    [SerializeField]
    ParticleSystem WanderingObstacleExplosionParticles;

    [SerializeField]
    ParticleSystem WanderingObstacleSpawnParticles;

    [SerializeField]
    ParticleSystem BossExplosionParticles;

    [SerializeField]
    ParticleSystem FreezePlayerParticles;

    public void SpawnParticleEffect(ParticleTypes effect, Vector3 position)
    {
        position.z = -.2f;
        switch (effect)
        {
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
}