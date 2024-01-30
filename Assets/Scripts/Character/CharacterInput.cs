using System;
using Fusion;
using UnityEngine.InputSystem;
using Context = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace GombleTask
{
    public class CharacterInput : NetworkBehaviour
    {
        public Character Character;

        [Networked] private TickTimer _fireTimer { get; set; }
        
        private GombleTaskInputAsset _inputAsset;

        private InputAction _moveAction;
        private InputAction _fireAction;

        private GameManager _gameManager;
        
        void Awake()
        {
            if (!Character)
            {
                Character = GetComponent<Character>();
            }

            _gameManager = NetworkRunner.GetRunnerForGameObject(gameObject).GetComponent<GameManager>();
            _gameManager.OnBattleStart += OnEnable;
            _gameManager.OnBattleEnd += OnDisable;
            _gameManager.OnPlayerWin += (p) => OnDisable();
            
            _inputAsset = new GombleTaskInputAsset();

            _moveAction = _inputAsset.Ingame.Move;
            _fireAction = _inputAsset.Ingame.Fire;
        }

        private void Start()
        {
            _inputAsset.Disable();
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority)
            {
                return;
            }
            
            if (!HasInputAuthority)
            {
                return;
            }
            
            InputSystem.Update();
            
            if (_moveAction.IsPressed())
            {
                float moveDirection = _moveAction.ReadValue<float>();
                
                if (moveDirection > 0)
                {
                    Character.Controller.MoveRight();
                }
                else if (moveDirection < 0)
                {
                    Character.Controller.MoveLeft();
                }
            }

            if (_fireAction.WasPerformedThisFrame())
            {
                if (_fireTimer.ExpiredOrNotRunning(Runner))
                {
                    Character.Controller.Fire();
                    _fireTimer = TickTimer.CreateFromSeconds(Runner, Character.Controller.FireCoolTime);
                }
            }
        }

        public void OnEnable()
        {
            _inputAsset.Enable();
        }

        public void OnDisable()
        {
            _inputAsset.Disable();
        }
    }
}