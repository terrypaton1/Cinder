using UnityEngine;

public class LaserBat : PlayersBatBase
{
    private float firingFrequency = 1.0f;
    private const string Intro = "LaserBatIntro";
    private const string ToNormal = "LaserBatToNormal";
    private const string PlayerLosesLifeAnimation = "LaserBatPlayerLosesLife";
    private float timer;
    private bool shootingBullet;

    readonly Vector2 FiringVelocity = new Vector2(0, .1f);

    public override void MorphToPlayState()
    {
        MorphToPlayingAnimation.Play(Intro);
    }

    public override void MorphToNormal()
    {
//		Debug.Log("transition to normal bat");
        MorphToPlayingAnimation.Play(ToNormal);
    }

    public override void PlayerLosesLife()
    {
        MorphToPlayingAnimation.Play(PlayerLosesLifeAnimation);
    }

    public override void EnableBat()
    {
        base.EnableBat();
        shootingBullet = true;
        firingFrequency = GameVariables.laserBatFiringFrequency;
    }

    public override void DisableBat()
    {
        base.DisableBat();
        shootingBullet = false;
    }

    private void FireBullet()
    {
        PlaySound(SoundList.LaserBulletFiring);
        CoreConnector.GameManager.powerUpManager.FireLaser(transform.position, FiringVelocity);
    }

    protected override void UpdateLoop()
    {
        if (!shootingBullet)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer < firingFrequency)
        {
            return;
        }

        timer -= firingFrequency;
        FireBullet();
    }

    public override void LevelComplete()
    {
        shootingBullet = false;
    }
}