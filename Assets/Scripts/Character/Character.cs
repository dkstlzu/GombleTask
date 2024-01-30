using System;
using System.Collections.Generic;
using Fusion;
using GombleTask.Utility;
using UnityEngine;

namespace GombleTask
{
    public class Character : NetworkBehaviour
    {
        public GameObject DisplayPrefab;
        
        private CharacterController _controller;
        public CharacterController Controller => _controller;

        public CharacterController.Properties ControllerProperties;

        public event Action<ShipType> OnShipTypeChanged;
        [Networked, OnChangedRender(nameof(SetType))]
        [field: SerializeField]
        public ShipType ShipType { get; set; }

        public event Action<int> OnHpChanged;
        public int MaxHp = 3;
        [Networked, OnChangedRender(nameof(HpChanged))]
        [field: SerializeField]
        public int CurrentHp { get; set; }

        public override void Spawned()
        {
            _controller = new CharacterController(this, ControllerProperties);
            CurrentHp = MaxHp;
        }

        void SetType()
        {
            OnShipTypeChanged?.Invoke(ShipType); 
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.StateAuthority)]
        public void HitRpc()
        {
            CurrentHp--;

        }

        public void HpChanged()
        {
            if (CurrentHp <= 0)
            {
                GameManager gameManager = Runner.GetComponent<GameManager>();

                if (HasStateAuthority)
                {
                    gameManager.PlayerWin(gameManager.EnemyPlayer);
                }
                else
                {
                    gameManager.PlayerWin(Runner.LocalPlayer);
                }
            }
            
            OnHpChanged?.Invoke(CurrentHp);
        }
    }
}