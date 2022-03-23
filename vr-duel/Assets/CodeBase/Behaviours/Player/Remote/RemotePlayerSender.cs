using CodeBase.Entities;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Behaviours.Player.Remote
{
    public class RemotePlayerSender : MonoBehaviour
    {
        public PlayerDeath PlayerDeath;
        private IRoundService _roundService;
        private RemotePlayer _remotePlayer;

        public void Construct(IRoundService roundService, RemotePlayer remotePlayer)
        {
            _remotePlayer = remotePlayer;
            _roundService = roundService;
        }

        private void Awake() => 
            SubscribeEvents();

        private void OnDestroy() => 
            Cleanup();

        private void SubscribeEvents() => 
            PlayerDeath.PlayerDead += SendPlayerDeadState;

        private void Cleanup() => 
            PlayerDeath.PlayerDead -= SendPlayerDeadState;

        private void SendPlayerDeadState() => 
            _roundService.PlayerDeath?.Invoke(_remotePlayer.Presence);
    }
}