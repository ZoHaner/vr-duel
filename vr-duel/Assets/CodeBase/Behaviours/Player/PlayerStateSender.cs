using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Utilities.Player;
using UnityEngine;

namespace CodeBase.Behaviours.Player
{
    public class PlayerStateSender : MonoBehaviour
    {
        public PlayerDeath PlayerDeath;
        
        public float StateFrequency = 0.1f;

        public Transform HeadTransform;
        public Transform LeftHandTransform;
        public Transform RightHandTransform;

        public Rigidbody HeadRigidbody;
        public Rigidbody LeftHandRigidbody;
        public Rigidbody RightHandRigidbody;
        
        private IInputEventService _eventService;
        private INetworkService _networkService;
        private float _stateSyncTimer;

        
        public void Construct(INetworkService networkService, IInputEventService eventService)
        {
            _eventService = eventService;
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
                        HeadTransform,
                        Vector3.zero,
                        LeftHandTransform,
                        Vector3.zero,
                        RightHandTransform
                    ));

                _stateSyncTimer = StateFrequency;
            }

            _stateSyncTimer -= Time.deltaTime;
        }

        private void SendInput()
        {
            if (_eventService.InputChanged)
            {
                _networkService.SendMatchState(OpCodes.Input, MatchDataJson.Input(_eventService.IsAttackButtonPressed()));
            }
        }
    }
}