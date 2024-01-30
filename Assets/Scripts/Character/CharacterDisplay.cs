using Fusion;
using UnityEngine;

namespace GombleTask
{
    public class CharacterDisplay : MonoBehaviour
    {
        public Character TargetCharacter;
        public bool FlipXY;

        public SpriteRenderer ShipSpriteRenderer;
        
        public Sprite ClassicShipSprite;
        public Sprite BlueShipSprite;
        public Sprite GreenShipSprite;
        public Sprite PurpleShipSprite;

        private GameManager _gameManager;
        
        public void Init(Character character)
        {
            TargetCharacter = character;
            character.OnShipTypeChanged += SetType;
            _gameManager = NetworkRunner.GetRunnerForGameObject(gameObject).GetComponent<GameManager>();
            _gameManager.OnFixedUpdateNetwork += FixedUpdateNetwork;
        }

        public void FixedUpdateNetwork()
        {
            if (!TargetCharacter)
            {
                _gameManager.OnFixedUpdateNetwork -= FixedUpdateNetwork;
                return;
            }
            
            if (FlipXY)
            {
                ShipSpriteRenderer.flipY = true;
                var targetPos = TargetCharacter.transform.position;
                transform.localPosition = new Vector3(-targetPos.x, targetPos.y);
            }
            else
            {
                ShipSpriteRenderer.flipY = false;
                transform.localPosition = TargetCharacter.transform.position;
            }
        }

        public void SetType(ShipType type)
        {
            switch (type)
            {
                case ShipType.Classic:
                    ShipSpriteRenderer.sprite = ClassicShipSprite;
                    break;
                case ShipType.Blue:
                    ShipSpriteRenderer.sprite = BlueShipSprite;
                    break;
                case ShipType.Green:
                    ShipSpriteRenderer.sprite = GreenShipSprite;
                    break;
                case ShipType.Purple:
                    ShipSpriteRenderer.sprite = PurpleShipSprite;
                    break;
                default:
                    Debug.Assert(false, $"알수없는 Shiptype입니다. Classic으로 대체합니다.");
                    break;
            }
        }
    }
}