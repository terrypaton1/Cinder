using UnityEngine;

public class BallEffectsManager : MonoBehaviour
{
    [SerializeField]
    protected ParticleSystem flameBallParticles;

    [SerializeField]
    protected ParticleSystem crazyBallParticles;

    [SerializeField]
    protected ParticleSystem ballTrailParticles;

    [SerializeField]
    protected TrailRenderer trailRenderer;

    public void ActivateFlameBall()
    {
        flameBallParticles.Play();
    }

    public void DisableFlameBall()
    {
        flameBallParticles.Stop();
    }

    public void DisableBallTrail()
    {
        trailRenderer.enabled = false;
        ballTrailParticles.Stop();
    }

    public void EnableBallTrail()
    {
        trailRenderer.enabled = true;
        ballTrailParticles.Play();
    }

    public void DisableEffects()
    {
        DisableBallTrail();
    }

    public void ActivateCrazyBall()
    {
        crazyBallParticles.Play();
    }

    public void DisableCrazyBall()
    {
        crazyBallParticles.Stop();
    }


    public void SetTrailEmittingState(bool state)
    {
        trailRenderer.emitting = state;
    }
}