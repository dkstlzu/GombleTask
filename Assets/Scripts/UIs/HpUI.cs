using System;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

namespace GombleTask.UIs
{
    public class HpUI : MonoBehaviour
    {
        public Image MyHp1;
        public Image MyHp2;
        public Image MyHp3;
        
        public Image EnemyHp1;
        public Image EnemyHp2;
        public Image EnemyHp3;

        private int _myHp = 3;
        private int _enemyHp = 3;

        private GameManager _gameManager;
        
        public void OnConnectedToServer()
        {
            _gameManager = NetworkRunner.GetRunnerForGameObject(gameObject).GetComponent<GameManager>();

            _gameManager.OnBattleStart += () =>
            {
                _gameManager.Runner.GetPlayerObject(_gameManager.Runner.LocalPlayer).GetComponent<Character>().OnHpChanged += MyHpLost;
                _gameManager.Runner.GetPlayerObject(_gameManager.EnemyPlayer).GetComponent<Character>().OnHpChanged += EnemyHpLost;
            };
        }

        public void MyHpLost(int hp)
        {
            _myHp = hp;
            
            if (_myHp == 2)
            {
                MyHp3.enabled = false;
            } else if (_myHp == 1)
            {
                MyHp2.enabled = false;
            } else if (_myHp == 0)
            {
                MyHp1.enabled = false;
            }
        }

        public void EnemyHpLost(int hp)
        {
            _enemyHp = hp;
            
            if (_enemyHp == 2)
            {
                EnemyHp3.enabled = false;
            } else if (_enemyHp == 1)
            {
                EnemyHp2.enabled = false;
            } else if (_enemyHp == 0)
            {
                EnemyHp1.enabled = false;
            }
        }
    }
}