using System;
using System.Collections.Generic;
using Fusion;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GombleTask.UIs
{
    public class MenuUI : MonoBehaviour
    {
        public static ShipType MyChosenShipType;
        
        public Image ShipImage;

        public List<Sprite> ShipSpriteList;

        private ShipType _chosenShipType;
        private int _shipTypeNumber;
        
        public string GameSceneName = "Ingame";
        public Slider SoundSlider;
        
        private const string _VOLUME_PLAYER_PREFS_KEY = "Volume";
        private const string _CHOSEN_SHIP_PLAYER_PREFS_KEY = "ChosenShip";

        private bool _restrictDoubleClick;
        
        private void Awake()
        {
            var savedVolume = PlayerPrefs.GetFloat(_VOLUME_PLAYER_PREFS_KEY, 0.5f);
            SoundSlider.value = savedVolume;
            SoundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);

            _chosenShipType = (ShipType)PlayerPrefs.GetInt(_CHOSEN_SHIP_PLAYER_PREFS_KEY, 0);
            ShipImage.sprite = ShipSpriteList[(int)_chosenShipType];
            _shipTypeNumber = ShipSpriteList.Count;
        }

        private void OnDestroy()
        {
            PlayerPrefs.SetFloat(_VOLUME_PLAYER_PREFS_KEY, SoundSlider.value);
            PlayerPrefs.SetInt(_CHOSEN_SHIP_PLAYER_PREFS_KEY, (int)_chosenShipType);
        }

        private void Update()
        {
            InputSystem.Update();
        }

        public void OnStartButtonClicked()
        {
            if (_restrictDoubleClick)
            {
                return;
            }

            MyChosenShipType = _chosenShipType;
            _restrictDoubleClick = true;
            SceneManager.LoadScene(GameSceneName);
        }

        void OnSoundSliderValueChanged(float value)
        {
            AudioListener.volume = value;
        }

        public void OnLeftButtonClicked()
        {
            int shipType = (int)_chosenShipType;
            shipType--;
            if (shipType < 0)
            {
                shipType += _shipTypeNumber;
            }

            _chosenShipType = (ShipType)shipType;
            ShipImage.sprite = ShipSpriteList[shipType];
        }

        public void OnRightButtonClicked()
        {
            int shipType = (int)_chosenShipType;
            shipType++;
            if (shipType >= _shipTypeNumber)
            {
                shipType = 0;
            }

            _chosenShipType = (ShipType)shipType;
            ShipImage.sprite = ShipSpriteList[shipType];
        }

        public void OnExitButtonClicked()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}