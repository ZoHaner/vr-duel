using System.Collections.Generic;
using CodeBase.Behaviours.Guns;
using CodeBase.Infrastructure.Utilities;
using CodeBase.Player;
using CodeBase.Player.Remote;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.Network;
using Nakama;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class NetworkPlayerFactory : INetworkPlayerFactory
    {
        public int PlayersCount => _players.Count;
        public string LocalUserSessionId { get; set; }

        private readonly INetworkService _networkService;
        private readonly InitialPointHolder _pointHolder;
        private readonly IInputEventService _inputEventService;

        Dictionary<string, GameObject> _players = new Dictionary<string, GameObject>();

        // private IMatchmakerUser _localUser;

        public NetworkPlayerFactory(INetworkService networkService, IInputEventService inputEventService)
        {
            _networkService = networkService;
            _inputEventService = inputEventService;
            _pointHolder = new InitialPointHolder();
        }

        public void SpawnPlayers(IEnumerable<IUserPresence> users, IRoundService roundService)
        {
            foreach (var user in users)
            {
                SpawnPlayer(user, roundService);
            }
        }

        private void SpawnPlayer(IUserPresence user, IRoundService roundService)
        {
            Debug.LogError("Spawn Player");
            var isLocal = user.SessionId == LocalUserSessionId;
            Debug.LogError($"Is local : {isLocal} ; user.SessionId =  {user.SessionId} ; localUser.SessionId {LocalUserSessionId}");
            var playerPrefabPath = isLocal ? AssetsPath.LocalPlayer : AssetsPath.NetworkPlayer;

            var initialPoint = _pointHolder.GetInitialPoint();
            var player = ResourcesUtilities.Instantiate(playerPrefabPath, initialPoint);
            player.transform.LookAt(Vector3.zero);

            if (isLocal)
            {
                player.GetComponent<PlayerStateSender>().Construct(_networkService, _inputEventService);
                CreateLocalPlayerGun(player);
            }
            else
            {
                GunShooting gunMono = CreateGun(player);

                var networkData = new RemotePlayerNetworkData(user);
                player.GetComponent<RemotePlayerSync>().Construct(networkData, gunMono);
                player.GetComponent<RemotePlayerSender>().Construct(roundService, networkData);
            }

            _players.Add(user.SessionId, player);
        }

        private void CreateLocalPlayerGun(GameObject player)
        {
            GunShooting gunMono = CreateGun(player);
            gunMono.Construct(_inputEventService);
            gunMono.SubscribeEvents();
        }

        private GunShooting CreateGun(GameObject player)
        {
            var gunPivot = player.GetComponentInChildren<GunPivot>().transform;
            var gunObject = ResourcesUtilities.Instantiate(AssetsPath.Revolver, gunPivot);
            var gunMono = gunObject.GetComponent<GunShooting>();
            return gunMono;
        }

        public void RemovePlayers(IEnumerable<IUserPresence> userPresences)
        {
            foreach (var presence in userPresences)
            {
                if (_players.ContainsKey(presence.SessionId))
                {
                    RemovePlayer(presence.SessionId);
                }
            }
        }

        public void RemovePlayer(string sessionId)
        {
            Debug.LogError("Remove player");
            _players[sessionId].SetActive(false);
            _players.Remove(sessionId);
        }

        public void UpdatePlayersState(IMatchState state)
        {
            // Debug.LogError("UpdatePlayersState");

            string userSessionId = state.UserPresence.SessionId;

            if (!_players.ContainsKey(userSessionId))
            {
                // Debug.LogError("State contains unknown SessionId!");
                return;
            }

            if (IsRemotePlayer(userSessionId))
            {
                _players[userSessionId].GetComponent<RemotePlayerSync>().UpdateState(state);
            }
        }

        public void RemoveAllPlayers()
        {
            foreach (var player in _players)
            {
                Object.Destroy(player.Value);
            }
            
            _players.Clear();
        }


        private bool IsRemotePlayer(string sessionId)
        {
            return sessionId != LocalUserSessionId;
        }
    }
}