using UnityEngine;
using UnityEngine.InputSystem;

namespace GombleTask
{
    public class CrossHair : MonoBehaviour
    {
        private Camera _cam;
        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            var mouseWorldPosition = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);
        }
    }
}