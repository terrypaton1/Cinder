using UnityEngine;

public class LaserBat : PlayersBatBase
{
    private float firingFrequency = 1.0f;
    private float timer;
    private bool shootingBullet;

    private readonly Vector2 firingVelocity = new Vector2(0, 0.1f);

    public override void EnableBat()
    {
        base.EnableBat();
        shootingBullet = true;
        firingFrequency = GameVariables.LaserBatFiringFrequency;
    }

    public override void DisableBat()
    {
        base.DisableBat();
        shootingBullet = false;
    }

    private void FireBullet()
    {
        PlaySound(SoundList.LaserBulletFiring);
        CoreConnector.GameManager.powerUpManager.FireLaser(transform.position, firingVelocity);
    }

    protected override void UpdateLoop()
    {
        // todo move this to the LaserBatPowerUp class
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