using UnityEngine;

namespace CodeBase.Services.Input.Standalone
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController _controller;

        public float Speed = 12f;

        private void Update()
        {
            float x = UnityEngine.Input.GetAxis("Horizontal");
            float z = UnityEngine.Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            _controller.Move(move * Speed * Time.deltaTime);
        }
    }
}