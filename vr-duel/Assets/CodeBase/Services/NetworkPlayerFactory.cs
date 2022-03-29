using System.Collections.Generic;
using CodeBase.Behaviours.Gun;
using CodeBase.Behaviours.Player;
using CodeBase.Behaviours.Player.Remote;
using CodeBase.Entities;
using CodeBase.Services.Input;
using CodeBase.Services.Input.Standalone;
using CodeBase.StaticData;
using CodeBase.Utilities;
using CodeBase.Utilities.Spawn;
using Nakama;
using UnityEngine;

namespace CodeBase.Services
{
    public class NetworkPlayerFactory : INetworkPlayerFactory
    {
        public int PlayersCount => _players.Count;
        
        public string LocalUserId { get; set; }

        private readonly INetworkService _networkService;
        private readonly IInputService _inputService;
        private InitialPointHolder _pointHolder;

        Dictionary<string, GameObject> _players = new Dictionary<string, GameObject>();
        
        private readonly Vector3 _gunPivotOffset = new Vector3(0.4f, 0.7f, 0f);

        public NetworkPlayerFactory(INetworkService networkService, IInputService inputService)
        {
            _networkService = networkService;
            _inputService = inputService;
        }

        public void Initialize()
        {
            _pointHolder = new InitialPointHolder();
        }

        public void CleanUp()
        {
            _pointHolder.CleanPoints();
            RemoveAllPlayers();
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
            var isLocal = user.UserId == LocalUserId;
            Debug.LogError($"Is local : {isLocal} ; user.UserId =  {user.UserId} ; localUser.UserId {LocalUserId}");
            var playerPrefabPath = isLocal ? AssetsPath.LocalNetworkPlayerStandalone : AssetsPath.RemoteNetworkPlayerStandalone;

            var initialPoint = _pointHolder.GetInitialPoint();
            var player = ResourcesUtilities.Instantiate(playerPrefabPath, initialPoint);
            player.transform.LookAt(Vector3.zero);

            if (isLocal)
            {
                player.GetComponent<PlayerStateSender>().Construct(_networkService, _inputService);
                player.GetComponent<PlayerMovement>().Construct(_inputService);
                CreateLocalPlayerGun(player);
            }
            else
            {
                GunShooting gunMono = CreateGun(player);

                var networkData = new RemotePlayer(user);
                player.GetComponent<RemotePlayerSync>().Construct(networkData, gunMono);
                player.GetComponent<RemotePlayerSender>().Construct(roundService, networkData);
            }

            _players.Add(user.UserId, player);
        }

        private void CreateLocalPlayerGun(GameObject player)
        {
            GunShooting gunMono = CreateGun(player);
            gunMono.Construct(_inputService);
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
                if (_players.ContainsKey(presence.UserId))
                {
                    RemovePlayer(presence.UserId);
                }
            }
        }

        public void RemovePlayer(string userId)
        {
            Debug.LogError("Remove player");
            var player = _players[userId];
            _players.Remove(userId);
            Object.Destroy(player);
        }

        // Todo shouldn't destroy player
        public void DeactivatePlayer(string userId)
        {
            Debug.LogError("Deactivate Player");
            var player = _players[userId];
            _players.Remove(userId);
            Object.Destroy(player, 1.5f);
        }

        public void UpdatePlayersState(IMatchState state)
        {
            // Debug.LogError("UpdatePlayersState");

            string userId = state.UserPresence.UserId;

            if (!_players.ContainsKey(userId))
            {
                // Debug.LogError("State contains unknown SessionId!");
                return;
            }

            if (IsRemotePlayer(userId))
            {
                _players[userId].GetComponent<RemotePlayerSync>().UpdateState(state);
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


        private bool IsRemotePlayer(string userId)
        {
            return userId != LocalUserId;
        }
    }
}