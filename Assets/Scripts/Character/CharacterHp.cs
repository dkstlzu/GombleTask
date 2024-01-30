using System;
using UnityEngine;

namespace GombleTask
{
    /// <summary>
    /// 다양한 단위의 Hp를 구현하기 위한 클래스
    /// </summary>
    [Serializable]
    public class CharacterHp
    {
        public float Min;
        public float Max;

        private float _value;
        public float Value 
        {
            get
            {
                float actualValue = Math.Clamp(_value - Min, 0, range);

                return actualValue / range;
            }
            private set => _value = value;
        }
        public bool IsFull => Value >= Max;
        public bool IsLost => Value <= Min;

        private float range => Max - Min;

        public CharacterHp(float min, float max, float initial)
        {
            Min = min;
            Max = max;
            Value = initial;
        }
        
        /// <summary>
        /// 0~1 사이의 값을 받아 실제값으로 변경하여 할당합니다.
        /// </summary>
        /// <param name="value">0~1사이의 값</param>
        public void SetHp(float value)
        {
            Debug.Assert(isValidHp(value), $"해당 CharacterHp에는 {Min}~{Max} 사이의 값이 유효합니다.");

            Value = Min + range * value;
        }

        public void SetFull()
        {
            Value = Max;
        }

        public void SetLost()
        {
            Value = Min;
        }

        private bool isValidHp(float value)
        {
            return value >= Min && value <= Max;
        }
    }
}