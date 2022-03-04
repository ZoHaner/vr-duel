using CodeBase.Services.Network;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerStateSender : MonoBehaviour
    {
        public float StateFrequency = 0.1f;

        private INetworkService _networkService;
        private float _stateSyncTimer;

        public Transform HeadTransform;
        public Transform LeftHandTransform;
        public Transform RightHandTransform;

        public Rigidbody HeadRigidbody;
        public Rigidbody LeftHandRigidbody;
        public Rigidbody RightHandRigidbody;

        public void Construct(INetworkService networkService)
        {
            _networkService = networkService;
        }

        private void LateUpdate()
        {
            if (_stateSyncTimer <= 0)
            {
                _networkService.SendMatchState(
                    OpCodes.VelocityAndPosition,
                    MatchDataJson.BodyVelocityAndPosition(
                        Vector3.zero, 
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
    }
}