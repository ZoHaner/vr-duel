using UnityEngine;

namespace CodeBase.Services.Input.Standalone
{
    public class VRPlayerMovement : PlayerMovement
    {
        [SerializeField] private Transform _playerCamera;

        protected override void Move()
        {
            var direction = CalculateMovingVector();
            var rotatedDirection = CalculateRotatedVector(direction);
            MovePlayer(rotatedDirection);
        }

        private Vector3 CalculateRotatedVector(Vector3 direction)
        {
            var angle = Vector3.SignedAngle(transform.forward, _playerCamera.forward, Vector3.up);
            return Quaternion.Euler(0, angle,0) * direction;
        }
    }
}