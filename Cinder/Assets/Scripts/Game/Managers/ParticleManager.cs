using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField]
    protected ParticleSystem _brickCollisionParticleSystem;

    [SerializeField]
    protected ParticleSystem _brickExplosionParticleSystem;

    [SerializeField]
    protected ParticleSystem _batCollisionParticleSystem;

    [SerializeField]
    protected ParticleSystem _newBallParticleSystem;

    [SerializeField]
    protected ParticleSystem _newBallTwoParticleSystem;

    [SerializeField]
    protected ParticleSystem ballLostParticleSystem;

    [SerializeField]
    protected ParticleSystem PowerUpLostParticles;

    [SerializeField]
    protected ParticleSystem PowerUpCollectedParticles;

    [SerializeField]
    protected ParticleSystem FallingPointsCollectedParticles;

    [SerializeField]
    protected ParticleSystem DestroyFallingItemsParticles;

    [SerializeField]
    protected ParticleSystem TNTExplosionParticles;

    [SerializeField]
    protected ParticleSystem LaserBulletParticles;

    [SerializeField]
    protected ParticleSystem WanderingObstacleExplosionParticles;

    [SerializeField]
    protected ParticleSystem WanderingObstacleSpawnParticles;

    [SerializeField]
    protected ParticleSystem BossExplosionParticles;

    [SerializeField]
    protected ParticleSystem FreezePlayerParticles;

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
            case ParticleTypes.PowerUpLost:
                PowerUpLostParticles.transform.position = position;
                PowerUpLostParticles.Emit(10);
                break;
            case ParticleTypes.PowerUpCollected:
                PowerUpCollectedParticles.transform.position = position;
                PowerUpCollectedParticles.Emit(10);
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
            default:
                // empty.
                break;
        }
    }
}