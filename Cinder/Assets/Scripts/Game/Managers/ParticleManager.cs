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
        position.z = -0.2f;
        switch (effect)
        {
            case ParticleTypes.BallHitsBrick:
                PositionAndEmit(_brickCollisionParticleSystem, position, 5);
                break;
            case ParticleTypes.BrickExplosion:
                PositionAndEmit(_brickExplosionParticleSystem, position, 10);
                break;
            case ParticleTypes.BallHitsBat:

                PositionAndEmit(_batCollisionParticleSystem, position, 5);
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
                PositionAndEmit(ballLostParticleSystem, position, 10);
                break;
            case ParticleTypes.PowerUpLost:
                PositionAndEmit(PowerUpLostParticles, position, 10);
                break;
            case ParticleTypes.PowerUpCollected:
                PositionAndEmit(PowerUpCollectedParticles, position, 10);
                break;
            case ParticleTypes.FallingPointsCollected:
                PositionAndEmit(FallingPointsCollectedParticles, position, 5);
                break;
            case ParticleTypes.DestroyFallingItems:
                PositionAndEmit(DestroyFallingItemsParticles, position, 5);
                break;
            case ParticleTypes.TNTExplosion:
                PositionAndEmit(TNTExplosionParticles, position, 40);
                break;
            case ParticleTypes.LaserHitsBrick:
                PositionAndEmit(LaserBulletParticles, position, 5);
                break;
            case ParticleTypes.WanderingObstacleExplosion:
                PositionAndEmit(WanderingObstacleExplosionParticles, position, 20);
                break;
            case ParticleTypes.WanderingObstacleSpawn:
                PositionAndEmit(WanderingObstacleSpawnParticles, position, 10);
                break;
            case ParticleTypes.BossExplosion:
                PositionAndEmit(BossExplosionParticles, position, 50);
                break;
            case ParticleTypes.FreezePlayer:
                PositionAndEmit(FreezePlayerParticles, position, 1);
                break;
        }
    }

    private static void PositionAndEmit(ParticleSystem particles, Vector3 position, int emitCount)
    {
        particles.transform.position = position;
        particles.Emit(emitCount);
    }
}