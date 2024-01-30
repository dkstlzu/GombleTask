using System;
using System.Collections.Generic;
using Fusion;
using GombleTask.Extensions;
using GombleTask.Utility;
using UnityEngine;

namespace GombleTask
{
    public class CharacterController
    {
        [Serializable]
        public class Properties
        {
            public GameObject ProjectilePrefab;
            public float FireCoolTime;
            public float MoveSpeed;
            public float ProjectileSpeed;
            public Transform ProjectileSpawnPosition;
        }
        
        public Character Character;
        private Properties _properties;
        public float FireCoolTime => _properties.FireCoolTime;
        
        private GameObject gameObject;
        private Transform transform;

        private List<Projectile> _projectileControllerList = new List<Projectile>();
        
        private GameManager _gameManager;
        
        public CharacterController(Character character, Properties properties)
        {
            Character = character;
            _properties = properties;

            gameObject = Character.gameObject;
            transform = Character.transform;
            _gameManager = NetworkRunner.GetRunnerForGameObject(gameObject).GetComponent<GameManager>();
        }

        public void MoveRight()
        {
            transform.MoveRight(_properties.MoveSpeed);
            if (transform.position.x > 7)
            {
                transform.position = new Vector3(7, 0, 0);
            }
        }

        public void MoveLeft()
        {
            transform.MoveLeft(_properties.MoveSpeed);
            if (transform.position.x < -7)
            {
                transform.position = new Vector3(-7, 0, 0);
            }
        }

        public void Rotate()
        {
            
        }

        public void Fire()
        {
            var spawned = _gameManager.Runner.Spawn(_properties.ProjectilePrefab, _properties.ProjectileSpawnPosition.position, transform.rotation);
            // spawned.RequestStateAuthority();
            var projectile = spawned.gameObject.GetComponent<Projectile>();
            projectile.MaxSpeed = _properties.ProjectileSpeed;
            projectile.Launch();
        }
    }
}