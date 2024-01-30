using System;
using Fusion;
using GombleTask.Extensions;
using UnityEngine;

namespace GombleTask
{
    public abstract class Projectile : NetworkBehaviour
    {
        public GameObject DisplayPrefab;
        
        public Vector2 TargetPosition;
        public Vector2 HeadingDirection;
        public float MaxSpeed;
        public float CurrentSpeed;
        
        public bool IsFlying;

        public override void Spawned()
        {
            var myShipPoint = GameObject.FindWithTag("MyShipPoint").transform;
            var enemyShipPoint = GameObject.FindWithTag("EnemyShipPoint").transform;
            
            if (HasStateAuthority)
            {
                var display = Instantiate(DisplayPrefab, myShipPoint.position + transform.position, Quaternion.identity, myShipPoint).GetComponent<ProjectileDisplay>();
                display.Init(this);
                display.HasStateAuthority = true;
            }
            else
            {
                var display = Instantiate(DisplayPrefab, enemyShipPoint.position - transform.position, Quaternion.identity, enemyShipPoint).GetComponent<ProjectileDisplay>();
                display.Init(this);
                display.HasStateAuthority = false;
                display.FlipXY = true;
            }
        }
        
        public override void FixedUpdateNetwork()
        {
            if (Runner.IsFirstTick)
            if (IsFlying)
            {
                UpdateLaunchedBullet();
            }
            else
            {
                UpdateAbortedBullet();                
            }

            UpdateBullet();

            if (transform.position.x < -20 || transform.position.x > 20 || transform.position.y > 30)
            {
                Runner.Despawn(Object);
            }
        }
        
        protected abstract void UpdateLaunchedBullet();
        protected abstract void UpdateAbortedBullet();
        protected abstract void UpdateBullet();
        
        public void FlyForward()
        {
            transform.MoveForward(CurrentSpeed);
        }
        
        public void Launch()
        {
            IsFlying = true;
        }

        public void Abort()
        {
            IsFlying = false;
        }
    }
}