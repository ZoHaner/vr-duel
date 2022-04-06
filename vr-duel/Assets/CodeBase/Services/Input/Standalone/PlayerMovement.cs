using UnityEngine;

namespace CodeBase.Services.Input.Standalone
{
    public abstract class PlayerMovement : MonoBehaviour
    {
        [SerializeField] protected float Speed = 12f;

        private IInputService _inputService;

        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Update() => Move();
        
        protected abstract void Move();
        
        protected void MovePlayer(Vector3 vector)
        {
            transform.position += vector * Speed * Time.deltaTime;
        }

        protected Vector3 CalculateMovingVector()
        {
            float x = _inputService.GetMoveAxis().x;
            float z = _inputService.GetMoveAxis().y;
            Vector3 move = transform.right * x + transform.forward * z;
            return move;
        }
    }
}