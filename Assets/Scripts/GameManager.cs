using System;
using System.Collections;
using System.Linq;
using System.Threading;
using Fusion;
using GombleTask.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GombleTask
{
    public enum ShipType
    {
        Classic,
        Blue,
        Green,
        Purple,
    }
    
    public class GameManager : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        public event Action OnEnemyConnected;
        public event Action OnBattleStart;
        public event Action OnBattleEnd;
        public event Action<PlayerRef> OnPlayerLeft;
        public event Action OnSpectatorJoin;
        public event Action<PlayerRef> OnPlayerWin;
        
        public GameObject NetworkAirshipPrefab;
        public GameObject DisplayAirshipPrefab;

        public GameObject CrossHairPrefab;

        private CancellationTokenSource _checkConnectionSource;

        public Transform MyShipSpawnPoint;
        public Transform EnemyShipSpawnPoint;
        private bool _bSelfConnected = false;
        private bool _bEnemyConnected = false;

        public PlayerRef EnemyPlayer;

        public event Action OnFixedUpdateNetwork;
        public override void FixedUpdateNetwork()
        {
            OnFixedUpdateNetwork?.Invoke();
        }

        IEnumerator ReadyForBattle()
        {
            yield return new WaitForSeconds(3f);

            SpawnMyShip();
            
            yield return new WaitForSeconds(1f);
            
            SpawnEnemyShip();
            
            yield return new WaitForSeconds(1f);

            SpawnCrosshair();
            Printer.Print("Battle Start");
            OnBattleStart?.Invoke();
        }

        void SpawnMyShip()
        {
            var display = SpawnShip(MyShipSpawnPoint.position, Quaternion.identity);
            display.transform.SetParent(MyShipSpawnPoint);
            display.Init(Runner.GetPlayerObject(Runner.LocalPlayer).GetComponent<Character>());
        }

        void SpawnEnemyShip()
        {
            var display = SpawnShip(EnemyShipSpawnPoint.position, Quaternion.identity);
            display.transform.SetParent(EnemyShipSpawnPoint);

            var players = Runner.ActivePlayers;
            display.Init(Runner.GetPlayerObject(EnemyPlayer).GetComponent<Character>());
            display.FlipXY = true;
        }

        CharacterDisplay SpawnShip(Vector3 position, Quaternion rotaion)
        {
            var display = Instantiate(DisplayAirshipPrefab, position, rotaion).GetComponent<CharacterDisplay>();
            
            return display;
        }

        void SpawnCrosshair()
        {
            Instantiate(CrossHairPrefab, Vector3.zero, Quaternion.identity);
        }

        public void PlayerJoined(PlayerRef player)
        {
            if (player == Runner.LocalPlayer)
            {
                if (Runner.ActivePlayers.Count() >= 3)
                {
                    OnSpectatorJoin?.Invoke();
                    return;
                }
                
                Printer.Print($"LocalPlayer {player.PlayerId} joined");

                Debug.Assert(!_bSelfConnected, "자기 자신이 두번 연결됐습니다");
                
                _bSelfConnected = true;
                var spawned = Runner.Spawn(NetworkAirshipPrefab);
                spawned.gameObject.name = "MyNetworkShip";
                
                spawned.AssignInputAuthority(player);
                
                Runner.SetPlayerObject(player, spawned);
            }
            else
            {
                Printer.Print($"Player {player.PlayerId} joined");

                if (Runner.ActivePlayers.Count() >= 3)
                {
                    return;
                }
                
                _bEnemyConnected = true;
                EnemyPlayer = player;
            }
            
            CheckForBattleStart();
        }

        private void CheckForBattleStart()
        {
            Printer.Print("Check for battle ready");
            if (_bSelfConnected && _bEnemyConnected)
            {
                Printer.Print("Ready for Battle!");
                OnEnemyConnected?.Invoke();
                StartCoroutine(ReadyForBattle());
            }
        }

        public void PlayerLeft(PlayerRef player)
        {
            Printer.Print($"Player {player.PlayerId} left");

            OnPlayerLeft?.Invoke(player);

            if (player == Runner.LocalPlayer || player == EnemyPlayer)
            {
                OnBattleEnd?.Invoke();
                Invoke("BackToMenuScene", 3f);
            }
        }

        void BackToMenuScene()
        {
            Runner.Shutdown(true);
            SceneManager.LoadScene(0);
        }

        public void PlayerWin(PlayerRef player)
        {
            Printer.Print($"{player.PlayerId} Win Game");
            Invoke("BackToMenuScene", 3f);
            OnPlayerWin?.Invoke(player);   
            OnBattleEnd?.Invoke();
        }
    }
}