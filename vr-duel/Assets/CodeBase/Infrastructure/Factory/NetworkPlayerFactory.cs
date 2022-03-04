using System.Collections.Generic;
using System.Linq;
using CodeBase.Behaviours.Guns;
using CodeBase.Infrastructure.Utilities;
using CodeBase.Player;
using CodeBase.Player.Remote;
using CodeBase.Services.Input;
using CodeBase.Services.Network;
using Nakama;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class NetworkPlayerFactory : INetworkPlayerFactory
    {
        private INetworkService _networkService;
        private InitialPointHolder _pointHolder;

        Dictionary<string, GameObject> _players = new Dictionary<string, GameObject>();

        private IMatchmakerUser _localUser;
        private IInputEventService _inputEventService;

        public NetworkPlayerFactory(INetworkService networkService, IInputEventService inputEventService)
        {
            _networkService = networkService;
            _inputEventService = inputEventService;
            _pointHolder = new InitialPointHolder();
        }

        public void SubscribeEvents()
        {
            _networkService.ReceivedMatchmakerMatched += CacheLocalUser;
            _networkService.MatchJoined += InstantiatePlayers;
            _networkService.ReceivedMatchPresence += UpdatePlayers;
            _networkService.ReceivedMatchState += UpdatePlayersState;
        }

        public void Cleanup()
        {
            _networkService.ReceivedMatchmakerMatched -= CacheLocalUser;
            _networkService.MatchJoined -= InstantiatePlayers;
            _networkService.ReceivedMatchPresence -= UpdatePlayers;
            _networkService.ReceivedMatchState -= UpdatePlayersState;
        }

        private void CacheLocalUser(IMatchmakerMatched matched)
        {
            _localUser = matched.Self;
        }

        private void InstantiatePlayers(IMatch match)
        {
            Debug.LogError("FirstInstantiatePlayers");
            Debug.LogError($"_networkService.Match.Presences.Count() : {match.Presences.Count()}");
            Debug.LogError($"MatchID : {match.Id}");

            foreach (var user in match.Presences)
            {
                SpawnPlayer(match.Id, user);
            }
        }

        private void SpawnPlayer(string matchId, IUserPresence user)
        {
            Debug.LogError("Spawn Player");
            var isLocal = user.SessionId == _localUser.Presence.SessionId;
            Debug.LogError($"Is local : {isLocal} ; user.SessionId =  {user.SessionId} ; localUser.SessionId {_localUser.Presence.SessionId}");
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
                player.GetComponent<RemotePlayerSync>().Construct(new RemotePlayerNetworkData(matchId, user), gunMono);
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


        private void UpdatePlayers(IMatchPresenceEvent matchPresenceEvent)
        {
            Debug.LogError("Start Update Players");

            foreach (var presence in matchPresenceEvent.Joins)
            {
                SpawnPlayer(matchPresenceEvent.MatchId, presence);
            }

            foreach (var presence in matchPresenceEvent.Leaves)
            {
                if (_players.ContainsKey(presence.SessionId))
                {
                    RemovePlayer(presence);
                }
            }

            Debug.LogError("End Update Players");
        }

        private void RemovePlayer(IUserPresence presence)
        {
            Debug.LogError("Remove player");
            _players[presence.SessionId].SetActive(false);
            _players.Remove(presence.SessionId);
        }

        private void UpdatePlayersState(IMatchState state)
        {
            Debug.LogError("UpdatePlayersState");
            foreach (var pair in _players)
            {
                var sessionId = pair.Key;
                var playerObj = pair.Value;

                if (sessionId != _localUser.Presence.SessionId)
                {
                    Debug.LogError("Update state for player session id = " + sessionId);
                    playerObj.GetComponent<RemotePlayerSync>().UpdateState(state);
                }
            }
        }
    }
}