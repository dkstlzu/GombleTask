using System;
using System.Collections;
using Fusion;
using GombleTask.Utility;
using Michsky.MUIP;
using TMPro;
using UnityEngine;

namespace GombleTask.UIs
{
    public class WaitForEnemyUI : MonoBehaviour, IPlayerLeft
    {
        public TextMeshProUGUI TextUI;
        public UIManagerProgressBarLoop Spinner;

        public string ConnectingToServer;
        public string WaitingEnemyText;
        public string YouLostConnectionText;
        public string EnemyLostConnectionText;
        public string SpectatingText;
        public string WinText;
        public string LoseText;
        public string GameResultToSpectatorText;

        private GameManager _gameManager;
        
        private void Start()
        {
            TextUI.text = ConnectingToServer;
        }

        private void OnDestroy()
        {
            _gameManager.OnEnemyConnected -= OnEnemyConnected;
            _gameManager.OnPlayerLeft -= PlayerLeft;
            _gameManager.OnSpectatorJoin -= Spectate;
            _gameManager.OnPlayerWin -= PlayerWin;
        }

        public void OnConnectedToServer()
        {
            _gameManager = NetworkRunner.GetRunnerForGameObject(gameObject).GetComponent<GameManager>();
            
            _gameManager.OnEnemyConnected += OnEnemyConnected;
            _gameManager.OnPlayerLeft += PlayerLeft;
            _gameManager.OnSpectatorJoin += Spectate;
            _gameManager.OnPlayerWin += PlayerWin;
            
            TextUI.text = WaitingEnemyText;
        }

        void OnEnemyConnected()
        {
            Destroy(Spinner.gameObject);
            StartCoroutine(CountDown());
        }

        IEnumerator CountDown()
        {
            TextUI.text = "3";
            yield return new WaitForSeconds(1);
            TextUI.text = "2";
            yield return new WaitForSeconds(1);
            TextUI.text = "1";
            yield return new WaitForSeconds(1);
            TextUI.enabled = false;
        }
        
        public void PlayerLeft(PlayerRef player)
        {
            if (player == _gameManager.Runner.LocalPlayer)
            {
                TextUI.enabled = true;
                TextUI.text = YouLostConnectionText;
            } else if (player == _gameManager.EnemyPlayer)
            {
                TextUI.text = EnemyLostConnectionText;
                TextUI.enabled = true;
            }
        }
        
        private void Spectate()
        {
            TextUI.text = SpectatingText;
        }

        public void PlayerWin(PlayerRef player)
        {
            TextUI.enabled = true;
            if (player == _gameManager.Runner.LocalPlayer)
            {
                TextUI.text = WinText;
            } else if (player == _gameManager.EnemyPlayer)
            {
                TextUI.text = LoseText;
            }
            else
            {
                TextUI.text = $"{player.PlayerId} {GameResultToSpectatorText}";
            }
        }
    }
}