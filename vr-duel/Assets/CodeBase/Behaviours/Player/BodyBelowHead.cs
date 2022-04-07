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
            var newBodyPosition = _head.position;
            newBodyPosition.y -= _distance;
            _body.position = newBodyPosition;
        }
    }
}