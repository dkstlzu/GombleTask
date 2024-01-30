using System;
using UnityEngine;

namespace GombleTask
{
    public class RocketProjectile : Projectile
    {
        public float Acceleration;
        public float Range;

        private void Awake()
        {
            TargetPosition = transform.position + transform.right * Range;
        }

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
            FlyForward();
        }
        
        void Accelerate()
        {
            CurrentSpeed += Acceleration;

            if (CurrentSpeed > MaxSpeed)
            {
                CurrentSpeed = MaxSpeed;
            }
        }

        void Deaccelerate()
        {
            CurrentSpeed -= Acceleration;

            if (CurrentSpeed < 0)
            {
                CurrentSpeed = 0;
            }
        }
    }
}