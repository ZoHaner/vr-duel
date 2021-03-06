using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.StaticData;
using CodeBase.Utilities.Network;
using UnityEngine;

namespace CodeBase.Behaviours.Player
{
    public class PlayerStateSender : MonoBehaviour
    {
        [SerializeField] private float StateFrequency = 0.1f;

        [SerializeField] private Transform _headTransform;
        [SerializeField] private Transform _leftHandTransform;
        [SerializeField] private Transform _rightHandTransform;

        private IInputService _service;
        private INetworkService _networkService;
        private float _stateSyncTimer;
        private bool _lastAttackData;


        public void Construct(INetworkService networkService, IInputService service)
        {
            _service = service;
            _networkService = networkService;
        }

        private void LateUpdate()
        {
            SendPositionAndVelocity();
            SendInput();
        }

        private void SendPositionAndVelocity()
        {
            if (_stateSyncTimer <= 0)
            {
                _networkService.SendMatchState(
                    OpCodes.VelocityAndPosition,
                    MatchDataJson.BodyVelocityAndPosition(
                        headVelocity: Vector3.zero,
                        _headTransform,
                        Vector3.zero,
                        _leftHandTransform,
                        Vector3.zero,
                        _rightHandTransform
                    ));

                _stateSyncTimer = StateFrequency;
            }

            _stateSyncTimer -= Time.deltaTime;
        }

        private void SendInput()
        {
            if (_service.IsAttackButtonPressed() != _lastAttackData)
            {
                _networkService.SendMatchState(OpCodes.Input, MatchDataJson.Input(_service.IsAttackButtonPressed()));
            }
            
            _lastAttackData = _service.IsAttackButtonPressed();
        }
    }
}