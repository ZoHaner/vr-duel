using UnityEngine;

namespace CodeBase.Behaviours.Player
{
    public class BodyBelowHead : MonoBehaviour
    {
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _body;
        [SerializeField] private float _distance;

        private void Update()
        {
            ApplyBodyPosition();
            ApplyBodyRotation();
        }

        private void ApplyBodyPosition()
        {
            var newBodyPosition = _head.position;
            newBodyPosition.y -= _distance;
            _body.position = newBodyPosition;
        }

        private void ApplyBodyRotation()
        {
            var newBodyRotation = _head.eulerAngles;
            newBodyRotation.x = 0;
            newBodyRotation.z = 0;
            _body.rotation = Quaternion.Euler(newBodyRotation);
        }
    }
}