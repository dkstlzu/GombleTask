using System;
using Fusion;
using GombleTask.Utility;
using UnityEngine;

namespace GombleTask
{
    public class ProjectileDisplay : MonoBehaviour
    {
        public Projectile TargetProjectile;
        public bool FlipXY;
        public bool HasStateAuthority;
        
        public SpriteRenderer ProjectileSpriteRenderer;
        
        // public Sprite DefaultProjectileSprite;
        // public Sprite RocketProjectileSprite;
        // public Sprite MissileProjectilSprite;

        private GameManager _gameManager;
        
        public void Init(Projectile projectile)
        {
            TargetProjectile = projectile;
            _gameManager = NetworkRunner.GetRunnerForGameObject(gameObject).GetComponent<GameManager>();
            _gameManager.OnFixedUpdateNetwork += FixedUpdateNetwork;
        }

        public void FixedUpdateNetwork()
        {
            if (!TargetProjectile)
            {
                _gameManager.OnFixedUpdateNetwork -= FixedUpdateNetwork;
                Destroy(gameObject);
                return;
            }
            
            if (FlipXY)
            {
                ProjectileSpriteRenderer.flipY = true;
                ProjectileSpriteRenderer.flipX = true;
                transform.localPosition = -TargetProjectile.transform.position;
            }
            else
            {
                ProjectileSpriteRenderer.flipY = false;
                ProjectileSpriteRenderer.flipX = false;
                transform.localPosition = TargetProjectile.transform.position;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Printer.Print($"TriggerEnter {other.gameObject}");
            if (other.TryGetComponent(out CharacterDisplay display))
            {
                if (!HasStateAuthority)
                {
                    display.TargetCharacter.HitRpc();
                }
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}