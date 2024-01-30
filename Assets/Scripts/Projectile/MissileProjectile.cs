using GombleTask.Extensions;
using UnityEngine;

namespace GombleTask
{
    public class MissileProjectile : Projectile
    {
        public Transform Target;
        private float _acceleration;

        private const float _LERP_SPEED = 0.1f;
        private const float _LERP_MAX_SPEED = 0.1f;
        
        protected override void UpdateLaunchedBullet()
        {
            Accelerate();
        }

        protected override void UpdateAbortedBullet()
        {
            Deaccelerate();
        }

        protected override void UpdateBullet()
        {
            TargetPosition = Target.position;
            
            if (CurrentSpeed > 0)
            {
                RotateToTarget();
            }
            
            FlyForward();
        }

        void RotateToTarget()
        {
            transform.RotateToLerp(Target.position, _LERP_SPEED, _LERP_MAX_SPEED);
        }

        void Accelerate()
        {
            CurrentSpeed += _acceleration;

            if (CurrentSpeed > MaxSpeed)
            {
                CurrentSpeed = MaxSpeed;
            }
        }

        void Deaccelerate()
        {
            CurrentSpeed -= _acceleration;

            if (CurrentSpeed < 0)
            {
                CurrentSpeed = 0;
            }
        }
    }
}