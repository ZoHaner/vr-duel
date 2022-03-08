using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Player.Remote
{
    public class RemotePlayerSender : MonoBehaviour
    {
        public PlayerDeath PlayerDeath;
        private IRoundService _roundService;
        private RemotePlayerNetworkData _remotePlayerNetworkData;

        public void Construct(IRoundService roundService, RemotePlayerNetworkData remotePlayerNetworkData)
        {
            _remotePlayerNetworkData = remotePlayerNetworkData;
            _roundService = roundService;
        }

        private void Awake()
        {
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        public void SubscribeEvents()
        {
            PlayerDeath.PlayerDead += SendPlayerDeadState;
        }

        public void Cleanup()
        {
            PlayerDeath.PlayerDead -= SendPlayerDeadState;
        }

        private void SendPlayerDeadState()
        {
            _roundService.PlayerDeath?.Invoke(_remotePlayerNetworkData.User);
        }
    }
}