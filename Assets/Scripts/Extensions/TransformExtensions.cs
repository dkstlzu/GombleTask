using UnityEngine;

namespace GombleTask.Extensions
{
    public static class TransformExtensions
    {
        public static float RotateEpsilon = 0.1f;

        public static void RotateToLerp(this Transform transform, Vector3 point, float lerpValue, float maxSpeed)
        {
            var eulerAngles = transform.eulerAngles;
            
            float currentEulerAngle = eulerAngles.z;
            Vector2 posDiff = point - transform.position;
            float targetEulerAngle = Mathf.Atan2(posDiff.y, posDiff.x) * Mathf.Rad2Deg;
            
            if (Mathf.Abs(targetEulerAngle - currentEulerAngle) >= RotateEpsilon)
            {
                float angleDiff = MathUtility.AngleDiff(currentEulerAngle, targetEulerAngle);
                float lerpedDiff = angleDiff * lerpValue;
                float maxDiff = maxSpeed / Time.deltaTime;

                float absLerpedDiff = Mathf.Abs(lerpedDiff);
                
                int sign = angleDiff > 0 ? 1 : -1;

                float clampedDiff = Mathf.Min(absLerpedDiff, maxDiff);
                
                float nextZRot = currentEulerAngle + clampedDiff * sign;
                
                transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, nextZRot);
            }
        }

        /// <summary>
        /// transform.up이 앞이라는 전제하에 사용됩니다.
        /// </summary>
        public static void MoveForward(this Transform transform, float speed)
        {
            Vector2 forwardPoint = transform.position + transform.up * (speed * Time.deltaTime);
            transform.MoveTo(forwardPoint);
        }
        
        /// <summary>
        /// transform.up이 앞이라는 전제하에 사용됩니다.
        /// </summary>
        public static void MoveBackward(this Transform transform, float speed)
        {
            Vector2 forwardPoint = transform.position + transform.up * -(speed * Time.deltaTime);
            transform.MoveTo(forwardPoint);
        }
        
        /// <summary>
        /// transform.up이 앞이라는 전제하에 사용됩니다.
        /// </summary>
        public static void MoveRight(this Transform transform, float speed)
        {
            Vector2 forwardPoint = transform.position + transform.right * (speed * Time.deltaTime);
            transform.MoveTo(forwardPoint);
        }
        
        /// <summary>
        /// transform.up이 앞이라는 전제하에 사용됩니다.
        /// </summary>
        public static void MoveLeft(this Transform transform, float speed)
        {
            Vector2 forwardPoint = transform.position + transform.right * -(speed * Time.deltaTime);
            transform.MoveTo(forwardPoint);
        }

        public static void MoveTo(this Transform transform, Vector2 point)
        {
            transform.position = point;
        }
    }
}