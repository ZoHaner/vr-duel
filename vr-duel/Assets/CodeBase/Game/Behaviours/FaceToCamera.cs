using UnityEngine;

namespace CodeBase.Behaviours
{
    public class FaceToCamera : MonoBehaviour
    {
        public bool BlockYPosition = true;

        public float MovingSpeed = 2f;
        public float DistanceToCamera = 5f;

        public float MovingTriggerAngle = 20;
        public float MovingTriggerDistance = 1f;

        public float StopMovingTriggerDistance = 0.05f;

        private float SqrStopMovingTriggerDistance => Mathf.Pow(StopMovingTriggerDistance, 2);
        private float SqrMovingTriggerDistance => Mathf.Pow(MovingTriggerDistance, 2);

        private Vector3 _lastStableForward = Vector3.up;
        private Vector3 _lastStablePosition = Vector3.zero;

        private Vector3 _targetPosition;
        private bool _shouldMove;

        private Transform _targetObject
        {
            get
            {
                var camera = Camera.main;
                return camera != null ? camera.transform : null;
            }
        }

        public void Update()
        {
            if (_targetObject == null)
                return;

            if (!_shouldMove)
                _shouldMove = CheckCameraOutOfRange() || CheckChangedDistance();

            if (_shouldMove)
            {
                MoveToTarget();

                if (ObjectCloseToTargetPoint())
                    StopMoving();
            }
        }

        private bool CheckCameraOutOfRange()
        {
            return Vector3.Angle(_lastStableForward, _targetObject.forward) > MovingTriggerAngle;
        }

        private bool CheckChangedDistance()
        {
            return Vector3.SqrMagnitude(_lastStablePosition - _targetObject.position) > SqrMovingTriggerDistance;
        }

        private void MoveToTarget()
        {
            CalculateForward();
            CalculatePosition();
        }

        private void CalculateForward()
        {
            transform.LookAt(_targetObject);
            transform.forward = -transform.forward;
        }

        private void CalculatePosition()
        {
            _targetPosition = _targetObject.position + _targetObject.forward.normalized * DistanceToCamera;
            var smoothed = Vector3.Slerp(transform.position, _targetPosition, Time.deltaTime * MovingSpeed);

            if (BlockYPosition)
            {
                _targetPosition.y = _targetObject.position.y;
                smoothed.y = _targetObject.position.y;
            }

            transform.position = smoothed;
        }

        private bool ObjectCloseToTargetPoint()
        {
            return Vector3.SqrMagnitude(transform.position - _targetPosition) < SqrStopMovingTriggerDistance;
        }

        private void StopMoving()
        {
            _shouldMove = false;
            _lastStableForward = _targetObject.forward;
            _lastStablePosition = _targetObject.position;
        }
    }
}