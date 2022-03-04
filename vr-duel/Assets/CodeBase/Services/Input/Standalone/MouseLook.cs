using UnityEngine;

namespace CodeBase.Services.Input.Standalone
{
    public class MouseLook : MonoBehaviour
    {
        public float MouseSensitivity = 100f;
        
        [SerializeField] private Transform Camera;
        [SerializeField] private Transform PlayerBody;

        private float _xRotation = 0f;

        // public MouseLook(Transform camera, Transform playerBody)
        // {
        //     Camera = camera;
        //     PlayerBody = playerBody;
        //     
        //     Initialize();
        // }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Update()
        {
            float mouseX = UnityEngine.Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
            float mouseY = UnityEngine.Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            Camera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            PlayerBody.Rotate(Vector3.up * mouseX);
        }
    }
}