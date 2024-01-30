namespace GombleTask
{
    public class ConstantSpeedProjectile : Projectile
    {
        protected override void UpdateLaunchedBullet()
        {
            CurrentSpeed = MaxSpeed;
        }

        protected override void UpdateAbortedBullet()
        {
            CurrentSpeed = 0;
        }

        protected override void UpdateBullet()
        {
            FlyForward();
        }
    }
}