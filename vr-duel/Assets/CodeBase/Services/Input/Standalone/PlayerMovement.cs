using UnityEngine;

namespace CodeBase.Services.Input.Standalone
{
    public class PlayerMovement : MonoBehaviour
    {
        public float Speed = 12f;
        private IInputService _inputService;

        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Update()
        {
            float x = _inputService.GetMoveAxis().x;
            float z = _inputService.GetMoveAxis().y;

            Vector3 move = transform.right * x + transform.forward * z;
            transform.position += move * Speed * Time.deltaTime;
        }
    }
}